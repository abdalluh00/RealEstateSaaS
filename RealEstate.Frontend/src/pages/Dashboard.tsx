// src/pages/Dashboard.tsx
import { useState, useEffect } from "react";
import {
    Building2, Users, FileText, CreditCard,
    AlertTriangle, Calendar, Clock,  // ← احذف TrendingUp
    type LucideIcon
} from "lucide-react";
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid,
    Tooltip, ResponsiveContainer
} from "recharts";
import { dashboardService } from "../services/dashboard.service";
import { type DashboardStats } from "../types/dashboard";
import { useAuthStore } from "../store/authStore";

// مكون الإحصائية
function StatCard({
    title, value, icon: Icon, color, subtitle
}: {
    title:    string;
    value:    string | number;
    icon:     LucideIcon;
    color:    string;
    subtitle?: string;
}) {
    return (
        <div className="card">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-gray-500 text-sm">{title}</p>
                    <p className="text-2xl font-bold text-gray-900 mt-1">{value}</p>
                    {subtitle && (
                        <p className="text-xs text-gray-400 mt-1">{subtitle}</p>
                    )}
                </div>
                <div className={`w-12 h-12 ${color} rounded-xl
                                flex items-center justify-center`}>
                    <Icon className="w-6 h-6 text-white" />
                </div>
            </div>
        </div>
    );
}

export default function Dashboard() {
    const user = useAuthStore(s => s.user);
    const [stats, setStats]     = useState<DashboardStats | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        dashboardService.getStats()
            .then(setStats)
            .finally(() => setLoading(false));
    }, []);

    if (loading) return (
        <div className="flex items-center justify-center h-96">
            <div className="animate-spin rounded-full h-12 w-12
                            border-4 border-primary-600 border-t-transparent"/>
        </div>
    );

    if (!stats) return null;

    const chartData = [
        { name: "متاح",  value: stats.availableProperties, fill: "#22c55e" },
        { name: "مؤجر",  value: stats.rentedProperties,    fill: "#3b82f6" },
        { name: "مباع",  value: stats.soldProperties,      fill: "#8b5cf6" },
    ];

    return (
        <div className="p-6 space-y-6" dir="rtl">

            {/* Header */}
            <div>
                <h1 className="text-2xl font-bold text-gray-900">
                    مرحباً، {user?.fullName} 👋
                </h1>
                <p className="text-gray-500 mt-1">
                    إليك نظرة عامة على أداء مكتبك اليوم
                </p>
            </div>

            {/* Stats Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                <StatCard
                    title="إجمالي العقارات"
                    value={stats.totalProperties}
                    icon={Building2}
                    color="bg-blue-600"
                    subtitle={`${stats.availableProperties} متاح`}
                />
                <StatCard
                    title="إجمالي العملاء"
                    value={stats.totalClients}
                    icon={Users}
                    color="bg-green-600"
                    subtitle={`${stats.hotLeads} عميل ساخن`}
                />
                <StatCard
                    title="العقود النشطة"
                    value={stats.activeContracts}
                    icon={FileText}
                    color="bg-purple-600"
                    subtitle={`${stats.expiringIn30Days} تنتهي قريباً`}
                />
                <StatCard
                    title="الإيرادات هذا الشهر"
                    value={`${stats.collectedThisMonth.toLocaleString()} ﷼`}
                    icon={CreditCard}
                    color="bg-orange-600"
                    subtitle={`${stats.overdueCount} دفعة متأخرة`}
                />
            </div>

            {/* Second Row */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">

                {/* Chart */}
                <div className="card lg:col-span-2">
                    <h3 className="font-bold text-gray-900 mb-4">
                        توزيع العقارات
                    </h3>
                    <ResponsiveContainer width="100%" height={200}>
                        <BarChart data={chartData}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="name" />
                            <YAxis />
                            <Tooltip />
                            <Bar dataKey="value" fill="#3b82f6" radius={[4,4,0,0]} />
                        </BarChart>
                    </ResponsiveContainer>
                </div>

                {/* Alerts */}
                <div className="card">
                    <h3 className="font-bold text-gray-900 mb-4">تنبيهات</h3>
                    <div className="space-y-3">
                        {stats.overdueCount > 0 && (
                            <div className="flex items-center gap-3 p-3
                                            bg-red-50 rounded-lg">
                                <AlertTriangle className="w-5 h-5 text-red-500" />
                                <div>
                                    <p className="text-sm font-medium text-red-700">
                                        {stats.overdueCount} دفعة متأخرة
                                    </p>
                                    <p className="text-xs text-red-500">
                                        {stats.overdueAmount.toLocaleString()} ﷼
                                    </p>
                                </div>
                            </div>
                        )}
                        {stats.expiringIn30Days > 0 && (
                            <div className="flex items-center gap-3 p-3
                                            bg-yellow-50 rounded-lg">
                                <Clock className="w-5 h-5 text-yellow-500" />
                                <div>
                                    <p className="text-sm font-medium text-yellow-700">
                                        {stats.expiringIn30Days} عقد ينتهي قريباً
                                    </p>
                                    <p className="text-xs text-yellow-500">
                                        خلال 30 يوم
                                    </p>
                                </div>
                            </div>
                        )}
                        <div className="flex items-center gap-3 p-3
                                        bg-blue-50 rounded-lg">
                            <Calendar className="w-5 h-5 text-blue-500" />
                            <div>
                                <p className="text-sm font-medium text-blue-700">
                                    {stats.todayAppointments} موعد اليوم
                                </p>
                                <p className="text-xs text-blue-500">
                                    {stats.pendingAppointments} قيد الانتظار
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Recent Contracts + Top Agents */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">

                {/* Recent Contracts */}
                <div className="card">
                    <h3 className="font-bold text-gray-900 mb-4">
                        آخر العقود
                    </h3>
                    <div className="space-y-3">
                        {stats.recentContracts.map(contract => (
                            <div key={contract.id}
                                 className="flex items-center justify-between
                                            p-3 bg-gray-50 rounded-lg">
                                <div>
                                    <p className="text-sm font-medium text-gray-900">
                                        {contract.propertyTitle}
                                    </p>
                                    <p className="text-xs text-gray-500">
                                        {contract.clientName}
                                    </p>
                                </div>
                                <div className="text-left">
                                    <p className="text-sm font-bold text-gray-900">
                                        {contract.amount.toLocaleString()} ﷼
                                    </p>
                                    <span className={
                                        contract.status === "Active"
                                            ? "badge-green"
                                            : "badge-yellow"
                                    }>
                                        {contract.status === "Active" ? "نشط" : "منتهي"}
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Top Agents */}
                <div className="card">
                    <h3 className="font-bold text-gray-900 mb-4">
                        أفضل الوكلاء
                    </h3>
                    <div className="space-y-3">
                        {stats.topAgents.map((agent, index) => (
                            <div key={agent.fullName}
                                 className="flex items-center gap-3">
                                <div className="w-8 h-8 bg-primary-100 rounded-full
                                                flex items-center justify-center
                                                text-primary-700 font-bold text-sm">
                                    {index + 1}
                                </div>
                                <div className="flex-1">
                                    <p className="text-sm font-medium text-gray-900">
                                        {agent.fullName}
                                    </p>
                                    <p className="text-xs text-gray-500">
                                        {agent.totalContracts} عقد
                                    </p>
                                </div>
                                <p className="text-sm font-bold text-green-600">
                                    {agent.totalCommission.toLocaleString()} ﷼
                                </p>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}