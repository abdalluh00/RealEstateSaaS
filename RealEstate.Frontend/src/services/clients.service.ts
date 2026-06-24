// src/services/clients.service.ts
import api from "./api";

export interface Client {
    id:         string;
    fullName:   string;
    phone:      string;
    email:      string | null;
    leadStatus: string;
    source:     string | null;
    createdAt:  string;
}

export const clientsService = {
   async getAll(leadStatus?: string) {
    const { data } = await api.get("/clients", { params: leadStatus ? { leadStatus } : {} });
    return data.data ?? [];
},

    async create(request: Omit<Client, "id" | "createdAt">) {
        const { data } = await api.post("/clients", request);
        return data;
    },

    async update(id: string, request: Omit<Client, "id" | "createdAt">) {
        const { data } = await api.put(`/clients/${id}`, { id, ...request });
        return data;
    },

    async delete(id: string) {
        const { data } = await api.delete(`/clients/${id}`);
        return data;
    }
};