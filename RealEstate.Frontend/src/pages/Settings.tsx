// src/pages/Settings.tsx
import { useState} from "react";
import {
    Building2, Phone, MapPin, Crown,
    Calendar, Users, FileText, CheckCircle,
    AlertTriangle, Edit2, Save, X
} from "lucide-react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { companyService, type CompanyDetail }
    from "../services/company.service";

// ── Schema ────────────────────────────────────────────────
const schema = z.object({
    name:    z.string().min(1, "اسم الشركة مطلوب"),
    phone:   z.string().regex(/^05\d{8}$/, "رقم الجوال غير صحيح"),
    address: z.string().optional(),
    logo:    z.string().optional()
});

type CompanyForm = z.infer<typeof schema>;

// ── Plan Badge ────────────────────────────────────────────
const planColor: Record<string, string> = {
    Basic:    "#64748b",
    Pro:      "#2563eb",
    Business: "#7c3aed"
};

const planLabel: Record<string, string> = {
    Basic:    "الأساسية",
    Pro:      "الاحترافية",
    Business: "الأعمال"
};

// ── Stat Card ─────────────────────────────────────────────
function StatCard({ icon: Icon, label, value, color }: {
    icon:  React.ElementType;
    label: string;
    value: number;
    color: string;
}) {
    return (
        <div style={{
            backgroundColor: "white", borderRadius: "12px",
            border: "1px solid #f1f5f9", padding: "16px",
            display: "flex", alignItems: "center", gap: "12px"
        }}>
            <div style={{
                width: "40px", height: "40px", borderRadius: "10px",
                backgroundColor: `${color}15`,
                display: "flex", alignItems: "center", justifyContent: "center"
            }}>
                <Icon size={20} color={color} />
            </div>
            <div>
                <p style={{ fontSize: "20px", fontWeight: "700", color: "#1e293b", margin: 0 }}>{value}</p>
                <p style={{ fontSize: "12px", color: "#94a3b8", margin: 0 }}>{label}</p>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Settings() {
    const queryClient = useQueryClient();
    const [isEditing, setEditing] = useState(false);

   const { data: company, isLoading } = useQuery<CompanyDetail>({
    queryKey: ["company-me"],
    queryFn:  companyService.get,
    select: (data) => ({
        ...data,
        daysLeft: Math.ceil(
            (new Date(data.subscriptionExpiry).getTime() - new Date().getTime())
            / (1000 * 60 * 60 * 24)
        )
    })
});

    const { register, handleSubmit, reset, formState: { errors } } =
        useForm<CompanyForm>({
            resolver: zodResolver(schema),
            values: company ? {
                name:    company.name,
                phone:   company.phone,
                address: company.address ?? "",
                logo:    company.logo ?? ""
            } : undefined
        });

    const updateMutation = useMutation({
        mutationFn: (data: CompanyForm) => companyService.update({
            name:    data.name,
            phone:   data.phone,
            address: data.address || null,
            logo:    data.logo    || null
        }),
        onSuccess: () => {
            toast.success("تم تحديث بيانات الشركة ✅");
            queryClient.invalidateQueries({ queryKey: ["company-me"] });
            setEditing(false);
        },
        onError: () => toast.error("فشل التحديث ❌")
    });

    if (isLoading) return (
        <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
            <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
        </div>
    );

    if (!company) return null;

    return (
        <div style={{ padding: "24px", maxWidth: "900px" }} dir="rtl">

            {/* Header */}
            <div style={{ marginBottom: "24px" }}>
                <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: "0 0 4px" }}>
                    إعدادات الشركة
                </h1>
                <p style={{ color: "#64748b", fontSize: "14px", margin: 0 }}>
                    إدارة بيانات وإعدادات مكتبك العقاري
                </p>
            </div>

            {/* Stats */}
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(180px, 1fr))", gap: "12px", marginBottom: "24px" }}>
                <StatCard icon={Building2} label="العقارات"       value={company.totalProperties} color="#2563eb" />
                <StatCard icon={Users}     label="المستخدمين"     value={company.totalUsers}      color="#7c3aed" />
                <StatCard icon={Users}     label="العملاء"        value={company.totalClients}     color="#16a34a" />
                <StatCard icon={FileText}  label="العقود النشطة"  value={company.activeContracts}  color="#d97706" />
            </div>

            {/* Subscription Card */}
            <div style={{
                background: company.isExpired
                    ? "linear-gradient(135deg, #fef2f2, #fee2e2)"
                    : company.daysLeft <= 7
                    ? "linear-gradient(135deg, #fffbeb, #fef3c7)"
                    : "linear-gradient(135deg, #eff6ff, #dbeafe)",
                borderRadius: "16px",
                padding: "20px 24px",
                marginBottom: "24px",
                border: `1px solid ${company.isExpired ? "#fecaca" : company.daysLeft <= 7 ? "#fde68a" : "#bfdbfe"}`
            }}>
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                    <div>
                        <div style={{ display: "flex", alignItems: "center", gap: "10px", marginBottom: "8px" }}>
                            <Crown size={20} color={planColor[company.subscriptionPlan]} />
                            <span style={{
                                fontSize: "16px", fontWeight: "700",
                                color: planColor[company.subscriptionPlan]
                            }}>
                                باقة {planLabel[company.subscriptionPlan] || company.subscriptionPlan}
                            </span>
                        </div>
                        <div style={{ display: "flex", alignItems: "center", gap: "8px", fontSize: "13px", color: "#64748b" }}>
                            <Calendar size={14} />
                            تنتهي في: {new Date(company.subscriptionExpiry).toLocaleDateString("ar-SA")}
                        </div>
                    </div>

                    <div style={{ textAlign: "center" }}>
                        {company.isExpired ? (
                            <div style={{ display: "flex", alignItems: "center", gap: "6px", color: "#dc2626" }}>
                                <AlertTriangle size={18} />
                                <span style={{ fontWeight: "700", fontSize: "14px" }}>منتهي</span>
                            </div>
                        ) : (
                            <div>
                                <p style={{ fontSize: "28px", fontWeight: "700", color: company.daysLeft <= 7 ? "#d97706" : "#2563eb", margin: 0 }}>
                                    {company.daysLeft}
                                </p>
                                <p style={{ fontSize: "12px", color: "#64748b", margin: 0 }}>يوم متبقي</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>

            {/* Company Info Card */}
            <div style={{ backgroundColor: "white", borderRadius: "16px", border: "1px solid #f1f5f9", overflow: "hidden" }}>

                {/* Card Header */}
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "20px 24px", borderBottom: "1px solid #f1f5f9" }}>
                    <h2 style={{ fontSize: "16px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                        بيانات المكتب
                    </h2>
                    {!isEditing ? (
                        <button
                            onClick={() => setEditing(true)}
                            style={{ display: "flex", alignItems: "center", gap: "6px", padding: "8px 16px", borderRadius: "8px", border: "none", backgroundColor: "#eff6ff", color: "#2563eb", cursor: "pointer", fontSize: "13px", fontWeight: "500" }}>
                            <Edit2 size={14} />
                            تعديل
                        </button>
                    ) : (
                        <div style={{ display: "flex", gap: "8px" }}>
                            <button
                                onClick={() => { reset(); setEditing(false); }}
                                style={{ display: "flex", alignItems: "center", gap: "6px", padding: "8px 16px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "13px" }}>
                                <X size={14} />
                                إلغاء
                            </button>
                            <button
                                onClick={handleSubmit(d => updateMutation.mutate(d))}
                                disabled={updateMutation.isPending}
                                style={{ display: "flex", alignItems: "center", gap: "6px", padding: "8px 16px", borderRadius: "8px", border: "none", backgroundColor: "#16a34a", color: "white", cursor: "pointer", fontSize: "13px", fontWeight: "500" }}>
                                <Save size={14} />
                                {updateMutation.isPending ? "جاري الحفظ..." : "حفظ"}
                            </button>
                        </div>
                    )}
                </div>

                {/* Card Body */}
                <div style={{ padding: "24px" }}>
                    {!isEditing ? (
                        // View Mode
                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "20px" }}>
                            {[
                                { icon: Building2, label: "اسم المكتب", value: company.name },
                                { icon: Phone,     label: "رقم الجوال", value: company.phone },
                                { icon: MapPin,    label: "العنوان",    value: company.address || "غير محدد" },
                                {
                                    icon: CheckCircle,
                                    label: "الحالة",
                                    value: company.isActive ? "نشط" : "موقوف"
                                }
                            ].map(item => (
                                <div key={item.label}>
                                    <div style={{ display: "flex", alignItems: "center", gap: "8px", marginBottom: "6px" }}>
                                        <item.icon size={15} color="#94a3b8" />
                                        <span style={{ fontSize: "12px", color: "#94a3b8" }}>{item.label}</span>
                                    </div>
                                    <p style={{ fontSize: "15px", fontWeight: "500", color: "#1e293b", margin: 0, paddingRight: "23px" }}>
                                        {item.value}
                                    </p>
                                </div>
                            ))}
                        </div>
                    ) : (
                        // Edit Mode
                        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px" }}>

                            {/* Name */}
                            <div style={{ gridColumn: "1 / -1" }}>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                                    اسم المكتب *
                                </label>
                                <input {...register("name")}
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                                {errors.name && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.name.message}</p>}
                            </div>

                            {/* Phone */}
                            <div>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                                    رقم الجوال *
                                </label>
                                <input {...register("phone")} dir="ltr"
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                                {errors.phone && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.phone.message}</p>}
                            </div>

                            {/* Address */}
                            <div>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                                    العنوان
                                </label>
                                <input {...register("address")} placeholder="الرياض — حي النرجس"
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                            </div>

                            {/* Logo URL */}
                            <div style={{ gridColumn: "1 / -1" }}>
                                <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                                    رابط الشعار (URL)
                                </label>
                                <input {...register("logo")} placeholder="https://..."
                                    style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }} />
                            </div>
                        </div>
                    )}
                </div>
            </div>

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}