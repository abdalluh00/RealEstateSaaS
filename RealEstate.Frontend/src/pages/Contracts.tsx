import { useState, useEffect, useCallback } from "react";
import {
    FileText, Plus, Search, Eye, CheckCircle
} from "lucide-react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { contractsService, type Contract, type CreateContractRequest }
    from "../services/contracts.service";
import { propertiesService, type Property }
    from "../services/properties.service";
import { clientsService, type Client }
    from "../services/clients.service";
import api from "../services/api";

// ── Helpers ───────────────────────────────────────────────
const statusLabel: Record<string, string> = {
    Active:    "نشط",
    Expired:   "منتهي",
    Cancelled: "ملغي",
    Renewed:   "مجدد"
};

const statusColor: Record<string, string> = {
    Active:    "#16a34a",
    Expired:   "#dc2626",
    Cancelled: "#64748b",
    Renewed:   "#2563eb"
};

const typeLabel: Record<string, string> = {
    Rent: "إيجار",
    Sale: "بيع"
};

// ── Schema ────────────────────────────────────────────────
const schema = z.object({
    propertyId:            z.string().min(1, "العقار مطلوب"),
    clientId:              z.string().min(1, "العميل مطلوب"),
    agentId:               z.string().min(1, "الوكيل مطلوب"),
    contractType:          z.string().min(1, "نوع العقد مطلوب"),
    amount:                z.preprocess(v => Number(v), z.number().min(1, "المبلغ مطلوب")),
    commission:            z.preprocess(v => Number(v), z.number().min(0)),
    startDate:             z.string().min(1, "تاريخ البداية مطلوب"),
    endDate:               z.string().min(1, "تاريخ النهاية مطلوب"),
    paymentIntervalMonths: z.preprocess(v => Number(v), z.number().min(1)),
    notes:                 z.string().optional()
});

// ── Payment type (moved to module scope) ───────────────────
interface Payment {
    id:          string;
    amount:      number;
    dueDate:     string;
    paidDate:    string | null;
    status:      string;
    method:      string | null;
    daysOverdue: number;
}

// ── Contract Card ──────────────────────────────────────────
function ContractCard({
    contract,
    onView
}: {
    contract: Contract;
    onView:   (c: Contract) => void;
}) {
    const progress = contract.totalPayments > 0
        ? Math.round((contract.paidPayments / contract.totalPayments) * 100)
        : 0;

    return (
        <div style={{
            background: "white",
            borderRadius: "16px",
            border: "1px solid #f1f5f9",
            padding: "20px",
            boxShadow: "0 1px 3px rgba(0,0,0,0.06)"
        }}>
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                <div>
                    <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", margin: "0 0 4px" }}>
                        {contract.propertyTitle}
                    </h3>
                    <p style={{ fontSize: "13px", color: "#64748b", margin: 0 }}>
                        {contract.clientName} — {contract.clientPhone}
                    </p>
                </div>
                <div style={{ display: "flex", flexDirection: "column", alignItems: "flex-end", gap: "4px" }}>
                    <span style={{
                        padding: "3px 10px", borderRadius: "20px",
                        fontSize: "12px", fontWeight: "600",
                        backgroundColor: `${statusColor[contract.status]}15`,
                        color: statusColor[contract.status]
                    }}>
                        {statusLabel[contract.status]}
                    </span>
                    <span style={{
                        padding: "3px 10px", borderRadius: "6px",
                        fontSize: "12px", backgroundColor: "#f1f5f9", color: "#475569"
                    }}>
                        {typeLabel[contract.contractType]}
                    </span>
                </div>
            </div>

            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                <div>
                    <p style={{ fontSize: "12px", color: "#94a3b8", margin: "0 0 2px" }}>قيمة العقد</p>
                    <p style={{ fontSize: "18px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                        {contract.amount.toLocaleString()} ﷼
                    </p>
                </div>
                <div style={{ textAlign: "left" }}>
                    <p style={{ fontSize: "12px", color: "#94a3b8", margin: "0 0 2px" }}>المتبقي</p>
                    <p style={{ fontSize: "18px", fontWeight: "700", color: contract.remaining > 0 ? "#dc2626" : "#16a34a", margin: 0 }}>
                        {contract.remaining.toLocaleString()} ﷼
                    </p>
                </div>
            </div>

            <div style={{ marginBottom: "16px" }}>
                <div style={{ display: "flex", justifyContent: "space-between", fontSize: "12px", color: "#64748b", marginBottom: "6px" }}>
                    <span>الدفعات: {contract.paidPayments}/{contract.totalPayments}</span>
                    <span>{progress}%</span>
                </div>
                <div style={{ height: "6px", backgroundColor: "#f1f5f9", borderRadius: "3px", overflow: "hidden" }}>
                    <div style={{
                        height: "100%",
                        width: `${progress}%`,
                        backgroundColor: progress === 100 ? "#16a34a" : "#2563eb",
                        borderRadius: "3px",
                        transition: "width 0.3s"
                    }} />
                </div>
            </div>

            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", borderTop: "1px solid #f1f5f9", paddingTop: "12px" }}>
                <div style={{ fontSize: "12px", color: "#94a3b8" }}>
                    {new Date(contract.startDate).toLocaleDateString("ar-SA")} —
                    {new Date(contract.endDate).toLocaleDateString("ar-SA")}
                </div>
                <button
                    onClick={() => onView(contract)}
                    style={{ display: "flex", alignItems: "center", gap: "6px", padding: "7px 14px", borderRadius: "8px", border: "none", backgroundColor: "#eff6ff", color: "#2563eb", cursor: "pointer", fontSize: "13px" }}>
                    <Eye size={14} />
                    التفاصيل
                </button>
            </div>
        </div>
    );
}

// ── Contract Detail Modal ──────────────────────────────────
function ContractDetailModal({
    contract,
    onClose
}: {
    contract: Contract | null;
    onClose:  () => void;
}) {
    const [payments, setPayments] = useState<Payment[]>([]);
    const [loading, setLoading]   = useState(false);
    const [payingId, setPayingId] = useState<string | null>(null);

    
    const loadPayments = useCallback(async () => {
        if (!contract) return;
        setLoading(true);
        try {
            const { data } = await api.get(`/payments/contract/${contract.id}`);
            setPayments(data.data ?? []);
        } finally {
            setLoading(false);
        }
    }, [contract]);

    useEffect(() => {
        if (contract) loadPayments();
    }, [contract, loadPayments]);

    async function handleMarkPaid(paymentId: string) {
        setPayingId(paymentId);
        try {
            await contractsService.markPaid(paymentId, "Cash");
            toast.success("تم تسجيل الدفعة بنجاح ✅");
            await loadPayments();
        } catch {
            toast.error("فشل تسجيل الدفعة ❌");
        } finally {
            setPayingId(null);
        }
    }

    if (!contract) return null;

    const paymentStatusColor: Record<string, string> = {
        Paid:      "#16a34a",
        Pending:   "#d97706",
        Late:      "#dc2626",
        Cancelled: "#64748b"
    };

    const paymentStatusLabel: Record<string, string> = {
        Paid:      "مدفوع",
        Pending:   "معلق",
        Late:      "متأخر",
        Cancelled: "ملغي"
    };

    return (
        <div style={{ position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", width: "600px", maxHeight: "90vh", overflowY: "auto" }}>

                <div style={{ padding: "24px 24px 0", borderBottom: "1px solid #f1f5f9", marginBottom: "20px" }}>
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", paddingBottom: "16px" }}>
                        <h2 style={{ fontSize: "18px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                            تفاصيل العقد
                        </h2>
                        <button
                            onClick={onClose}
                            style={{ background: "none", border: "none", cursor: "pointer", fontSize: "20px", color: "#94a3b8" }}>
                            ✕
                        </button>
                    </div>
                </div>

                <div style={{ padding: "0 24px 24px" }}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px", marginBottom: "24px" }}>
                        {[
                            { label: "العقار",        value: contract.propertyTitle },
                            { label: "العميل",        value: contract.clientName },
                            { label: "الوكيل",        value: contract.agentName },
                            { label: "نوع العقد",     value: typeLabel[contract.contractType] },
                            { label: "قيمة العقد",    value: `${contract.amount.toLocaleString()} ﷼` },
                            { label: "العمولة",       value: `${contract.commission.toLocaleString()} ﷼` },
                            { label: "تاريخ البداية", value: new Date(contract.startDate).toLocaleDateString("ar-SA") },
                            { label: "تاريخ النهاية", value: new Date(contract.endDate).toLocaleDateString("ar-SA") },
                        ].map(item => (
                            <div key={item.label} style={{ backgroundColor: "#f8fafc", borderRadius: "10px", padding: "12px" }}>
                                <p style={{ fontSize: "12px", color: "#94a3b8", margin: "0 0 4px" }}>{item.label}</p>
                                <p style={{ fontSize: "14px", fontWeight: "600", color: "#1e293b", margin: 0 }}>{item.value}</p>
                            </div>
                        ))}
                    </div>

                    <h3 style={{ fontSize: "15px", fontWeight: "700", color: "#1e293b", marginBottom: "12px" }}>
                        جدول الدفعات
                    </h3>

                    {loading ? (
                        <div style={{ textAlign: "center", padding: "20px", color: "#94a3b8" }}>جاري التحميل...</div>
                    ) : (
                        <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                            {payments.map((payment, index) => (
                                <div key={payment.id} style={{
                                    display: "flex", alignItems: "center", justifyContent: "space-between",
                                    padding: "12px 16px", borderRadius: "10px",
                                    backgroundColor: payment.status === "Paid" ? "#f0fdf4" : payment.status === "Late" ? "#fef2f2" : "#f8fafc",
                                    border: `1px solid ${payment.status === "Paid" ? "#bbf7d0" : payment.status === "Late" ? "#fecaca" : "#e2e8f0"}`
                                }}>
                                    <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                                        <div style={{
                                            width: "28px", height: "28px", borderRadius: "50%",
                                            backgroundColor: `${paymentStatusColor[payment.status]}20`,
                                            display: "flex", alignItems: "center", justifyContent: "center",
                                            fontSize: "12px", fontWeight: "700",
                                            color: paymentStatusColor[payment.status]
                                        }}>
                                            {index + 1}
                                        </div>
                                        <div>
                                            <p style={{ fontSize: "14px", fontWeight: "600", color: "#1e293b", margin: 0 }}>
                                                {payment.amount.toLocaleString()} ﷼
                                            </p>
                                            <p style={{ fontSize: "12px", color: "#94a3b8", margin: 0 }}>
                                                {new Date(payment.dueDate).toLocaleDateString("ar-SA")}
                                                {payment.daysOverdue > 0 && (
                                                    <span style={{ color: "#dc2626", marginRight: "8px" }}>
                                                        متأخر {payment.daysOverdue} يوم
                                                    </span>
                                                )}
                                            </p>
                                        </div>
                                    </div>

                                    <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                                        <span style={{
                                            padding: "3px 10px", borderRadius: "20px", fontSize: "12px",
                                            backgroundColor: `${paymentStatusColor[payment.status]}15`,
                                            color: paymentStatusColor[payment.status], fontWeight: "500"
                                        }}>
                                            {paymentStatusLabel[payment.status]}
                                        </span>
                                        {payment.status !== "Paid" && payment.status !== "Cancelled" && (
                                            <button
                                                onClick={() => handleMarkPaid(payment.id)}
                                                disabled={payingId === payment.id}
                                                style={{
                                                    display: "flex", alignItems: "center", gap: "4px",
                                                    padding: "5px 10px", borderRadius: "6px", border: "none",
                                                    backgroundColor: "#16a34a", color: "white",
                                                    cursor: "pointer", fontSize: "12px"
                                                }}>
                                                <CheckCircle size={12} />
                                                {payingId === payment.id ? "..." : "تسجيل دفع"}
                                            </button>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

// ── Create Contract Modal ──────────────────────────────────
function CreateContractModal({
    isOpen, onClose, onSave
}: {
    isOpen:  boolean;
    onClose: () => void;
    onSave:  (data: CreateContractRequest) => void;
}) {
    const [properties, setProperties] = useState<Property[]>([]);
    const [clients, setClients]       = useState<Client[]>([]);
    const [agents, setAgents]         = useState<{ id: string; fullName: string }[]>([]);

    const { register, handleSubmit, reset, formState: { errors } } =
        useForm({ resolver: zodResolver(schema) });

    
    const loadData = useCallback(async () => {
        try {
            const [propsData, clientsData, agentsData] = await Promise.all([
                propertiesService.getAll(),
                clientsService.getAll(),
                api.get("/users").then(r => r.data.data ?? [])
            ]);
            setProperties(propsData?.items ?? propsData ?? []);
            setClients(clientsData ?? []);
            setAgents(agentsData ?? []);
        } catch (err) {
            console.error(err);
        }
    }, []);
    useEffect(() => {
        if (isOpen) {
            loadData();
            reset({
                contractType: "Rent",
                paymentIntervalMonths: 1,
                commission: 0
            });
        }
    }, [isOpen, loadData, reset]);

    if (!isOpen) return null;

    return (
        <div style={{ position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "580px", maxHeight: "90vh", overflowY: "auto" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    إنشاء عقد جديد
                </h2>

                <form onSubmit={handleSubmit(onSave as never)}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px" }}>

                        <div style={{ gridColumn: "1 / -1" }}>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>العقار *</label>
                            <select {...register("propertyId")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="">اختر العقار</option>
                                {properties.map(p => (
                                    <option key={p.id} value={p.id}>{p.title} — {p.city}</option>
                                ))}
                            </select>
                            {errors.propertyId && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.propertyId.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>العميل *</label>
                            <select {...register("clientId")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="">اختر العميل</option>
                                {clients.map(c => (
                                    <option key={c.id} value={c.id}>{c.fullName}</option>
                                ))}
                            </select>
                            {errors.clientId && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.clientId.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>الوكيل *</label>
                            <select {...register("agentId")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="">اختر الوكيل</option>
                                {agents.map(a => (
                                    <option key={a.id} value={a.id}>{a.fullName}</option>
                                ))}
                            </select>
                            {errors.agentId && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.agentId.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>نوع العقد *</label>
                            <select {...register("contractType")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="Rent">إيجار</option>
                                <option value="Sale">بيع</option>
                            </select>
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>قيمة العقد (ريال) *</label>
                            <input {...register("amount")} type="number" placeholder="50000"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                            {errors.amount && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.amount.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>العمولة (ريال)</label>
                            <input {...register("commission")} type="number" placeholder="2500"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>تاريخ البداية *</label>
                            <input {...register("startDate")} type="date"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                            {errors.startDate && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.startDate.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>تاريخ النهاية *</label>
                            <input {...register("endDate")} type="date"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                            {errors.endDate && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.endDate.message as string}</p>}
                        </div>

                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>فترة الدفع</label>
                            <select {...register("paymentIntervalMonths")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="1">شهري</option>
                                <option value="3">ربع سنوي</option>
                                <option value="6">نصف سنوي</option>
                                <option value="12">سنوي</option>
                            </select>
                        </div>

                        <div style={{ gridColumn: "1 / -1" }}>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>ملاحظات</label>
                            <textarea {...register("notes")} rows={2} placeholder="ملاحظات..."
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box", resize: "vertical" }} />
                        </div>
                    </div>

                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "14px" }}>
                            إلغاء
                        </button>
                        <button type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                            إنشاء العقد
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Contracts() {
    const [contracts, setContracts]       = useState<Contract[]>([]);
    const [loading, setLoading]           = useState(true);
    const [search, setSearch]             = useState("");
    const [statusFilter, setStatusFilter] = useState("");
    const [isCreateOpen, setCreateOpen]   = useState(false);
    const [selectedContract, setSelected] = useState<Contract | null>(null);

    
    const loadContracts = useCallback(async () => {
        setLoading(true);
        try {
            const data = await contractsService.getAll();
            setContracts(data ?? []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }, []);
       // eslint-disable-next-line react-hooks/set-state-in-effect
    useEffect(() => { loadContracts(); }, [loadContracts]);

    async function handleCreate(form: CreateContractRequest) {
        try {
            await contractsService.create(form);
            toast.success("تم إنشاء العقد بنجاح ✅");
            setCreateOpen(false);
            await loadContracts();
        } catch {
            toast.error("حدث خطأ، يرجى المحاولة مرة ثانية ❌");
        }
    }

    const filtered = contracts.filter(c => {
        const matchSearch = !search ||
            c.propertyTitle.includes(search) ||
            c.clientName.includes(search);
        const matchStatus = !statusFilter || c.status === statusFilter;
        return matchSearch && matchStatus;
    });

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>العقود</h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>
                        {contracts.length} عقد مسجل
                    </p>
                </div>
                <button
                    onClick={() => setCreateOpen(true)}
                    style={{ display: "flex", alignItems: "center", gap: "8px", padding: "10px 20px", borderRadius: "10px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                    <Plus size={18} />
                    إنشاء عقد
                </button>
            </div>

            <div style={{ display: "flex", gap: "8px", marginBottom: "20px", flexWrap: "wrap" }}>
                <div onClick={() => setStatusFilter("")}
                    style={{ padding: "6px 16px", borderRadius: "8px", cursor: "pointer", fontSize: "13px", fontWeight: "500", backgroundColor: !statusFilter ? "#1e3a8a" : "#f8fafc", color: !statusFilter ? "white" : "#64748b", border: `1px solid ${!statusFilter ? "#1e3a8a" : "#e2e8f0"}` }}>
                    الكل ({contracts.length})
                </div>
                {Object.entries(statusLabel).map(([key, label]) => (
                    <div key={key} onClick={() => setStatusFilter(statusFilter === key ? "" : key)}
                        style={{ padding: "6px 16px", borderRadius: "8px", cursor: "pointer", fontSize: "13px", fontWeight: "500", backgroundColor: statusFilter === key ? statusColor[key] : "#f8fafc", color: statusFilter === key ? "white" : "#64748b", border: `1px solid ${statusFilter === key ? statusColor[key] : "#e2e8f0"}` }}>
                        {label} ({contracts.filter(c => c.status === key).length})
                    </div>
                ))}
            </div>

            <div style={{ position: "relative", marginBottom: "24px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث بالعقار أو العميل..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }} />
            </div>

            {loading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <FileText size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا توجد عقود</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(340px, 1fr))", gap: "20px" }}>
                    {filtered.map(contract => (
                        <ContractCard
                            key={contract.id}
                            contract={contract}
                            onView={setSelected}
                        />
                    ))}
                </div>
            )}

            <CreateContractModal
                isOpen={isCreateOpen}
                onClose={() => setCreateOpen(false)}
                onSave={handleCreate}
            />

            <ContractDetailModal
                contract={selectedContract}
                onClose={() => setSelected(null)}
            />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}