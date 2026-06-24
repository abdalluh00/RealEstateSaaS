// src/services/contracts.service.ts
import api from "./api";

export interface Contract {
    id:            string;
    propertyTitle: string;
    clientName:    string;
    clientPhone:   string;
    agentName:     string;
    contractType:  string;
    status:        string;
    amount:        number;
    commission:    number;
    startDate:     string;
    endDate:       string;
    totalPayments: number;
    paidPayments:  number;
    totalPaid:     number;
    remaining:     number;
}

export interface CreateContractRequest {
    propertyId:             string;
    clientId:               string;
    agentId:                string;
    contractType:           string;
    amount:                 number;
    commission:             number;
    startDate:              string;
    endDate:                string;
    notes?:                 string;
    paymentIntervalMonths:  number;
}

export const contractsService = {
    async getAll() {
        const { data } = await api.get("/contracts");
        return data.data ?? [];
    },

    async getById(id: string) {
        const { data } = await api.get(`/contracts/detail/${id}`);
        return data.data;
    },

    async create(request: CreateContractRequest) {
        const { data } = await api.post("/contracts", request);
        return data;
    },

    async markPaid(paymentId: string, method: string, reference?: string) {
        const { data } = await api.put(`/payments/pay/${paymentId}`, {
            paymentId,
            method,
            reference: reference || null
        });
        return data;
    }
};