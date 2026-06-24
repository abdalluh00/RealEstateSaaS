// src/components/Layout.tsx
import { Outlet, Navigate } from "react-router-dom";
import Sidebar from "./Sidebar";
import { useAuthStore } from "../store/authStore";

export default function Layout() {
    const isLoggedIn = useAuthStore(s => s.isLoggedIn);

    if (!isLoggedIn) return <Navigate to="/login" replace />;

    return (
        <div className="flex min-h-screen bg-gray-50" dir="rtl">
            <Sidebar />
            <main className="flex-1 overflow-auto">
                <Outlet />
            </main>
        </div>
    );
}