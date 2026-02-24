"use client";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { changePasswordSchema, ChangePasswordRequest } from "@/features/auth/changePassword/schema";
import { useChangePassword } from "@/features/auth/changePassword/hooks";
import Link from "next/link";
import { useState } from "react";

export default function ChangePasswordPage() {
  const { register, handleSubmit, formState: { errors }, reset } = useForm<ChangePasswordRequest>({
    resolver: zodResolver(changePasswordSchema),
  });
  
  const { mutate: changePassword, isPending } = useChangePassword();
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  const onSubmit = (data: ChangePasswordRequest) => {
    setErrorMsg(null);
    changePassword(data, {
      onError: (error: any) => {
        // If current password is wrong -> return 400 with error INVALID_CURRENT_PASSWORD based on spec
        if (error.response?.data?.code === "INVALID_CURRENT_PASSWORD") {
          setErrorMsg("The current password provided is incorrect.");
        } else {
          // General validation or other error
          const details = error.response?.data?.errors;
          if (details) {
            const firstErrorList = Object.values(details)[0] as string[];
            setErrorMsg(firstErrorList?.[0] || "Validation failed.");
          } else {
            setErrorMsg(error.response?.data?.detail || error.message || "Failed to change password.");
          }
        }
      }
    });
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-slate-50 p-6">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-xl border border-slate-100 p-8 space-y-8">
        <div className="text-center space-y-2">
          <h2 className="text-3xl font-extrabold text-slate-900 tracking-tight">Change Password</h2>
          <p className="text-slate-500">Secure your account with a new password</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          {errorMsg && (
            <div className="p-3 bg-red-50 text-red-700 rounded-lg text-sm font-medium text-center border border-red-100">
              {errorMsg}
            </div>
          )}
          
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1">Current Password</label>
              <input
                type="password"
                {...register("currentPassword")}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-indigo-600 outline-none transition-colors"
                placeholder="Current password"
              />
              {errors.currentPassword && <p className="mt-1 text-sm text-red-600">{errors.currentPassword.message}</p>}
            </div>

            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1">New Password</label>
              <input
                type="password"
                {...register("newPassword")}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-indigo-600 outline-none transition-colors"
                placeholder="New password (min 8 chars)"
              />
              {errors.newPassword && <p className="mt-1 text-sm text-red-600">{errors.newPassword.message}</p>}
            </div>

            <div>
              <label className="block text-sm font-medium text-slate-700 mb-1">Confirm New Password</label>
              <input
                type="password"
                {...register("confirmPassword")}
                className="w-full px-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-indigo-600 focus:border-indigo-600 outline-none transition-colors"
                placeholder="Confirm new password"
              />
              {errors.confirmPassword && <p className="mt-1 text-sm text-red-600">{errors.confirmPassword.message}</p>}
            </div>
          </div>

          <button
            type="submit"
            disabled={isPending}
            className="w-full bg-indigo-600 text-white font-semibold py-2.5 rounded-lg hover:bg-indigo-700 transition-colors disabled:opacity-70 disabled:cursor-not-allowed shadow-sm"
          >
            {isPending ? "Updating..." : "Update Password"}
          </button>
        </form>

        <p className="text-center text-sm text-slate-500">
          <Link href="/" className="text-indigo-600 hover:text-indigo-800 font-medium">
            ← Cancel and return home
          </Link>
        </p>
      </div>
    </div>
  );
}
