// src/services/users.service.ts
import api from "./api";

export interface User {
    id:        string;
    fullName:  string;
    email:     string;
    phone:     string;
    role:      string;
    isActive:  boolean;
    createdAt: string;
}

export const usersService = {
    async getAll(): Promise<User[]> {
        const { data } = await api.get("/users");
        return data.data ?? [];
    },

    async create(payload: {
        fullName:  string;
        email:     string;
        phone:     string;
        password:  string;
        role:      string;
    }) {
        const { data } = await api.post("/users", payload);
        return data;
    },

    async update(id: string, payload: {
        fullName: string;
        phone:    string;
        role:     string;
        isActive: boolean;
    }) {
        const { data } = await api.put(`/users/${id}`, { id, ...payload });
        return data;
    },

    async changePassword(userId: string, oldPassword: string, newPassword: string) {
        const { data } = await api.put(`/users/${userId}/change-password`, {
            userId, oldPassword, newPassword
        });
        return data;
    },

    async delete(id: string) {
        const { data } = await api.delete(`/users/${id}`);
        return data;
    }
};