// src/pages/Appointments.tsx
import { useState, useEffect } from "react";
import {
    Calendar, Plus, Search, Clock,
    MapPin, Phone, User, Check,
    X, MessageSquare, Trash2
} from "lucide-react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { appointmentsService, type Appointment }
    from "../services/appointments.service";
import { propertiesService, type Property }
    from "../services/properties.service";
import { clientsService, type Client }
    from "../services/clients.service";
import api from "../services/api";

// ── Helpers ───────────────────────────────────────────────
const statusLabel: Record<string, string> = {
    Pending:   "قيد الانتظار",
    Confirmed: "مؤكد",
    Done:      "منتهي",
    Cancelled: "ملغي"
};

const statusColor: Record<string, string> = {
    Pending:   "#d97706",
    Confirmed: "#2563eb",
    Done:      "#16a34a",
    Cancelled: "#64748b"
};

// ── Schema ────────────────────────────────────────────────
const schema = z.object({
    propertyId:  z.string().min(1, "العقار مطلوب"),
    clientId:    z.string().min(1, "العميل مطلوب"),
    agentId:     z.string().min(1, "الوكيل مطلوب"),
    scheduledAt: z.string().min(1, "التاريخ والوقت مطلوبان"),
    notes:       z.string().optional()
});

// ── Appointment Card ──────────────────────────────────────
function AppointmentCard({
    appointment,
    onStatusChange,
    onDelete
}: {
    appointment:    Appointment;
    onStatusChange: (id: string, status: string, feedback?: string) => void;
    onDelete:       (id: string) => void;
}) {
    const [showFeedback, setShowFeedback] = useState(false);
    const [feedback, setFeedback]         = useState("");

    const date = new Date(appointment.scheduledAt);
    const isToday = new Date().toDateString() === date.toDateString();

    return (
        <div style={{
            background: "white",
            borderRadius: "16px",
            border: `1px solid ${isToday ? "#bfdbfe" : "#f1f5f9"}`,
            padding: "20px",
            boxShadow: isToday ? "0 0 0 2px #eff6ff" : "0 1px 3px rgba(0,0,0,0.06)"
        }}>
            {/* Today Badge */}
            {isToday && (
                <div style={{ marginBottom: "12px" }}>
                    <span style={{ backgroundColor: "#2563eb", color: "white", padding: "3px 10px", borderRadius: "20px", fontSize: "11px", fontWeight: "600" }}>
                        اليوم
                    </span>
                </div>
            )}

            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                <div>
                    <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", margin: "0 0 4px" }}>
                        {appointment.propertyTitle}
                    </h3>
                    <div style={{ display: "flex", alignItems: "center", gap: "4px", color: "#64748b", fontSize: "13px" }}>
                        <MapPin size={13} />
                        {appointment.propertyCity}
                    </div>
                </div>
                <span style={{
                    padding: "4px 12px", borderRadius: "20px", fontSize: "12px", fontWeight: "600",
                    backgroundColor: `${statusColor[appointment.status]}15`,
                    color: statusColor[appointment.status]
                }}>
                    {statusLabel[appointment.status]}
                </span>
            </div>

            {/* Info */}
            <div style={{ display: "flex", flexDirection: "column", gap: "8px", marginBottom: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "8px", fontSize: "13px", color: "#475569" }}>
                    <User size={14} color="#94a3b8" />
                    {appointment.clientName}
                </div>
                <div style={{ display: "flex", alignItems: "center", gap: "8px", fontSize: "13px", color: "#475569" }}>
                    <Phone size={14} color="#94a3b8" />
                    <span dir="ltr">{appointment.clientPhone}</span>
                </div>
                <div style={{ display: "flex", alignItems: "center", gap: "8px", fontSize: "13px", color: "#475569" }}>
                    <Clock size={14} color="#94a3b8" />
                    {date.toLocaleDateString("ar-SA")} — {date.toLocaleTimeString("ar-SA", { hour: "2-digit", minute: "2-digit" })}
                </div>
                <div style={{ display: "flex", alignItems: "center", gap: "8px", fontSize: "13px", color: "#475569" }}>
                    <User size={14} color="#94a3b8" />
                    الوكيل: {appointment.agentName}
                </div>
            </div>

            {/* Notes */}
            {appointment.notes && (
                <div style={{ backgroundColor: "#f8fafc", borderRadius: "8px", padding: "10px", marginBottom: "16px", fontSize: "13px", color: "#64748b" }}>
                    {appointment.notes}
                </div>
            )}

            {/* Feedback input */}
            {showFeedback && (
                <div style={{ marginBottom: "12px" }}>
                    <textarea
                        value={feedback}
                        onChange={e => setFeedback(e.target.value)}
                        placeholder="أضف تقييم للزيارة..."
                        rows={2}
                        style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "8px 12px", fontSize: "13px", boxSizing: "border-box", resize: "none" }}
                    />
                    <div style={{ display: "flex", gap: "8px", marginTop: "8px" }}>
                        <button
                            onClick={() => {
                                onStatusChange(appointment.id, "Done", feedback);
                                setShowFeedback(false);
                            }}
                            style={{ flex: 1, padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#16a34a", color: "white", cursor: "pointer", fontSize: "13px" }}>
                            حفظ
                        </button>
                        <button
                            onClick={() => setShowFeedback(false)}
                            style={{ padding: "8px 12px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", cursor: "pointer", fontSize: "13px" }}>
                            إلغاء
                        </button>
                    </div>
                </div>
            )}

            {/* Actions */}
            {appointment.status !== "Done" && appointment.status !== "Cancelled" && (
                <div style={{ display: "flex", gap: "8px", borderTop: "1px solid #f1f5f9", paddingTop: "12px" }}>
                    {appointment.status === "Pending" && (
                        <button
                            onClick={() => onStatusChange(appointment.id, "Confirmed")}
                            style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", gap: "6px", padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#eff6ff", color: "#2563eb", cursor: "pointer", fontSize: "13px" }}>
                            <Check size={14} />
                            تأكيد
                        </button>
                    )}
                    {appointment.status === "Confirmed" && (
                        <button
                            onClick={() => setShowFeedback(true)}
                            style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", gap: "6px", padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#f0fdf4", color: "#16a34a", cursor: "pointer", fontSize: "13px" }}>
                            <MessageSquare size={14} />
                            إنهاء الزيارة
                        </button>
                    )}
                    <button
                        onClick={() => onStatusChange(appointment.id, "Cancelled")}
                        style={{ display: "flex", alignItems: "center", justifyContent: "center", gap: "6px", padding: "8px 12px", borderRadius: "8px", border: "none", backgroundColor: "#fef2f2", color: "#dc2626", cursor: "pointer", fontSize: "13px" }}>
                        <X size={14} />
                        إلغاء
                    </button>
                    <button
                        onClick={() => onDelete(appointment.id)}
                        style={{ padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#f8fafc", color: "#94a3b8", cursor: "pointer" }}>
                        <Trash2 size={14} />
                    </button>
                </div>
            )}
        </div>
    );
}

// ── Create Modal ──────────────────────────────────────────
function CreateAppointmentModal({
    isOpen, onClose, onSave
}: {
    isOpen:  boolean;
    onClose: () => void;
    onSave:  (data: z.infer<typeof schema>) => void;
}) {
    const [properties, setProperties] = useState<Property[]>([]);
    const [clients, setClients]       = useState<Client[]>([]);
    const [agents, setAgents]         = useState<{ id: string; fullName: string }[]>([]);

    const { register, handleSubmit, reset, formState: { errors } } =
        useForm({ resolver: zodResolver(schema) });

    async function loadData() {
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
    }

    useEffect(() => {
        if (isOpen) {
            loadData();
            reset({});
        }
    }, [isOpen]);

    if (!isOpen) return null;

    return (
        <div style={{ position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "520px", maxHeight: "90vh", overflowY: "auto" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    إضافة موعد جديد
                </h2>

                <form onSubmit={handleSubmit(onSave as never)}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>

                        {/* Property */}
                        <div>
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

                        {/* Client */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>العميل *</label>
                            <select {...register("clientId")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="">اختر العميل</option>
                                {clients.map(c => (
                                    <option key={c.id} value={c.id}>{c.fullName} — {c.phone}</option>
                                ))}
                            </select>
                            {errors.clientId && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.clientId.message as string}</p>}
                        </div>

                        {/* Agent */}
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

                        {/* Date & Time */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>التاريخ والوقت *</label>
                            <input
                                {...register("scheduledAt")}
                                type="datetime-local"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.scheduledAt && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.scheduledAt.message as string}</p>}
                        </div>

                        {/* Notes */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>ملاحظات</label>
                            <textarea
                                {...register("notes")}
                                rows={3}
                                placeholder="ملاحظات الموعد..."
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box", resize: "vertical" }}
                            />
                        </div>
                    </div>

                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "14px" }}>
                            إلغاء
                        </button>
                        <button type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                            إضافة الموعد
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Appointments() {
    const [appointments, setAppointments] = useState<Appointment[]>([]);
    const [loading, setLoading]           = useState(true);
    const [search, setSearch]             = useState("");
    const [statusFilter, setStatus]       = useState("");
    const [todayOnly, setTodayOnly]       = useState(false);
    const [isModalOpen, setModal]         = useState(false);

    async function loadAppointments() {
        setLoading(true);
        try {
            const data = await appointmentsService.getAll(todayOnly);
            setAppointments(data ?? []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => { loadAppointments(); }, [todayOnly]);

    async function handleCreate(form: z.infer<typeof schema>) {
        try {
            const scheduledAt = new Date(form.scheduledAt).toISOString();
            await appointmentsService.create({ ...form, scheduledAt });
            toast.success("تم إضافة الموعد بنجاح ✅");
            setModal(false);
            await loadAppointments();
        } catch {
            toast.error("حدث خطأ ❌");
        }
    }

    async function handleStatusChange(id: string, status: string, feedback?: string) {
        try {
            await appointmentsService.updateStatus(id, status, feedback);
            toast.success(
                status === "Confirmed" ? "تم تأكيد الموعد ✅" :
                status === "Done"      ? "تم إنهاء الزيارة ✅" :
                "تم إلغاء الموعد 🗑️"
            );
            await loadAppointments();
        } catch {
            toast.error("حدث خطأ ❌");
        }
    }

    async function handleDelete(id: string) {
        if (!confirm("هل أنت متأكد من حذف هذا الموعد؟")) return;
        try {
            await appointmentsService.delete(id);
            toast.success("تم حذف الموعد 🗑️");
            await loadAppointments();
        } catch {
            toast.error("فشل الحذف ❌");
        }
    }

    const filtered = appointments.filter(a => {
        const matchSearch = !search ||
            a.propertyTitle.includes(search) ||
            a.clientName.includes(search);
        const matchStatus = !statusFilter || a.status === statusFilter;
        return matchSearch && matchStatus;
    });

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>المواعيد</h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>
                        {appointments.length} موعد مسجل
                    </p>
                </div>
                <button
                    onClick={() => setModal(true)}
                    style={{ display: "flex", alignItems: "center", gap: "8px", padding: "10px 20px", borderRadius: "10px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                    <Plus size={18} />
                    إضافة موعد
                </button>
            </div>

            {/* Today Toggle */}
            <div style={{ display: "flex", gap: "8px", marginBottom: "20px", alignItems: "center" }}>
                <button
                    onClick={() => setTodayOnly(!todayOnly)}
                    style={{
                        display: "flex", alignItems: "center", gap: "8px",
                        padding: "8px 16px", borderRadius: "8px", cursor: "pointer",
                        fontSize: "13px", fontWeight: "500",
                        backgroundColor: todayOnly ? "#2563eb" : "#f8fafc",
                        color: todayOnly ? "white" : "#64748b",
                        border: `1px solid ${todayOnly ? "#2563eb" : "#e2e8f0"}`
                    }}>
                    <Calendar size={14} />
                    مواعيد اليوم فقط
                </button>
            </div>

            {/* Status Filter */}
            <div style={{ display: "flex", gap: "8px", marginBottom: "20px", flexWrap: "wrap" }}>
                <div onClick={() => setStatus("")}
                    style={{ padding: "6px 16px", borderRadius: "8px", cursor: "pointer", fontSize: "13px", fontWeight: "500", backgroundColor: !statusFilter ? "#1e3a8a" : "#f8fafc", color: !statusFilter ? "white" : "#64748b", border: `1px solid ${!statusFilter ? "#1e3a8a" : "#e2e8f0"}` }}>
                    الكل ({appointments.length})
                </div>
                {Object.entries(statusLabel).map(([key, label]) => (
                    <div key={key} onClick={() => setStatus(statusFilter === key ? "" : key)}
                        style={{ padding: "6px 16px", borderRadius: "8px", cursor: "pointer", fontSize: "13px", fontWeight: "500", backgroundColor: statusFilter === key ? statusColor[key] : "#f8fafc", color: statusFilter === key ? "white" : "#64748b", border: `1px solid ${statusFilter === key ? statusColor[key] : "#e2e8f0"}` }}>
                        {label} ({appointments.filter(a => a.status === key).length})
                    </div>
                ))}
            </div>

            {/* Search */}
            <div style={{ position: "relative", marginBottom: "24px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث بالعقار أو العميل..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }} />
            </div>

            {/* Content */}
            {loading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <Calendar size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا توجد مواعيد</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(320px, 1fr))", gap: "20px" }}>
                    {filtered.map(appointment => (
                        <AppointmentCard
                            key={appointment.id}
                            appointment={appointment}
                            onStatusChange={handleStatusChange}
                            onDelete={handleDelete}
                        />
                    ))}
                </div>
            )}

            <CreateAppointmentModal
                isOpen={isModalOpen}
                onClose={() => setModal(false)}
                onSave={handleCreate}
            />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}