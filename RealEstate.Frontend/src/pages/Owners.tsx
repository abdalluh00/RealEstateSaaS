// src/pages/Owners.tsx
import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import toast from "react-hot-toast";
import { Users, Plus, Search, Eye, Edit2, Trash2, Phone, Mail, IdCard } from "lucide-react";
import {
    ownersService,
    type Owner,
    type OwnerDetail,
    type CreateOwnerRequest
} from "../services/owners.service";

// ── Schema ────────────────────────────────────────────────
const schema = z.object({
    fullName: z.string().min(1, "اسم المالك مطلوب").max(200),
    phone:    z.string().regex(/^05\d{8}$/, "رقم الجوال غير صحيح (يبدأ بـ 05)"),
    email:    z.string().email("البريد الإلكتروني غير صحيح").optional().or(z.literal("")),
    idNumber: z.string().optional(),
    notes:    z.string().optional(),
});

type FormValues = z.infer<typeof schema>;

// ── Owner Card ────────────────────────────────────────────
function OwnerCard({ owner, onView, onEdit, onDelete }: {
    owner: Owner;
    onView: (o: Owner) => void;
    onEdit: (o: Owner) => void;
    onDelete: (o: Owner) => void;
}) {
    return (
        <div style={{ background: "white", borderRadius: "16px", border: "1px solid #f1f5f9", padding: "20px", boxShadow: "0 1px 3px rgba(0,0,0,0.06)" }}>
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                    <div style={{ width: "44px", height: "44px", borderRadius: "50%", backgroundColor: "#eff6ff", color: "#2563eb", display: "flex", alignItems: "center", justifyContent: "center", fontWeight: "700", fontSize: "16px" }}>
                        {owner.fullName.charAt(0)}
                    </div>
                    <div>
                        <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", margin: "0 0 2px" }}>{owner.fullName}</h3>
                        <p style={{ fontSize: "12px", color: "#94a3b8", margin: 0 }}>
                            {new Date(owner.createdAt).toLocaleDateString("ar-SA")}
                        </p>
                    </div>
                </div>
            </div>

            <div style={{ display: "flex", flexDirection: "column", gap: "8px", marginBottom: "16px", fontSize: "13px", color: "#475569" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                    <Phone size={14} color="#94a3b8" /> {owner.phone}
                </div>
                {owner.email && (
                    <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                        <Mail size={14} color="#94a3b8" /> {owner.email}
                    </div>
                )}
                {owner.idNumber && (
                    <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                        <IdCard size={14} color="#94a3b8" /> {owner.idNumber}
                    </div>
                )}
            </div>

            <div style={{ display: "flex", justifyContent: "space-between", paddingTop: "12px", borderTop: "1px solid #f1f5f9" }}>
                <div style={{ display: "flex", gap: "16px", fontSize: "12px" }}>
                    <div><span style={{ color: "#94a3b8" }}>العقارات: </span><strong style={{ color: "#1e293b" }}>{owner.totalProperties}</strong></div>
                    <div><span style={{ color: "#94a3b8" }}>العقود النشطة: </span><strong style={{ color: "#16a34a" }}>{owner.activeContracts}</strong></div>
                </div>
                <div style={{ display: "flex", gap: "6px" }}>
                    <button onClick={() => onView(owner)} title="عرض" style={iconBtn("#eff6ff", "#2563eb")}><Eye size={14} /></button>
                    <button onClick={() => onEdit(owner)} title="تعديل" style={iconBtn("#fef3c7", "#d97706")}><Edit2 size={14} /></button>
                    <button onClick={() => onDelete(owner)} title="حذف" style={iconBtn("#fef2f2", "#dc2626")}><Trash2 size={14} /></button>
                </div>
            </div>
        </div>
    );
}

const iconBtn = (bg: string, color: string): React.CSSProperties => ({
    display: "flex", alignItems: "center", justifyContent: "center",
    width: "30px", height: "30px", borderRadius: "8px",
    border: "none", backgroundColor: bg, color, cursor: "pointer"
});

// ── Detail Modal ──────────────────────────────────────────
function OwnerDetailModal({ ownerId, onClose }: { ownerId: string | null; onClose: () => void; }) {
    const { data, isLoading } = useQuery<OwnerDetail>({
        queryKey: ["owner-detail", ownerId],
        enabled: !!ownerId,
        queryFn: () => ownersService.getById(ownerId!),
    });

    if (!ownerId) return null;

    return (
        <div style={modalOverlay}>
            <div style={{ ...modalBox, width: "640px" }}>
                <div style={modalHeader}>
                    <h2 style={{ fontSize: "18px", fontWeight: "700", margin: 0 }}>تفاصيل المالك</h2>
                    <button onClick={onClose} style={closeBtn}>✕</button>
                </div>

                <div style={{ padding: "20px 24px 24px" }}>
                    {isLoading ? (
                        <div style={{ textAlign: "center", padding: "40px", color: "#94a3b8" }}>جاري التحميل...</div>
                    ) : data ? (
                        <>
                            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px", marginBottom: "24px" }}>
                                {[
                                    { label: "الاسم", value: data.fullName },
                                    { label: "الجوال", value: data.phone },
                                    { label: "البريد", value: data.email || "-" },
                                    { label: "رقم الهوية", value: data.idNumber || "-" },
                                ].map(i => (
                                    <div key={i.label} style={{ backgroundColor: "#f8fafc", borderRadius: "10px", padding: "12px" }}>
                                        <p style={{ fontSize: "12px", color: "#94a3b8", margin: "0 0 4px" }}>{i.label}</p>
                                        <p style={{ fontSize: "14px", fontWeight: "600", color: "#1e293b", margin: 0 }}>{i.value}</p>
                                    </div>
                                ))}
                            </div>

                            {data.notes && (
                                <div style={{ backgroundColor: "#fffbeb", border: "1px solid #fde68a", borderRadius: "10px", padding: "12px", marginBottom: "20px" }}>
                                    <p style={{ fontSize: "12px", color: "#92400e", margin: "0 0 4px", fontWeight: "600" }}>ملاحظات</p>
                                    <p style={{ fontSize: "13px", color: "#78350f", margin: 0 }}>{data.notes}</p>
                                </div>
                            )}

                            <h3 style={{ fontSize: "15px", fontWeight: "700", color: "#1e293b", marginBottom: "12px" }}>
                                العقارات ({data.properties.length})
                            </h3>
                            {data.properties.length === 0 ? (
                                <p style={{ color: "#94a3b8", fontSize: "13px", textAlign: "center", padding: "20px" }}>لا توجد عقارات</p>
                            ) : (
                                <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                                    {data.properties.map(p => (
                                        <div key={p.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "10px 14px", backgroundColor: "#f8fafc", borderRadius: "10px", border: "1px solid #e2e8f0" }}>
                                            <div>
                                                <p style={{ fontSize: "14px", fontWeight: "600", color: "#1e293b", margin: 0 }}>{p.title}</p>
                                                <p style={{ fontSize: "12px", color: "#94a3b8", margin: "2px 0 0" }}>{p.city} • {p.type}</p>
                                            </div>
                                            <div style={{ textAlign: "left" }}>
                                                <p style={{ fontSize: "14px", fontWeight: "700", color: "#1e293b", margin: 0 }}>{p.price.toLocaleString()} ﷼</p>
                                                <p style={{ fontSize: "12px", color: "#2563eb", margin: "2px 0 0" }}>{p.status}</p>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </>
                    ) : null}
                </div>
            </div>
        </div>
    );
}

// ── Create / Edit Modal ───────────────────────────────────
function OwnerFormModal({ isOpen, editingOwner, onClose, onSubmit }: {
    isOpen: boolean;
    editingOwner: Owner | null;
    onClose: () => void;
    onSubmit: (data: FormValues, id?: string) => void;
}) {
    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>({
        resolver: zodResolver(schema),
        values: editingOwner ? {
            fullName: editingOwner.fullName,
            phone:    editingOwner.phone,
            email:    editingOwner.email ?? "",
            idNumber: editingOwner.idNumber ?? "",
            notes:    "",
        } : { fullName: "", phone: "", email: "", idNumber: "", notes: "" },
        resetOptions: { keepDefaultValues: false }
    });

    if (!isOpen) return null;

    return (
        <div style={modalOverlay}>
            <div style={{ ...modalBox, width: "520px" }}>
                <div style={modalHeader}>
                    <h2 style={{ fontSize: "18px", fontWeight: "700", margin: 0 }}>
                        {editingOwner ? "تعديل المالك" : "إضافة مالك جديد"}
                    </h2>
                    <button onClick={() => { reset(); onClose(); }} style={closeBtn}>✕</button>
                </div>

                <form onSubmit={handleSubmit(d => onSubmit(d, editingOwner?.id))} style={{ padding: "20px 24px 24px" }}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "14px" }}>
                        <Field label="الاسم *" error={errors.fullName?.message} fullWidth>
                            <input {...register("fullName")} placeholder="محمد أحمد" style={inputStyle} />
                        </Field>
                        <Field label="رقم الجوال *" error={errors.phone?.message}>
                            <input {...register("phone")} placeholder="05XXXXXXXX" style={inputStyle} />
                        </Field>
                        <Field label="البريد الإلكتروني" error={errors.email?.message}>
                            <input {...register("email")} placeholder="email@example.com" style={inputStyle} />
                        </Field>
                        <Field label="رقم الهوية" error={errors.idNumber?.message} fullWidth>
                            <input {...register("idNumber")} placeholder="10XXXXXXXX" style={inputStyle} />
                        </Field>
                        <Field label="ملاحظات" error={errors.notes?.message} fullWidth>
                            <textarea {...register("notes")} rows={2} style={{ ...inputStyle, resize: "vertical" }} />
                        </Field>
                    </div>

                    <div style={{ display: "flex", gap: "10px", marginTop: "20px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={() => { reset(); onClose(); }} style={btnSecondary}>إلغاء</button>
                        <button type="submit" style={btnPrimary}>{editingOwner ? "حفظ التعديلات" : "إضافة"}</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

function Field({ label, error, fullWidth, children }: { label: string; error?: string; fullWidth?: boolean; children: React.ReactNode; }) {
    return (
        <div style={{ gridColumn: fullWidth ? "1 / -1" : "auto" }}>
            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>{label}</label>
            {children}
            {error && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{error}</p>}
        </div>
    );
}

// ── Shared styles ─────────────────────────────────────────
const modalOverlay: React.CSSProperties = { position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 };
const modalBox: React.CSSProperties = { backgroundColor: "white", borderRadius: "16px", maxHeight: "90vh", overflowY: "auto" };
const modalHeader: React.CSSProperties = { display: "flex", justifyContent: "space-between", alignItems: "center", padding: "20px 24px", borderBottom: "1px solid #f1f5f9" };
const closeBtn: React.CSSProperties = { background: "none", border: "none", cursor: "pointer", fontSize: "20px", color: "#94a3b8" };
const inputStyle: React.CSSProperties = { width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" };
const btnSecondary: React.CSSProperties = { padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "14px" };
const btnPrimary: React.CSSProperties = { padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" };

// ── Main Page ─────────────────────────────────────────────
export default function Owners() {
    const queryClient = useQueryClient();
    const [search, setSearch]       = useState("");
    const [formOpen, setFormOpen]   = useState(false);
    const [editing, setEditing]     = useState<Owner | null>(null);
    const [detailId, setDetailId]   = useState<string | null>(null);

    const { data: owners = [], isLoading } = useQuery<Owner[]>({
        queryKey: ["owners"],
        queryFn: () => ownersService.getAll(),
    });

    const invalidateOwners = () => queryClient.invalidateQueries({ queryKey: ["owners"] });

    const createMutation = useMutation({
        mutationFn: (payload: Omit<CreateOwnerRequest, "companyId">) => ownersService.create(payload),
        onSuccess: () => { toast.success("تم إضافة المالك ✅"); invalidateOwners(); setFormOpen(false); },
        onError: (e: { response?: { data?: { message?: string } } }) =>
            toast.error(e?.response?.data?.message ?? "فشل إضافة المالك ❌"),
    });

    const updateMutation = useMutation({
        mutationFn: (payload: { id: string } & Omit<CreateOwnerRequest, "companyId">) =>
            ownersService.update({ ...payload, companyId: "" }), // companyId not needed on update
        onSuccess: () => { toast.success("تم حفظ التعديلات ✅"); invalidateOwners(); setFormOpen(false); setEditing(null); },
        onError: () => toast.error("فشل التعديل ❌"),
    });

    const deleteMutation = useMutation({
        mutationFn: (id: string) => ownersService.remove(id),
        onSuccess: () => { toast.success("تم حذف المالك ✅"); invalidateOwners(); },
        onError: () => toast.error("فشل الحذف ❌"),
    });

    function handleSubmit(form: FormValues, id?: string) {
        const payload = {
            fullName: form.fullName,
            phone:    form.phone,
            email:    form.email || undefined,
            idNumber: form.idNumber || undefined,
            notes:    form.notes || undefined,
        };
        if (id) updateMutation.mutate({ id, ...payload });
        else    createMutation.mutate(payload);
    }

    function handleDelete(owner: Owner) {
        if (window.confirm(`هل أنت متأكد من حذف "${owner.fullName}"؟`)) {
            deleteMutation.mutate(owner.id);
        }
    }

    const filtered = owners.filter(o =>
        !search ||
        o.fullName.includes(search) ||
        o.phone.includes(search) ||
        (o.idNumber ?? "").includes(search)
    );

    return (
        <div style={{ padding: "24px" }} dir="rtl">
            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>الملاك</h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>{owners.length} مالك مسجل</p>
                </div>
                <button onClick={() => { setEditing(null); setFormOpen(true); }}
                    style={{ display: "flex", alignItems: "center", gap: "8px", padding: "10px 20px", borderRadius: "10px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                    <Plus size={18} /> إضافة مالك
                </button>
            </div>

            {/* Search */}
            <div style={{ position: "relative", marginBottom: "24px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث بالاسم أو الجوال أو الهوية..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }} />
            </div>

            {/* Content */}
            {isLoading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <Users size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا يوجد ملاك</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(340px, 1fr))", gap: "20px" }}>
                    {filtered.map(o => (
                        <OwnerCard key={o.id} owner={o}
                            onView={x => setDetailId(x.id)}
                            onEdit={x => { setEditing(x); setFormOpen(true); }}
                            onDelete={handleDelete} />
                    ))}
                </div>
            )}

            <OwnerFormModal isOpen={formOpen} editingOwner={editing}
                onClose={() => { setFormOpen(false); setEditing(null); }}
                onSubmit={handleSubmit} />

            <OwnerDetailModal ownerId={detailId} onClose={() => setDetailId(null)} />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}