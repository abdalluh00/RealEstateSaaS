// src/pages/Properties.tsx
import { useState, useEffect } from "react";
import {
    Building2, Plus, Search, Filter,
    Edit, Trash2, MapPin, BedDouble,
    Bath, Square
} from "lucide-react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { propertiesService, type Property }
    from "../services/properties.service";

// ── Helpers ──────────────────────────────────────────────
const statusLabel: Record<string, string> = {
    Available: "متاح",
    Rented:    "مؤجر",
    Sold:      "مباع",
    Reserved:  "محجوز"
};

const statusColor: Record<string, string> = {
    Available: "#16a34a",
    Rented:    "#2563eb",
    Sold:      "#7c3aed",
    Reserved:  "#d97706"
};

const typeLabel: Record<string, string> = {
    Apartment: "شقة",
    Villa:     "فيلا",
    Office:    "مكتب",
    Land:      "أرض",
    Building:  "عمارة",
    Warehouse: "مستودع"
};

// ── Schema ───────────────────────────────────────────────
const schema = z.object({
    title:       z.string().min(1, "العنوان مطلوب"),
    type:        z.string().min(1, "النوع مطلوب"),
    price:       z.preprocess(v => Number(v), z.number().min(1, "السعر مطلوب")),
    area:        z.preprocess(v => Number(v), z.number().min(1, "المساحة مطلوبة")),
    bedrooms:    z.preprocess(v => v === "" || v == null ? undefined : Number(v), z.number().optional()),
    bathrooms:   z.preprocess(v => v === "" || v == null ? undefined : Number(v), z.number().optional()),
    city:        z.string().min(1, "المدينة مطلوبة"),
    district:    z.string().min(1, "الحي مطلوب"),
    description: z.string().optional()
});

//type PropertyForm = z.infer<typeof schema>;

// ── Property Card ─────────────────────────────────────────
function PropertyCard({
    property,
    onEdit,
    onDelete
}: {
    property: Property;
    onEdit:   (p: Property) => void;
    onDelete: (id: string) => void;
}) {
    return (
        <div style={{
            background: "white",
            borderRadius: "16px",
            border: "1px solid #f1f5f9",
            overflow: "hidden",
            boxShadow: "0 1px 3px rgba(0,0,0,0.06)",
            transition: "transform 0.2s, box-shadow 0.2s"
        }}>
            {/* Status Bar */}
            <div style={{
                height: "4px",
                backgroundColor: statusColor[property.status] || "#94a3b8"
            }} />

            <div style={{ padding: "20px" }}>
                {/* Header */}
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", marginBottom: "12px" }}>
                    <div style={{ flex: 1 }}>
                        <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", marginBottom: "4px" }}>
                            {property.title}
                        </h3>
                        <div style={{ display: "flex", alignItems: "center", gap: "4px", color: "#64748b", fontSize: "13px" }}>
                            <MapPin size={13} />
                            {property.city} — {property.district}
                        </div>
                    </div>
                    <span style={{
                        padding: "4px 10px",
                        borderRadius: "20px",
                        fontSize: "12px",
                        fontWeight: "500",
                        backgroundColor: `${statusColor[property.status]}15`,
                        color: statusColor[property.status]
                    }}>
                        {statusLabel[property.status] || property.status}
                    </span>
                </div>

                {/* Type Badge */}
                <div style={{ marginBottom: "16px" }}>
                    <span style={{
                        padding: "3px 10px",
                        borderRadius: "6px",
                        fontSize: "12px",
                        backgroundColor: "#f1f5f9",
                        color: "#475569"
                    }}>
                        {typeLabel[property.type] || property.type}
                    </span>
                </div>

                {/* Details */}
                <div style={{ display: "flex", gap: "16px", marginBottom: "16px", color: "#64748b", fontSize: "13px" }}>
                    {property.bedrooms && (
                        <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
                            <BedDouble size={14} />
                            {property.bedrooms} غرف
                        </div>
                    )}
                    {property.bathrooms && (
                        <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
                            <Bath size={14} />
                            {property.bathrooms} حمام
                        </div>
                    )}
                    <div style={{ display: "flex", alignItems: "center", gap: "4px" }}>
                        <Square size={14} />
                        {property.area} م²
                    </div>
                </div>

                {/* Price + Actions */}
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                    <div>
                        <div style={{ fontSize: "18px", fontWeight: "700", color: "#1e293b" }}>
                            {property.price.toLocaleString()}
                        </div>
                        <div style={{ fontSize: "12px", color: "#94a3b8" }}>ريال سعودي</div>
                    </div>
                    <div style={{ display: "flex", gap: "8px" }}>
                        <button
                            onClick={() => onEdit(property)}
                            style={{
                                padding: "8px",
                                borderRadius: "8px",
                                border: "none",
                                backgroundColor: "#eff6ff",
                                color: "#2563eb",
                                cursor: "pointer"
                            }}>
                            <Edit size={15} />
                        </button>
                        <button
                            onClick={() => onDelete(property.id)}
                            style={{
                                padding: "8px",
                                borderRadius: "8px",
                                border: "none",
                                backgroundColor: "#fef2f2",
                                color: "#dc2626",
                                cursor: "pointer"
                            }}>
                            <Trash2 size={15} />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

// ── Modal ─────────────────────────────────────────────────
function PropertyModal({
    isOpen,
    onClose,
    onSave,
    editData
}: {
    isOpen:   boolean;
    onClose:  () => void;
    onSave: (data: z.infer<typeof schema>) => void;
    editData: Property | null;
}) {
    const { register, handleSubmit, reset, formState: { errors } } =
        useForm({ resolver: zodResolver(schema) });

  useEffect(() => {
    if (editData) {
        reset({
            title:       editData.title,
            type:        editData.type,
            price:       editData.price,
            area:        editData.area,
            bedrooms:    editData.bedrooms  ?? undefined,
            bathrooms:   editData.bathrooms ?? undefined,
            city:        editData.city,
            district:    editData.district,
            description: undefined
        });
    } else {
        reset({
            title: "", type: "", price: 0,
            area: 0, city: "", district: ""
        });
    }
}, [editData, reset]);

    if (!isOpen) return null;

    return (
        <div style={{
            position: "fixed", inset: 0,
            backgroundColor: "rgba(0,0,0,0.5)",
            display: "flex", alignItems: "center",
            justifyContent: "center", zIndex: 1000
        }}>
            <div style={{
                backgroundColor: "white",
                borderRadius: "16px",
                padding: "32px",
                width: "560px",
                maxHeight: "90vh",
                overflowY: "auto"
            }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    {editData ? "تعديل العقار" : "إضافة عقار جديد"}
                </h2>

                <form onSubmit={handleSubmit(onSave)}>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "16px" }}>

                        {/* Title */}
                        <div style={{ gridColumn: "1 / -1" }}>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                                عنوان العقار *
                            </label>
                            <input
                                {...register("title")}
                                placeholder="شقة في حي النرجس"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.title && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.title.message}</p>}
                        </div>

                        {/* Type */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>النوع *</label>
                            <select
                                {...register("type")}
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}>
                                <option value="">اختر النوع</option>
                                <option value="Apartment">شقة</option>
                                <option value="Villa">فيلا</option>
                                <option value="Office">مكتب</option>
                                <option value="Land">أرض</option>
                                <option value="Building">عمارة</option>
                                <option value="Warehouse">مستودع</option>
                            </select>
                            {errors.type && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.type.message}</p>}
                        </div>

                        {/* Price */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>السعر (ريال) *</label>
                            <input
                                {...register("price")}
                                type="number"
                                placeholder="35000"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.price && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.price.message}</p>}
                        </div>

                        {/* Area */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>المساحة (م²) *</label>
                            <input
                                {...register("area")}
                                type="number"
                                placeholder="120"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.area && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.area.message}</p>}
                        </div>

                        {/* Bedrooms */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>غرف النوم</label>
                            <input
                                {...register("bedrooms")}
                                type="number"
                                placeholder="3"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                        </div>

                        {/* Bathrooms */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>الحمامات</label>
                            <input
                                {...register("bathrooms")}
                                type="number"
                                placeholder="2"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                        </div>

                        {/* City */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>المدينة *</label>
                            <input
                                {...register("city")}
                                placeholder="الرياض"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.city && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.city.message}</p>}
                        </div>

                        {/* District */}
                        <div>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>الحي *</label>
                            <input
                                {...register("district")}
                                placeholder="النرجس"
                                style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                            />
                            {errors.district && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{errors.district.message}</p>}
                        </div>

                        {/* Description */}
                        <div style={{ gridColumn: "1 / -1" }}>
                            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>الوصف</label>
                            <textarea
                                {...register("description")}
                                rows={3}
                                placeholder="وصف العقار..."
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
                            {editData ? "حفظ التعديلات" : "إضافة العقار"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Properties() {
    const [properties, setProperties] = useState<Property[]>([]);
    const [loading, setLoading]       = useState(true);
    const [search, setSearch]         = useState("");
    const [statusFilter, setStatus]   = useState("");
    const [isModalOpen, setModal]     = useState(false);
    const [editData, setEditData]     = useState<Property | null>(null);
   

    useEffect(() => { loadProperties(); }, []);

    async function loadProperties() {
        setLoading(true);
        try {
            const data = await propertiesService.getAll();
            setProperties(data?.items ?? data ?? []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

   async function handleSave(form: z.infer<typeof schema>) {
    try {
        const request = {
            title:       form.title,
            type:        form.type,
            price:       Number(form.price),
            area:        Number(form.area),
            bedrooms:    form.bedrooms  ? Number(form.bedrooms)  : undefined,
            bathrooms:   form.bathrooms ? Number(form.bathrooms) : undefined,
            city:        form.city,
            district:    form.district,
            description: form.description,
            status:      "Available",
            address:     null,
            isFeatured:  false
        };

        if (editData) {
            await propertiesService.update(editData.id, request);
            toast.success("تم تعديل العقار بنجاح ✅");
        } else {
            await propertiesService.create(request);
            toast.success("تم إضافة العقار بنجاح ✅");
        }


        setModal(false);
        setEditData(null);
        await loadProperties();
    } catch (err) {
        console.error(err);
    }
}

   async function handleDelete(id: string) {
    if (!confirm("هل أنت متأكد من حذف هذا العقار؟")) return;
    try {
        await propertiesService.delete(id);
        toast.success("تم حذف العقار بنجاح 🗑️");
        await loadProperties();
    } catch (err) {
         toast.error("فشل الحذف، يرجى المحاولة مرة ثانية ❌");
        console.error(err);
       
    }
}

    function handleEdit(property: Property) {
        setEditData(property);
        setModal(true);
    }

    // فلترة محلية
    const filtered = properties.filter(p => {
        const matchSearch = !search ||
            p.title.includes(search) ||
            p.city.includes(search) ||
            p.district.includes(search);
        const matchStatus = !statusFilter || p.status === statusFilter;
        return matchSearch && matchStatus;
    });

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                        العقارات
                    </h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>
                        {properties.length} عقار مسجل
                    </p>
                </div>
                <button
                    onClick={() => { setEditData(null); setModal(true); }}
                    style={{
                        display: "flex", alignItems: "center", gap: "8px",
                        padding: "10px 20px", borderRadius: "10px",
                        border: "none", backgroundColor: "#2563eb",
                        color: "white", cursor: "pointer",
                        fontSize: "14px", fontWeight: "500"
                    }}>
                    <Plus size={18} />
                    إضافة عقار
                </button>
            </div>

            {/* Filters */}
            <div style={{ display: "flex", gap: "12px", marginBottom: "24px" }}>
                <div style={{ position: "relative", flex: 1 }}>
                    <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                    <input
                        value={search}
                        onChange={e => setSearch(e.target.value)}
                        placeholder="ابحث عن عقار..."
                        style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }}
                    />
                </div>
                <div style={{ position: "relative" }}>
                    <Filter size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                    <select
                        value={statusFilter}
                        onChange={e => setStatus(e.target.value)}
                        style={{ padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", backgroundColor: "white", cursor: "pointer" }}>
                        <option value="">كل الحالات</option>
                        <option value="Available">متاح</option>
                        <option value="Rented">مؤجر</option>
                        <option value="Sold">مباع</option>
                        <option value="Reserved">محجوز</option>
                    </select>
                </div>
            </div>

            {/* Stats */}
            <div style={{ display: "flex", gap: "12px", marginBottom: "24px" }}>
                {Object.entries(statusLabel).map(([key, label]) => (
                    <div
                        key={key}
                        onClick={() => setStatus(statusFilter === key ? "" : key)}
                        style={{
                            padding: "8px 16px", borderRadius: "8px",
                            cursor: "pointer", fontSize: "13px", fontWeight: "500",
                            backgroundColor: statusFilter === key ? statusColor[key] : "#f8fafc",
                            color: statusFilter === key ? "white" : "#64748b",
                            border: `1px solid ${statusFilter === key ? statusColor[key] : "#e2e8f0"}`
                        }}>
                        {label} ({properties.filter(p => p.status === key).length})
                    </div>
                ))}
            </div>

            {/* Content */}
            {loading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <Building2 size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا توجد عقارات</p>
                    <p style={{ fontSize: "14px" }}>ابدأ بإضافة عقار جديد</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(300px, 1fr))", gap: "20px" }}>
                    {filtered.map(property => (
                        <PropertyCard
                            key={property.id}
                            property={property}
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                        />
                    ))}
                </div>
            )}

            {/* Modal */}
            <PropertyModal
                isOpen={isModalOpen}
                onClose={() => { setModal(false); setEditData(null); }}
                onSave={handleSave}
                editData={editData}
            />

            {/* Loading Spinner CSS */}
            <style>{`
                @keyframes spin {
                    to { transform: rotate(360deg); }
                }
            `}</style>
        </div>
    );
}