using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Api.Domain;
using MyProject.Api.Features.Auth.ChangePassword;
using MyProject.Api.Infrastructure.Persistence;
using MyProject.Api.Infrastructure.Auth;

namespace MyProject.Tests.Features.Auth;

public class ChangePasswordTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ChangePasswordTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // In a real scenario, use Testcontainers. Here we replace with InMemory for simplicity
                // to avoid Docker dependency in the pipeline if not available.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);
                
                services.AddDbContext<AppDbContext>(options => 
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
    }

    [Fact]
    public async Task ChangePassword_WithValidData_Returns200OK()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtTokenProvider>();

        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("OldPassword123!"),
            FirstName = "Test",
            LastName = "User"
        };
        db.Users.Add(user);
        
        var token = new RefreshToken
        {
            UserId = user.Id,
            Token = "some-token",
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
            IsRevoked = false
        };
        db.RefreshTokens.Add(token);
        await db.SaveChangesAsync();

        var accessToken = jwtProvider.Generate(user);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var request = new ChangePasswordRequest("OldPassword123!", "NewPassword456!", "NewPassword456!");

        // Act
        var response = await client.PutAsJsonAsync("/api/v1/auth/password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUser = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);
        BCrypt.Net.BCrypt.Verify("NewPassword456!", updatedUser!.PasswordHash).Should().BeTrue();

        var updatedToken = await db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.Id == token.Id);
        updatedToken!.IsRevoked.Should().BeTrue();
    }

    [Fact]
    public async Task ChangePassword_WithInvalidCurrentPassword_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtTokenProvider>();

        var user = new User
        {
            Email = "test2@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("OldPassword123!"),
            FirstName = "Test",
            LastName = "User"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var accessToken = jwtProvider.Generate(user);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var request = new ChangePasswordRequest("WrongPassword!", "NewPassword456!", "NewPassword456!");

        // Act
        var response = await client.PutAsJsonAsync("/api/v1/auth/password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("INVALID_CURRENT_PASSWORD");
    }

    [Fact]
    public async Task ChangePassword_WithMismatchedPasswords_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtTokenProvider>();

        var user = new User
        {
            Email = "test3@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("OldPassword123!"),
            FirstName = "Test",
            LastName = "User"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var accessToken = jwtProvider.Generate(user);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var request = new ChangePasswordRequest("OldPassword123!", "NewPassword456!", "DifferentPassword!");

        // Act
        var response = await client.PutAsJsonAsync("/api/v1/auth/password", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Passwords do not match");
    }
}
