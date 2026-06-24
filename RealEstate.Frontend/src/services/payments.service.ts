// src/services/payments.service.ts
import api from "./api";

export interface Payment {
    id:            string;
    propertyTitle: string;
    clientName:    string;
    clientPhone:   string;
    amount:        number;
    dueDate:       string;
    paidDate:      string | null;
    status:        string;
    method:        string | null;
    reference:     string | null;
    contractId:    string;
    daysOverdue:   number;
}

export interface PaymentSummary {
    totalExpected:    number;
    totalCollected:   number;
    totalOverdue:     number;
    totalPending:     number;
    overdueCount:     number;
    pendingCount:     number;
}

export const paymentsService = {
    async getOverdue() {
        const { data } = await api.get("/payments/overdue");
        return data.data ?? [];
    },

    async getUpcoming(daysAhead = 30) {
        const { data } = await api.get("/payments/upcoming", {
            params: { daysAhead }
        });
        return data.data ?? [];
    },

    async getSummary() {
        const { data } = await api.get("/payments/summary");
        return data.data;
    },

    async markPaid(paymentId: string, method: string, reference?: string) {
        const { data } = await api.put(`/payments/pay/${paymentId}`, {
            paymentId,
            method,
            reference: reference || null
        });
        return data;
    },

    async cancel(paymentId: string) {
        const { data } = await api.put(`/payments/cancel/${paymentId}`);
        return data;
    }
};