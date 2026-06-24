// src/services/owners.service.ts
import api from "./api";
export interface Owner {
    id:              string;
    fullName:        string;
    phone:           string;
    email?:          string | null;
    idNumber?:       string | null;
    totalProperties: number;
    activeContracts: number;
    createdAt:       string;
}

export interface OwnerProperty {
    id:     string;
    title:  string;
    type:   string;
    status: string;
    price:  number;
    city:   string;
}

export interface OwnerDetail {
    id:         string;
    fullName:   string;
    phone:      string;
    email?:     string | null;
    idNumber?:  string | null;
    notes?:     string | null;
    properties: OwnerProperty[];
}

export interface CreateOwnerRequest {
    fullName:  string;
    phone:     string;
    email?:    string;
    idNumber?: string;
    notes?:    string;
    companyId: string;
}

export interface UpdateOwnerRequest extends CreateOwnerRequest {
    id: string;
}



export const ownersService = {
    getAll: async (): Promise<Owner[]> => {
        const { data } = await api.get("/owners"); 
        return data.data ?? [];
    },


     getById: async (id: string): Promise<OwnerDetail> => {
        const { data } = await api.get(`/owners/detail/${id}`);
        return data.data;
    },

    create: async (payload: Omit<CreateOwnerRequest, "companyId">) => {
        const { data } = await api.post("/owners", payload); 
        return data;
    },

    update: async (payload: UpdateOwnerRequest) => {
        const { data } = await api.put(`/owners/${payload.id}`, payload);
        return data;
    },

    remove: async (id: string) => {
        const { data } = await api.delete(`/owners/${id}`);
        return data;
    }
};