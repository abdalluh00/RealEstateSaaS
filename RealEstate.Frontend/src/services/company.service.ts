// src/services/company.service.ts
import api from "./api";

export interface CompanyDetail {
    id:                 string;
    name:               string;
    phone:              string;
    logo:               string | null;
    address:            string | null;
    subscriptionPlan:   string;
    subscriptionExpiry: string;
    isActive:           boolean;
    isExpired:          boolean;
    totalUsers:         number;
    totalProperties:    number;
    totalClients:       number;
    totalContracts:     number;
    activeContracts:    number;
    daysLeft:           number; 
}

export const companyService = {
    async get(): Promise<CompanyDetail> {
        const { data } = await api.get("/companies/me");
        return data.data;
    },

    async update(payload: {
        name:    string;
        phone:   string;
        address: string | null;
        logo:    string | null;
    }) {
        const { data } = await api.put("/companies/me", payload);
        return data;
    }
};