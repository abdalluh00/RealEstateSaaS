// src/services/auth.service.ts
import api from "./api";
import type { LoginRequest, AuthUser } from "../types/auth";

export const authService = {
    async login(request: LoginRequest): Promise<AuthUser> {
        const { data } = await api.post("/auth/login", request);
        return data.data;
    },

    async getMe(): Promise<AuthUser> {
        const { data } = await api.get("/auth/me");
        return data.data;
    },

    logout() {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
    }
};