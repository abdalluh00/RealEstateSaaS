export interface DashboardStats {
    totalProperties:     number;
    availableProperties: number;
    rentedProperties:    number;
    soldProperties:      number;
    totalClients:        number;
    hotLeads:            number;
    newLeadsThisMonth:   number;
    totalContracts:      number;
    activeContracts:     number;
    expiringIn30Days:    number;
    totalRevenue:        number;
    collectedThisMonth:  number;
    overdueAmount:       number;
    overdueCount:        number;
    todayAppointments:   number;
    pendingAppointments: number;
    topAgents:           AgentPerformance[];
    recentContracts:     RecentContract[];
}

export interface AgentPerformance {
    fullName:        string;
    totalContracts:  number;
    totalCommission: number;
}

export interface RecentContract {
    id:            string;
    propertyTitle: string;
    clientName:    string;
    amount:        number;
    status:        string;
    createdAt:     string;
}