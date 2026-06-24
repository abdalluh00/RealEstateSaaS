// src/services/properties.service.ts
import api from "./api";

export interface Property {
    id:         string;
    title:      string;
    type:       string;
    status:     string;
    price:      number;
    area:       number;
    bedrooms:   number | null;
    bathrooms:  number | null;
    city:       string;
    district:   string;
    isFeatured: boolean;
    createdAt:  string;
}

export interface CreatePropertyRequest {
    title:        string;
    type:         string;
    price:        number;
    area:         number;
    bedrooms?:    number;
    bathrooms?:   number;
    city:         string;
    district:     string;
    description?: string;
}

export const propertiesService = {
    async getAll(page = 1, pageSize = 20, status?: string) {
        const params: Record<string, unknown> = { page, pageSize };
        if (status) params.status = status;
        const { data } = await api.get("/properties", { params });
        return data.data;
    },

    async create(request: CreatePropertyRequest) {
        const { data } = await api.post("/properties", request);
        return data;
    },

   async update(id: string, request: CreatePropertyRequest) {
    const { data } = await api.put(`/properties/${id}`, {
        id,
        ...request,
        status:     "Available", // ← مطلوب في الـ Command
        address:    null,
        isFeatured: false
    });
    return data;
},

    async delete(id: string) {
        const { data } = await api.delete(`/properties/${id}`);
        return data;
    }
};