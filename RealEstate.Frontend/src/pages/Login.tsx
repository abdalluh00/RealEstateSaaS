// src/pages/Login.tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { authService } from "../services/auth.service";
import { useAuthStore } from "../store/authStore";
import { Building2, Mail, Lock, Eye, EyeOff } from "lucide-react";

const schema = z.object({
    email:    z.string().email("البريد الإلكتروني غير صحيح"),
    password: z.string().min(1, "كلمة المرور مطلوبة")
});

type LoginForm = z.infer<typeof schema>;

export default function Login() {
    const navigate  = useNavigate();
    const setUser   = useAuthStore(s => s.setUser);
    const [error, setError]     = useState("");
    const [loading, setLoading] = useState(false);
    const [showPass, setShowPass] = useState(false);

    const { register, handleSubmit, formState: { errors } } =
        useForm<LoginForm>({ resolver: zodResolver(schema) });

    async function onSubmit(form: LoginForm) {
        setLoading(true);
        setError("");
        try {
            const user = await authService.login(form);
            setUser(user);
            navigate("/dashboard");
        } catch {
            setError("البريد الإلكتروني أو كلمة المرور غير صحيحة");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-linear-to-br from-primary-900 to-primary-700
                        flex items-center justify-center p-4">
            <div className="bg-white rounded-2xl shadow-2xl w-full max-w-md p-8">

                {/* Logo */}
                <div className="text-center mb-8">
                    <div className="inline-flex items-center justify-center
                                    w-16 h-16 bg-primary-600 rounded-2xl mb-4">
                        <Building2 className="w-8 h-8 text-white" />
                    </div>
                    <h1 className="text-2xl font-bold text-gray-900">
                        نظام إدارة العقارات
                    </h1>
                    <p className="text-gray-500 mt-1">سجّل دخولك للمتابعة</p>
                </div>

                {/* Error */}
                {error && (
                    <div className="bg-red-50 border border-red-200 text-red-700
                                    rounded-lg p-3 mb-4 text-sm text-center">
                        {error}
                    </div>
                )}

                {/* Form */}
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">

                    {/* Email */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            البريد الإلكتروني
                        </label>
                        <div className="relative">
                            <Mail className="absolute right-3 top-2.5 w-4 h-4 text-gray-400" />
                            <input
                                {...register("email")}
                                type="email"
                                placeholder="example@company.com"
                                className="input pr-10"
                            />
                        </div>
                        {errors.email && (
                            <p className="text-red-500 text-xs mt-1">
                                {errors.email.message}
                            </p>
                        )}
                    </div>

                    {/* Password */}
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            كلمة المرور
                        </label>
                        <div className="relative">
                            <Lock className="absolute right-3 top-2.5 w-4 h-4 text-gray-400" />
                            <input
                                {...register("password")}
                                type={showPass ? "text" : "password"}
                                placeholder="••••••••"
                                className="input pr-10 pl-10"
                            />
                            <button
                                type="button"
                                onClick={() => setShowPass(!showPass)}
                                className="absolute left-3 top-2.5 text-gray-400">
                                {showPass
                                    ? <EyeOff className="w-4 h-4" />
                                    : <Eye className="w-4 h-4" />
                                }
                            </button>
                        </div>
                        {errors.password && (
                            <p className="text-red-500 text-xs mt-1">
                                {errors.password.message}
                            </p>
                        )}
                    </div>

                    {/* Submit */}
                    <button
                        type="submit"
                        disabled={loading}
                        className="btn-primary w-full py-3 text-base mt-2">
                        {loading ? "جاري تسجيل الدخول..." : "تسجيل الدخول"}
                    </button>
                </form>
            </div>
        </div>
    );
}