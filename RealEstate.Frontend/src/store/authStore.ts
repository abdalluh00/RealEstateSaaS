// src/store/authStore.ts
import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { AuthUser } from "../types/auth";

interface AuthState {
    user:      AuthUser | null;
    token:     string | null;
    isLoggedIn: boolean;
    setUser:   (user: AuthUser) => void;
    logout:    () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            user:       null,
            token:      null,
            isLoggedIn: false,

            setUser: (user: AuthUser) => {
                localStorage.setItem("token", user.token);
                set({ user, token: user.token, isLoggedIn: true });
            },

            logout: () => {
                localStorage.removeItem("token");
                set({ user: null, token: null, isLoggedIn: false });
            }
        }),
        { name: "auth-storage" }
    )
);