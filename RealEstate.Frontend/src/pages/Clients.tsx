// src/pages/Clients.tsx
import { useState, useEffect } from "react";
import { Users, Plus, Search, Phone, Mail, Edit, Trash2 } from "lucide-react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { clientsService, type Client } from "../services/clients.service";

// ── Helpers ──────────────────────────────────────────────
const leadStatusLabel: Record<string, string> = {
    New:    "جديد",
    Hot:    "ساخن",
    Warm:   "دافئ",
    Cold:   "بارد",
    Closed: "مغلق"
};

const leadStatusColor: Record<string, string> = {
    New:    "#2563eb",
    Hot:    "#dc2626",
    Warm:   "#d97706",
    Cold:   "#64748b",
    Closed: "#16a34a"
};

const sourceLabel: Record<string, string> = {
    WhatsApp: "واتساب",
    Website:  "الموقع",
    Referral: "توصية",
    "Walk-in": "زيارة مباشرة",
    Other:    "أخرى"
};

// ── Schema ────────────────────────────────────────────────
const schema = z.object({
    fullName:   z.string().min(1, "الاسم مطلوب"),
    phone:      z.string().min(10, "رقم الجوال غير صحيح"),
    email:      z.string().email("البريد غير صحيح").optional().or(z.literal("")),
    leadStatus: z.string().min(1, "الحالة مطلوبة"),
    source:     z.string().optional(),
    notes:      z.string().optional()
});

type ClientForm = z.infer<typeof schema>;

// ── Client Card ───────────────────────────────────────────
function ClientCard({
    client,
    onEdit,
    onDelete
}: {
    client:   Client;
    onEdit:   (c: Client) => void;
    onDelete: (id: string) => void;
}) {
    return (
        <div style={{
            background: "white",
            borderRadius: "16px",
            border: "1px solid #f1f5f9",
            padding: "20px",
            boxShadow: "0 1px 3px rgba(0,0,0,0.06)"
        }}>
            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                    <div style={{
                        width: "44px", height: "44px",
                        borderRadius: "50%",
                        backgroundColor: `${leadStatusColor[client.leadStatus]}15`,
                        display: "flex", alignItems: "center", justifyContent: "center",
                        fontSize: "18px", fontWeight: "700",
                        color: leadStatusColor[client.leadStatus]
                    }}>
                        {client.fullName[0]}
                    </div>
                    <div>
                        <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", margin: 0 }}>
                            {client.fullName}
                        </h3>
                        {client.source && (
                            <span style={{ fontSize: "12px", color: "#94a3b8" }}>
                                {sourceLabel[client.source] || client.source}
                            </span>
                        )}
                    </div>
                </div>
                <span style={{
                    padding: "4px 12px",
                    borderRadius: "20px",
                    fontSize: "12px",
                    fontWeight: "600",
                    backgroundColor: `${leadStatusColor[client.leadStatus]}15`,
                    color: leadStatusColor[client.leadStatus]
                }}>
                    {leadStatusLabel[client.leadStatus] || client.leadStatus}
                </span>
            </div>

            {/* Contact */}
            <div style={{ display: "flex", flexDirection: "column", gap: "8px", marginBottom: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "8px", color: "#475569", fontSize: "13px" }}>
                    <Phone size={14} color="#94a3b8" />
                    <span dir="ltr">{client.phone}</span>
                </div>
                {client.email && (
                    <div style={{ display: "flex", alignItems: "center", gap: "8px", color: "#475569", fontSize: "13px" }}>
                        <Mail size={14} color="#94a3b8" />
                        {client.email}
                    </div>
                )}
            </div>

            {/* Actions */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", borderTop: "1px solid #f1f5f9", paddingTop: "12px" }}>
                <span style={{ fontSize: "12px", color: "#94a3b8" }}>
                    {new Date(client.createdAt).toLocaleDateString("ar-SA")}
                </span>
                <div style={{ display: "flex", gap: "8px" }}>
                    <button
                        onClick={() => onEdit(client)}
                        style={{ padding: "7px", borderRadius: "8px", border: "none", backgroundColor: "#eff6ff", color: "#2563eb", cursor: "pointer" }}>
                        <Edit size={14} />
                    </button>
                    <button
                        onClick={() => onDelete(client.id)}
                        style={{ padding: "7px", borderRadius: "8px", border: "none", backgroundColor: "#fef2f2", color: "#dc2626", cursor: "pointer" }}>
                        <Trash2 size={14} />
                    </button>
                </div>
            </div>
        </div>
    );
}

// ── Modal ─────────────────────────────────────────────────
function ClientModal({
    isOpen, onClose, onSave, editData
}: {
    isOpen:   boolean;
    onClose:  () => void;
    onSave:   (data: ClientForm) => void;
    editData: Client | null;
}) {
    const { register, handleSubmit, reset, formState: { errors } } =
        useForm({ resolver: zodResolver(schema) });

    useEffect(() => {
        if (editData) {
            reset({
                fullName:   editData.fullName,
                phone:      editData.phone,
                email:      editData.email ?? "",
                leadStatus: editData.leadStatus,
                source:     editData.source ?? "",
            });
        } else {
            reset({ fullName: "", phone: "", email: "", leadStatus: "New", source: "" });
        }
    }, [editData, reset]);

    if (!isOpen) return null;

    return (
        <div style={{ position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "480px", maxHeight: "90vh", overflowY: "auto" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    {editData ? "تعديل بيانات العميل" : "إضافة عميل جديد"}
                </h2>

                <form onSubmit={handleSubmit(onSave as never)}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>

                        {/* FullName */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>الاسم الكامل *</label>
                            <input
                                {...register("fullName")}
                                placeholder="محمد العمري"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.fullName && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.fullName.message as string}</p>}
                        </div>

                        {/* Phone */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>رقم الجوال *</label>
                            <input
                                {...register("phone")}
                                placeholder="05xxxxxxxx"
                                dir="ltr"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.phone && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.phone.message as string}</p>}
                        </div>

                        {/* Email */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>البريد الإلكتروني</label>
                            <input
                                {...register("email")}
                                type="email"
                                placeholder="example@email.com"
                                dir="ltr"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.email && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.email.message as string}</p>}
                        </div>

                        {/* Lead Status + Source */}
                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px" }}>
                            <div>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>حالة العميل *</label>
                                <select
                                    {...register("leadStatus")}
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                    <option value="New">جديد</option>
                                    <option value="Hot">ساخن</option>
                                    <option value="Warm">دافئ</option>
                                    <option value="Cold">بارد</option>
                                    <option value="Closed">مغلق</option>
                                </select>
                            </div>
                            <div>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>المصدر</label>
                                <select
                                    {...register("source")}
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                    <option value="">اختر</option>
                                    <option value="WhatsApp">واتساب</option>
                                    <option value="Website">الموقع</option>
                                    <option value="Referral">توصية</option>
                                    <option value="Walk-in">زيارة مباشرة</option>
                                    <option value="Other">أخرى</option>
                                </select>
                            </div>
                        </div>

                        {/* Notes */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>ملاحظات</label>
                            <textarea
                                {...register("notes")}
                                rows={3}
                                placeholder="أي ملاحظات عن العميل..."
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box", resize: "vertical" }}
                            />
                        </div>
                    </div>

                    {/* Buttons */}
                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button
                            type="button"
                            onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "14px" }}>
                            إلغاء
                        </button>
                        <button
                            type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                            {editData ? "حفظ التعديلات" : "إضافة العميل"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Clients() {
    const [clients, setClients]       = useState<Client[]>([]);
    const [loading, setLoading]       = useState(true);
    const [search, setSearch]         = useState("");
    const [statusFilter, setStatus]   = useState("");
    const [isModalOpen, setModal]     = useState(false);
    const [editData, setEditData]     = useState<Client | null>(null);

    useEffect(() => { loadClients(); }, []);

    async function loadClients() {
        setLoading(true);
        try {
            const data = await clientsService.getAll();
            setClients(data ?? []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

    async function handleSave(form: ClientForm) {
        try {
            const request = {
                fullName:   form.fullName,
                phone:      form.phone,
                email:      form.email || null,
                leadStatus: form.leadStatus,
                source:     form.source || null,
                notes:      form.notes || null,
            };

            if (editData) {
                await clientsService.update(editData.id, request as never);
                toast.success("تم تعديل بيانات العميل بنجاح ✅");
            } else {
                await clientsService.create(request as never);
                toast.success("تم إضافة العميل بنجاح ✅");
            }

            setModal(false);
            setEditData(null);
            await loadClients();
        } catch {
            toast.error("حدث خطأ، يرجى المحاولة مرة ثانية ❌");
        }
    }

    async function handleDelete(id: string) {
        if (!confirm("هل أنت متأكد من حذف هذا العميل؟")) return;
        try {
            await clientsService.delete(id);
            toast.success("تم حذف العميل بنجاح 🗑️");
            await loadClients();
        } catch {
            toast.error("فشل الحذف ❌");
        }
    }

    function handleEdit(client: Client) {
        setEditData(client);
        setModal(true);
    }

    // فلترة محلية
    const filtered = clients.filter(c => {
        const matchSearch = !search ||
            c.fullName.includes(search) ||
            c.phone.includes(search);
        const matchStatus = !statusFilter || c.leadStatus === statusFilter;
        return matchSearch && matchStatus;
    });

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>العملاء</h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>
                        {clients.length} عميل مسجل
                    </p>
                </div>
                <button
                    onClick={() => { setEditData(null); setModal(true); }}
                    style={{ display: "flex", alignItems: "center", gap: "8px", padding: "10px 20px", borderRadius: "10px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                    <Plus size={18} />
                    إضافة عميل
                </button>
            </div>

            {/* Lead Status Filter */}
            <div style={{ display: "flex", gap: "8px", marginBottom: "20px", flexWrap: "wrap" }}>
                <div
                    onClick={() => setStatus("")}
                    style={{
                        padding: "6px 16px", borderRadius: "8px", cursor: "pointer",
                        fontSize: "13px", fontWeight: "500",
                        backgroundColor: !statusFilter ? "#1e3a8a" : "#f8fafc",
                        color: !statusFilter ? "white" : "#64748b",
                        border: `1px solid ${!statusFilter ? "#1e3a8a" : "#e2e8f0"}`
                    }}>
                    الكل ({clients.length})
                </div>
                {Object.entries(leadStatusLabel).map(([key, label]) => (
                    <div
                        key={key}
                        onClick={() => setStatus(statusFilter === key ? "" : key)}
                        style={{
                            padding: "6px 16px", borderRadius: "8px", cursor: "pointer",
                            fontSize: "13px", fontWeight: "500",
                            backgroundColor: statusFilter === key ? leadStatusColor[key] : "#f8fafc",
                            color: statusFilter === key ? "white" : "#64748b",
                            border: `1px solid ${statusFilter === key ? leadStatusColor[key] : "#e2e8f0"}`
                        }}>
                        {label} ({clients.filter(c => c.leadStatus === key).length})
                    </div>
                ))}
            </div>

            {/* Search */}
            <div style={{ position: "relative", marginBottom: "24px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث باسم العميل أو رقم الجوال..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }}
                />
            </div>

            {/* Content */}
            {loading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <Users size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا يوجد عملاء</p>
                    <p style={{ fontSize: "14px" }}>ابدأ بإضافة عميل جديد</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(300px, 1fr))", gap: "20px" }}>
                    {filtered.map(client => (
                        <ClientCard
                            key={client.id}
                            client={client}
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                        />
                    ))}
                </div>
            )}

            <ClientModal
                isOpen={isModalOpen}
                onClose={() => { setModal(false); setEditData(null); }}
                onSave={handleSave}
                editData={editData}
            />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}