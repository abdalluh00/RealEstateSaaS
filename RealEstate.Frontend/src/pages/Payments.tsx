// src/pages/Payments.tsx
import { useState, useEffect } from "react";
import {
    CreditCard, Search, CheckCircle,
    AlertTriangle, Clock, TrendingUp,
    XCircle, DollarSign
} from "lucide-react";
import toast from "react-hot-toast";
import { paymentsService, type Payment, type PaymentSummary }
    from "../services/payments.service";

// ── Helpers ───────────────────────────────────────────────
const methodLabel: Record<string, string> = {
    Cash:   "نقداً",
    Bank:   "تحويل بنكي",
    Online: "دفع إلكتروني",
    Mada:   "مدى",
    STC:    "STC Pay"
};

const statusColor: Record<string, string> = {
    Paid:      "#16a34a",
    Pending:   "#d97706",
    Late:      "#dc2626",
    Cancelled: "#64748b"
};

const statusLabel: Record<string, string> = {
    Paid:      "مدفوع",
    Pending:   "معلق",
    Late:      "متأخر",
    Cancelled: "ملغي"
};

// ── Summary Card ──────────────────────────────────────────
function SummaryCard({
    title, value, icon: Icon, color, subtitle
}: {
    title:    string;
    value:    string;
    icon:     React.ElementType;
    color:    string;
    subtitle?: string;
}) {
    return (
        <div style={{ background: "white", borderRadius: "16px", border: "1px solid #f1f5f9", padding: "20px", boxShadow: "0 1px 3px rgba(0,0,0,0.06)" }}>
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                <div>
                    <p style={{ fontSize: "13px", color: "#64748b", margin: "0 0 8px" }}>{title}</p>
                    <p style={{ fontSize: "20px", fontWeight: "700", color: "#1e293b", margin: 0 }}>{value}</p>
                    {subtitle && <p style={{ fontSize: "12px", color: "#94a3b8", margin: "4px 0 0" }}>{subtitle}</p>}
                </div>
                <div style={{ width: "44px", height: "44px", borderRadius: "12px", backgroundColor: `${color}15`, display: "flex", alignItems: "center", justifyContent: "center" }}>
                    <Icon size={22} color={color} />
                </div>
            </div>
        </div>
    );
}

// ── Pay Modal ─────────────────────────────────────────────
function PayModal({
    payment,
    onClose,
    onPay
}: {
    payment:  Payment | null;
    onClose:  () => void;
    onPay:    (method: string, reference: string) => void;
}) {
    const [method, setMethod]       = useState("Cash");
    const [reference, setReference] = useState("");

    if (!payment) return null;

    return (
        <div style={{ position: "fixed", inset: 0, backgroundColor: "rgba(0,0,0,0.5)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000 }}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "420px" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "8px", color: "#1e293b" }}>
                    تسجيل دفعة
                </h2>
                <p style={{ fontSize: "14px", color: "#64748b", marginBottom: "24px" }}>
                    {payment.propertyTitle} — {payment.clientName}
                </p>

                {/* Amount */}
                <div style={{ backgroundColor: "#f0fdf4", borderRadius: "12px", padding: "16px", marginBottom: "20px", textAlign: "center" }}>
                    <p style={{ fontSize: "12px", color: "#16a34a", margin: "0 0 4px" }}>المبلغ المطلوب</p>
                    <p style={{ fontSize: "28px", fontWeight: "700", color: "#15803d", margin: 0 }}>
                        {payment.amount.toLocaleString()} ﷼
                    </p>
                </div>

                {/* Method */}
                <div style={{ marginBottom: "16px" }}>
                    <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "8px" }}>
                        طريقة الدفع
                    </label>
                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "8px" }}>
                        {Object.entries(methodLabel).map(([key, label]) => (
                            <button
                                key={key}
                                type="button"
                                onClick={() => setMethod(key)}
                                style={{
                                    padding: "10px", borderRadius: "8px", cursor: "pointer",
                                    fontSize: "13px", fontWeight: "500",
                                    backgroundColor: method === key ? "#2563eb" : "#f8fafc",
                                    color: method === key ? "white" : "#374151",
                                    border: `1px solid ${method === key ? "#2563eb" : "#e2e8f0"}`
                                }}>
                                {label}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Reference */}
                <div style={{ marginBottom: "24px" }}>
                    <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                        رقم المرجع (اختياري)
                    </label>
                    <input
                        value={reference}
                        onChange={e => setReference(e.target.value)}
                        placeholder="رقم الإيصال أو التحويل..."
                        style={{ width: "100%", border: "1px solid #d1d5db", borderRadius: "8px", padding: "10px 12px", fontSize: "14px", boxSizing: "border-box" }}
                    />
                </div>

                <div style={{ display: "flex", gap: "12px" }}>
                    <button
                        onClick={onClose}
                        style={{ flex: 1, padding: "12px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer", fontSize: "14px" }}>
                        إلغاء
                    </button>
                    <button
                        onClick={() => onPay(method, reference)}
                        style={{ flex: 1, padding: "12px", borderRadius: "8px", border: "none", backgroundColor: "#16a34a", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                        تأكيد الدفع ✓
                    </button>
                </div>
            </div>
        </div>
    );
}

// ── Payment Row ───────────────────────────────────────────
function PaymentRow({
    payment,
    onPay,
    onCancel
}: {
    payment:  Payment;
    onPay:    (p: Payment) => void;
    onCancel: (id: string) => void;
}) {
    return (
        <div style={{
            display: "flex", alignItems: "center", justifyContent: "space-between",
            padding: "16px 20px",
            backgroundColor: payment.status === "Late" ? "#fff7f7" : "white",
            borderRadius: "12px",
            border: `1px solid ${payment.status === "Late" ? "#fecaca" : "#f1f5f9"}`,
            marginBottom: "8px"
        }}>
            <div style={{ display: "flex", alignItems: "center", gap: "16px", flex: 1 }}>
                {/* Status Icon */}
                <div style={{
                    width: "40px", height: "40px", borderRadius: "10px",
                    backgroundColor: `${statusColor[payment.status]}15`,
                    display: "flex", alignItems: "center", justifyContent: "center"
                }}>
                    {payment.status === "Paid"      && <CheckCircle size={20} color="#16a34a" />}
                    {payment.status === "Pending"   && <Clock size={20} color="#d97706" />}
                    {payment.status === "Late"      && <AlertTriangle size={20} color="#dc2626" />}
                    {payment.status === "Cancelled" && <XCircle size={20} color="#64748b" />}
                </div>

                {/* Info */}
                <div style={{ flex: 1 }}>
                    <p style={{ fontSize: "14px", fontWeight: "600", color: "#1e293b", margin: "0 0 2px" }}>
                        {payment.propertyTitle}
                    </p>
                    <p style={{ fontSize: "13px", color: "#64748b", margin: 0 }}>
                        {payment.clientName} — {payment.clientPhone}
                    </p>
                </div>

                {/* Date */}
                <div style={{ textAlign: "center" }}>
                    <p style={{ fontSize: "12px", color: "#94a3b8", margin: "0 0 2px" }}>تاريخ الاستحقاق</p>
                    <p style={{ fontSize: "13px", fontWeight: "500", color: "#374151", margin: 0 }}>
                        {new Date(payment.dueDate).toLocaleDateString("ar-SA")}
                    </p>
                    {payment.daysOverdue > 0 && (
                        <p style={{ fontSize: "11px", color: "#dc2626", margin: "2px 0 0" }}>
                            متأخر {payment.daysOverdue} يوم
                        </p>
                    )}
                </div>
            </div>

            {/* Amount + Actions */}
            <div style={{ display: "flex", alignItems: "center", gap: "16px", marginRight: "16px" }}>
                <div style={{ textAlign: "left" }}>
                    <p style={{ fontSize: "16px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                        {payment.amount.toLocaleString()} ﷼
                    </p>
                    {payment.method && (
                        <p style={{ fontSize: "12px", color: "#94a3b8", margin: "2px 0 0" }}>
                            {methodLabel[payment.method] || payment.method}
                        </p>
                    )}
                </div>

                <span style={{
                    padding: "4px 12px", borderRadius: "20px", fontSize: "12px", fontWeight: "600",
                    backgroundColor: `${statusColor[payment.status]}15`,
                    color: statusColor[payment.status]
                }}>
                    {statusLabel[payment.status]}
                </span>

                {(payment.status === "Pending" || payment.status === "Late") && (
                    <div style={{ display: "flex", gap: "8px" }}>
                        <button
                            onClick={() => onPay(payment)}
                            style={{ display: "flex", alignItems: "center", gap: "6px", padding: "8px 14px", borderRadius: "8px", border: "none", backgroundColor: "#16a34a", color: "white", cursor: "pointer", fontSize: "13px", fontWeight: "500" }}>
                            <CheckCircle size={14} />
                            دفع
                        </button>
                        <button
                            onClick={() => onCancel(payment.id)}
                            style={{ padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#f8fafc", color: "#94a3b8", cursor: "pointer" }}>
                            <XCircle size={16} />
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function Payments() {
    const [overduePayments, setOverdue]   = useState<Payment[]>([]);
    const [upcomingPayments, setUpcoming] = useState<Payment[]>([]);
    const [summary, setSummary]           = useState<PaymentSummary | null>(null);
    const [loading, setLoading]           = useState(true);
    const [search, setSearch]             = useState("");
    const [activeTab, setTab]             = useState<"overdue" | "upcoming">("overdue");
    const [payModal, setPayModal]         = useState<Payment | null>(null);

    async function loadAll() {
        setLoading(true);
        try {
            const [overdueData, upcomingData, summaryData] = await Promise.all([
                paymentsService.getOverdue(),
                paymentsService.getUpcoming(30),
                paymentsService.getSummary()
            ]);
            setOverdue(overdueData ?? []);
            setUpcoming(upcomingData ?? []);
            setSummary(summaryData);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => { loadAll(); }, []);

    async function handlePay(method: string, reference: string) {
        if (!payModal) return;
        try {
            await paymentsService.markPaid(payModal.id, method, reference);
            toast.success("تم تسجيل الدفعة بنجاح ✅");
            setPayModal(null);
            await loadAll();
        } catch {
            toast.error("حدث خطأ ❌");
        }
    }

    async function handleCancel(id: string) {
        if (!confirm("هل أنت متأكد من إلغاء هذه الدفعة؟")) return;
        try {
            await paymentsService.cancel(id);
            toast.success("تم إلغاء الدفعة 🗑️");
            await loadAll();
        } catch {
            toast.error("فشل الإلغاء ❌");
        }
    }

    const currentList = activeTab === "overdue" ? overduePayments : upcomingPayments;
    const filtered = currentList.filter(p =>
        !search ||
        p.propertyTitle.includes(search) ||
        p.clientName.includes(search)
    );

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            {/* Header */}
            <div style={{ marginBottom: "24px" }}>
                <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: "0 0 4px" }}>
                    المدفوعات
                </h1>
                <p style={{ color: "#64748b", fontSize: "14px", margin: 0 }}>
                    إدارة ومتابعة دفعات الإيجار
                </p>
            </div>

            {/* Summary Cards */}
            {summary && (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(220px, 1fr))", gap: "16px", marginBottom: "24px" }}>
                    <SummaryCard
                        title="إجمالي المحصّل"
                        value={`${summary.totalCollected.toLocaleString()} ﷼`}
                        icon={TrendingUp}
                        color="#16a34a"
                        subtitle="من إجمالي العقود"
                    />
                    <SummaryCard
                        title="المتأخرات"
                        value={`${summary.totalOverdue.toLocaleString()} ﷼`}
                        icon={AlertTriangle}
                        color="#dc2626"
                        subtitle={`${summary.overdueCount} دفعة متأخرة`}
                    />
                    <SummaryCard
                        title="المعلقة"
                        value={`${summary.totalPending.toLocaleString()} ﷼`}
                        icon={Clock}
                        color="#d97706"
                        subtitle={`${summary.pendingCount} دفعة قادمة`}
                    />
                    <SummaryCard
                        title="إجمالي المتوقع"
                        value={`${summary.totalExpected.toLocaleString()} ﷼`}
                        icon={DollarSign}
                        color="#2563eb"
                        subtitle="من كل العقود"
                    />
                </div>
            )}

            {/* Tabs */}
            <div style={{ display: "flex", gap: "8px", marginBottom: "20px" }}>
                <button
                    onClick={() => setTab("overdue")}
                    style={{
                        display: "flex", alignItems: "center", gap: "8px",
                        padding: "10px 20px", borderRadius: "10px", cursor: "pointer",
                        fontSize: "14px", fontWeight: "500", border: "none",
                        backgroundColor: activeTab === "overdue" ? "#dc2626" : "#f8fafc",
                        color: activeTab === "overdue" ? "white" : "#64748b"
                    }}>
                    <AlertTriangle size={16} />
                    المتأخرة ({overduePayments.length})
                </button>
                <button
                    onClick={() => setTab("upcoming")}
                    style={{
                        display: "flex", alignItems: "center", gap: "8px",
                        padding: "10px 20px", borderRadius: "10px", cursor: "pointer",
                        fontSize: "14px", fontWeight: "500", border: "none",
                        backgroundColor: activeTab === "upcoming" ? "#2563eb" : "#f8fafc",
                        color: activeTab === "upcoming" ? "white" : "#64748b"
                    }}>
                    <Clock size={16} />
                    القادمة ({upcomingPayments.length})
                </button>
            </div>

            {/* Search */}
            <div style={{ position: "relative", marginBottom: "20px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث بالعقار أو العميل..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }}
                />
            </div>

            {/* Content */}
            {loading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "60px", color: "#94a3b8" }}>
                    <CreditCard size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>
                        {activeTab === "overdue" ? "لا توجد مدفوعات متأخرة 🎉" : "لا توجد مدفوعات قادمة"}
                    </p>
                </div>
            ) : (
                <div>
                    {filtered.map(payment => (
                        <PaymentRow
                            key={payment.id}
                            payment={payment}
                            onPay={setPayModal}
                            onCancel={handleCancel}
                        />
                    ))}
                </div>
            )}

            {/* Pay Modal */}
            <PayModal
                payment={payModal}
                onClose={() => setPayModal(null)}
                onPay={handlePay}
            />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}