// src/services/appointments.service.ts
import api from "./api";

export interface Appointment {
    id:            string;
    propertyTitle: string;
    propertyCity:  string;
    clientName:    string;
    clientPhone:   string;
    agentName:     string;
    scheduledAt:   string;
    status:        string;
    notes:         string | null;
    feedback:      string | null;
}

export const appointmentsService = {
    async getAll(todayOnly = false) {
        const { data } = await api.get("/appointments", {
            params: { todayOnly }
        });
        return data.data ?? [];
    },

    async create(request: {
        propertyId:  string;
        clientId:    string;
        agentId:     string;
        scheduledAt: string;
        notes?:      string;
    }) {
        const { data } = await api.post("/appointments", request);
        return data;
    },

    async updateStatus(id: string, status: string, feedback?: string) {
        const { data } = await api.put(`/appointments/${id}/status`, {
            id, status, feedback: feedback || null
        });
        return data;
    },

    async delete(id: string) {
        const { data } = await api.delete(`/appointments/${id}`);
        return data;
    }
};