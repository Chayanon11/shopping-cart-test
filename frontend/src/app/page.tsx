"use client";

import Link from "next/link";
import { useAuthStore } from "@/shared/store/authStore";

export default function LandingPage() {
  const { isAuthenticated, user, logout } = useAuthStore();

  return (
    <main className="min-h-screen bg-slate-50 flex flex-col items-center justify-center p-6">
      <div className="max-w-2xl text-center space-y-8">
        <h1 className="text-5xl font-extrabold text-slate-900 tracking-tight">
          E-Commerce - Shopping Cart
        </h1>
        <p className="text-lg text-slate-600">
          Shopping Cart
        </p>

        <div className="pt-8">
          {isAuthenticated ? (
            <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex flex-col items-center space-y-6">
              <div className="space-y-2">
                <p className="text-xl font-semibold text-slate-800">
                  Hello, {user?.firstName} {user?.lastName}!
                </p>
                <p className="text-slate-500">{user?.email}</p>
              </div>
              
  
            </div>
          ) : (
            <div className="flex justify-center gap-4">
              <Link
                href="/products"
                className="px-8 py-3 bg-indigo-600 text-white font-semibold rounded-xl hover:bg-indigo-700 transition-colors shadow-sm"
              >
                Click Here to see out Products
              </Link>
            </div>
          )}
        </div>
      </div>
    </main>
  );
}
