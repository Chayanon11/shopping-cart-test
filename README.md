# dotnet-next shopping cart test


## What you need
- Node 18+
- .NET 8 SDK
- Postgres running locally (or tweak the connection string)

## How to run 


### 1. Spin up the Backend (API)

Head over to the API project, restore packages, run the migrations to seed some mockup data, and fire it up:
```bash
cd backend/src/MyProject.Api
dotnet restore
dotnet ef database update
dotnet run
```
> The API should be listening on `http://localhost:5242`

*(Note: If your local Postgres isn't using the default postgres/postgres credentials on port 5432, you'll need to update `appsettings.Development.json` first).*

### 2. Start the Frontend (Next.js)

```bash
cd frontend
npm i
npm run dev
```
> UI will be up at `http://localhost:3000`
