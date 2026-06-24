// src/pages/Users.tsx
import { useState } from "react";
import {
    Users, Plus, Search, Edit2,
    Trash2, Key, Shield, ShieldCheck,
    ShieldAlert, CheckCircle, XCircle
} from "lucide-react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import toast from "react-hot-toast";
import { usersService, type User } from "../services/users.service";
import { useAuthStore } from "../store/authStore";

// ── Helpers ───────────────────────────────────────────────
const roleLabel: Record<string, string> = {
    Owner: "مالك",
    Admin: "مدير",
    Agent: "وكيل"
};

const roleColor: Record<string, string> = {
    Owner: "#7c3aed",
    Admin: "#2563eb",
    Agent: "#16a34a"
};

const roleIcon: Record<string, React.ElementType> = {
    Owner: ShieldAlert,
    Admin: ShieldCheck,
    Agent: Shield
};

// ── Schemas ───────────────────────────────────────────────
const createSchema = z.object({
    fullName: z.string().min(1, "الاسم مطلوب"),
    email:    z.string().email("البريد غير صحيح"),
    phone:    z.string().regex(/^05\d{8}$/, "رقم الجوال غير صحيح"),
    password: z.string().min(8, "كلمة المرور 8 أحرف على الأقل")
                        .regex(/[A-Z]/, "يجب أن تحتوي على حرف كبير")
                        .regex(/[0-9]/, "يجب أن تحتوي على رقم"),
    role:     z.string().min(1, "الدور مطلوب")
});

const updateSchema = z.object({
    fullName: z.string().min(1, "الاسم مطلوب"),
    phone:    z.string().regex(/^05\d{8}$/, "رقم الجوال غير صحيح"),
    role:     z.string().min(1, "الدور مطلوب"),
    isActive: z.boolean()
});

const passwordSchema = z.object({
    oldPassword: z.string().min(1, "كلمة المرور الحالية مطلوبة"),
    newPassword: z.string().min(8, "كلمة المرور 8 أحرف على الأقل")
                           .regex(/[A-Z]/, "يجب أن تحتوي على حرف كبير")
                           .regex(/[0-9]/, "يجب أن تحتوي على رقم"),
    confirmPassword: z.string()
}).refine(d => d.newPassword === d.confirmPassword, {
    message: "كلمة المرور غير متطابقة",
    path: ["confirmPassword"]
});

type CreateForm   = z.infer<typeof createSchema>;
type UpdateForm   = z.infer<typeof updateSchema>;
type PasswordForm = z.infer<typeof passwordSchema>;

// ── Shared Styles ─────────────────────────────────────────
const inputStyle: React.CSSProperties = {
    width: "100%", border: "1px solid #d1d5db",
    borderRadius: "8px", padding: "10px 12px",
    fontSize: "14px", boxSizing: "border-box"
};
const modalOverlay: React.CSSProperties = {
    position: "fixed", inset: 0,
    backgroundColor: "rgba(0,0,0,0.5)",
    display: "flex", alignItems: "center",
    justifyContent: "center", zIndex: 1000
};

function Field({ label, error, children }: {
    label:    string;
    error?:   string;
    children: React.ReactNode;
}) {
    return (
        <div>
            <label style={{ fontSize: "13px", fontWeight: "500", color: "#374151", display: "block", marginBottom: "6px" }}>
                {label}
            </label>
            {children}
            {error && <p style={{ color: "#dc2626", fontSize: "12px", marginTop: "4px" }}>{error}</p>}
        </div>
    );
}

// ── User Card ─────────────────────────────────────────────
function UserCard({ user, currentUserId, onEdit, onDelete, onChangePassword }: {
    user:            User;
    currentUserId:   string;
    onEdit:          (u: User) => void;
    onDelete:        (u: User) => void;
    onChangePassword:(u: User) => void;
}) {
    const RoleIcon = roleIcon[user.role] ?? Shield;
    const isMe = user.id === currentUserId;

    return (
        <div style={{
            background: "white", borderRadius: "16px",
            border: `1px solid ${isMe ? "#bfdbfe" : "#f1f5f9"}`,
            padding: "20px",
            boxShadow: isMe ? "0 0 0 2px #eff6ff" : "0 1px 3px rgba(0,0,0,0.06)"
        }}>
            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "16px" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                    <div style={{
                        width: "44px", height: "44px", borderRadius: "50%",
                        backgroundColor: `${roleColor[user.role]}15`,
                        display: "flex", alignItems: "center", justifyContent: "center"
                    }}>
                        <RoleIcon size={20} color={roleColor[user.role]} />
                    </div>
                    <div>
                        <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                            <h3 style={{ fontSize: "15px", fontWeight: "600", color: "#1e293b", margin: 0 }}>
                                {user.fullName}
                            </h3>
                            {isMe && (
                                <span style={{ fontSize: "11px", backgroundColor: "#eff6ff", color: "#2563eb", padding: "2px 8px", borderRadius: "10px", fontWeight: "600" }}>
                                    أنت
                                </span>
                            )}
                        </div>
                        <p style={{ fontSize: "13px", color: "#94a3b8", margin: "2px 0 0" }}>
                            {user.email}
                        </p>
                    </div>
                </div>

                <div style={{ display: "flex", flexDirection: "column", alignItems: "flex-end", gap: "6px" }}>
                    <span style={{
                        padding: "3px 10px", borderRadius: "20px", fontSize: "12px", fontWeight: "600",
                        backgroundColor: `${roleColor[user.role]}15`,
                        color: roleColor[user.role]
                    }}>
                        {roleLabel[user.role]}
                    </span>
                    <div style={{ display: "flex", alignItems: "center", gap: "4px", fontSize: "12px" }}>
                        {user.isActive
                            ? <><CheckCircle size={13} color="#16a34a" /><span style={{ color: "#16a34a" }}>نشط</span></>
                            : <><XCircle size={13} color="#dc2626" /><span style={{ color: "#dc2626" }}>موقوف</span></>
                        }
                    </div>
                </div>
            </div>

            {/* Phone */}
            <p style={{ fontSize: "13px", color: "#64748b", margin: "0 0 16px", direction: "ltr", textAlign: "right" }}>
                {user.phone}
            </p>

            {/* Actions */}
            <div style={{ display: "flex", gap: "8px", borderTop: "1px solid #f1f5f9", paddingTop: "12px" }}>
                <button
                    onClick={() => onEdit(user)}
                    style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", gap: "6px", padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#eff6ff", color: "#2563eb", cursor: "pointer", fontSize: "13px" }}>
                    <Edit2 size={14} /> تعديل
                </button>
                <button
                    onClick={() => onChangePassword(user)}
                    style={{ flex: 1, display: "flex", alignItems: "center", justifyContent: "center", gap: "6px", padding: "8px", borderRadius: "8px", border: "none", backgroundColor: "#fef3c7", color: "#d97706", cursor: "pointer", fontSize: "13px" }}>
                    <Key size={14} /> كلمة المرور
                </button>
                {!isMe && (
                    <button
                        onClick={() => onDelete(user)}
                        style={{ padding: "8px 12px", borderRadius: "8px", border: "none", backgroundColor: "#fef2f2", color: "#dc2626", cursor: "pointer" }}>
                        <Trash2 size={14} />
                    </button>
                )}
            </div>
        </div>
    );
}

// ── Create Modal ──────────────────────────────────────────
function CreateUserModal({ isOpen, onClose, onSave }: {
    isOpen:  boolean;
    onClose: () => void;
    onSave:  (data: CreateForm) => void;
}) {
    const { register, handleSubmit, reset, formState: { errors } } =
        useForm<CreateForm>({ resolver: zodResolver(createSchema) });

    if (!isOpen) return null;

    return (
        <div style={modalOverlay}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "500px", maxHeight: "90vh", overflowY: "auto" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    إضافة مستخدم جديد
                </h2>
                <form onSubmit={handleSubmit(d => { onSave(d); reset(); })}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                        <Field label="الاسم الكامل *" error={errors.fullName?.message}>
                            <input {...register("fullName")} placeholder="محمد العمري" style={inputStyle} />
                        </Field>
                        <Field label="البريد الإلكتروني *" error={errors.email?.message}>
                            <input {...register("email")} type="email" placeholder="user@company.com" style={inputStyle} dir="ltr" />
                        </Field>
                        <Field label="رقم الجوال *" error={errors.phone?.message}>
                            <input {...register("phone")} placeholder="05xxxxxxxx" style={inputStyle} dir="ltr" />
                        </Field>
                        <Field label="كلمة المرور *" error={errors.password?.message}>
                            <input {...register("password")} type="password" placeholder="••••••••" style={inputStyle} />
                        </Field>
                        <Field label="الدور *" error={errors.role?.message}>
                            <select {...register("role")} style={inputStyle}>
                                <option value="">اختر الدور</option>
                                <option value="Admin">مدير</option>
                                <option value="Agent">وكيل</option>
                            </select>
                        </Field>
                    </div>
                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer" }}>
                            إلغاء
                        </button>
                        <button type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontWeight: "500" }}>
                            إضافة المستخدم
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Edit Modal ────────────────────────────────────────────
function EditUserModal({ user, onClose, onSave }: {
    user:    User | null;
    onClose: () => void;
    onSave:  (id: string, data: UpdateForm) => void;
}) {
    const { register, handleSubmit, formState: { errors } } =
        useForm<UpdateForm>({
            resolver: zodResolver(updateSchema),
            values: user ? {
                fullName: user.fullName,
                phone:    user.phone,
                role:     user.role,
                isActive: user.isActive
            } : undefined
        });

    if (!user) return null;

    return (
        <div style={modalOverlay}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "460px" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "24px", color: "#1e293b" }}>
                    تعديل المستخدم
                </h2>
                <form onSubmit={handleSubmit(d => onSave(user.id, d))}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                        <Field label="الاسم الكامل *" error={errors.fullName?.message}>
                            <input {...register("fullName")} style={inputStyle} />
                        </Field>
                        <Field label="رقم الجوال *" error={errors.phone?.message}>
                            <input {...register("phone")} style={inputStyle} dir="ltr" />
                        </Field>
                        <Field label="الدور *" error={errors.role?.message}>
                            <select {...register("role")} style={inputStyle}>
                                <option value="Owner">مالك</option>
                                <option value="Admin">مدير</option>
                                <option value="Agent">وكيل</option>
                            </select>
                        </Field>
                        <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
                            <input {...register("isActive")} type="checkbox" id="isActive"
                                style={{ width: "16px", height: "16px", cursor: "pointer" }} />
                            <label htmlFor="isActive" style={{ fontSize: "14px", color: "#374151", cursor: "pointer" }}>
                                الحساب نشط
                            </label>
                        </div>
                    </div>
                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer" }}>
                            إلغاء
                        </button>
                        <button type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontWeight: "500" }}>
                            حفظ التعديلات
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Change Password Modal ─────────────────────────────────
function ChangePasswordModal({ user, onClose, onSave }: {
    user:    User | null;
    onClose: () => void;
    onSave:  (userId: string, oldPassword: string, newPassword: string) => void;
}) {
    const { register, handleSubmit, reset, formState: { errors } } =
        useForm<PasswordForm>({ resolver: zodResolver(passwordSchema) });

    if (!user) return null;

    return (
        <div style={modalOverlay}>
            <div style={{ backgroundColor: "white", borderRadius: "16px", padding: "32px", width: "440px" }}>
                <h2 style={{ fontSize: "18px", fontWeight: "700", marginBottom: "8px", color: "#1e293b" }}>
                    تغيير كلمة المرور
                </h2>
                <p style={{ fontSize: "14px", color: "#64748b", marginBottom: "24px" }}>
                    {user.fullName}
                </p>
                <form onSubmit={handleSubmit(d => { onSave(user.id, d.oldPassword, d.newPassword); reset(); })}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                        <Field label="كلمة المرور الحالية *" error={errors.oldPassword?.message}>
                            <input {...register("oldPassword")} type="password" placeholder="••••••••" style={inputStyle} />
                        </Field>
                        <Field label="كلمة المرور الجديدة *" error={errors.newPassword?.message}>
                            <input {...register("newPassword")} type="password" placeholder="••••••••" style={inputStyle} />
                        </Field>
                        <Field label="تأكيد كلمة المرور *" error={errors.confirmPassword?.message}>
                            <input {...register("confirmPassword")} type="password" placeholder="••••••••" style={inputStyle} />
                        </Field>
                    </div>
                    <div style={{ display: "flex", gap: "12px", marginTop: "24px", justifyContent: "flex-end" }}>
                        <button type="button" onClick={onClose}
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "1px solid #d1d5db", backgroundColor: "white", color: "#374151", cursor: "pointer" }}>
                            إلغاء
                        </button>
                        <button type="submit"
                            style={{ padding: "10px 20px", borderRadius: "8px", border: "none", backgroundColor: "#d97706", color: "white", cursor: "pointer", fontWeight: "500" }}>
                            تغيير كلمة المرور
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// ── Main Page ─────────────────────────────────────────────
export default function UsersPage() {
    const queryClient  = useQueryClient();
    const currentUser  = useAuthStore(s => s.user);
    const [search, setSearch]           = useState("");
    const [createOpen, setCreateOpen]   = useState(false);
    const [editUser, setEditUser]       = useState<User | null>(null);
    const [passUser, setPassUser]       = useState<User | null>(null);

    const { data: users = [], isLoading } = useQuery<User[]>({
        queryKey: ["users"],
        queryFn:  () => usersService.getAll()
    });

    const invalidate = () => queryClient.invalidateQueries({ queryKey: ["users"] });

    const createMutation = useMutation({
        mutationFn: usersService.create,
        onSuccess: () => { toast.success("تم إضافة المستخدم ✅"); invalidate(); setCreateOpen(false); },
        onError:   () => toast.error("فشل إضافة المستخدم ❌")
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, ...rest }: { id: string } & UpdateForm) =>
            usersService.update(id, rest),
        onSuccess: () => { toast.success("تم حفظ التعديلات ✅"); invalidate(); setEditUser(null); },
        onError:   () => toast.error("فشل التعديل ❌")
    });

    const passwordMutation = useMutation({
        mutationFn: ({ userId, oldPassword, newPassword }: {
            userId: string; oldPassword: string; newPassword: string
        }) => usersService.changePassword(userId, oldPassword, newPassword),
        onSuccess: () => { toast.success("تم تغيير كلمة المرور ✅"); setPassUser(null); },
        onError:   () => toast.error("كلمة المرور الحالية غير صحيحة ❌")
    });

    const deleteMutation = useMutation({
        mutationFn: usersService.delete,
        onSuccess: () => { toast.success("تم حذف المستخدم ✅"); invalidate(); },
        onError:   () => toast.error("فشل الحذف ❌")
    });

    function handleDelete(user: User) {
        if (confirm(`هل أنت متأكد من حذف "${user.fullName}"؟`))
            deleteMutation.mutate(user.id);
    }

    const filtered = users.filter(u =>
        !search ||
        u.fullName.includes(search) ||
        u.email.includes(search) ||
        u.phone.includes(search)
    );

    const roleCount = (role: string) => users.filter(u => u.role === role).length;

    return (
        <div style={{ padding: "24px" }} dir="rtl">

            {/* Header */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                <div>
                    <h1 style={{ fontSize: "22px", fontWeight: "700", color: "#1e293b", margin: 0 }}>
                        المستخدمين
                    </h1>
                    <p style={{ color: "#64748b", fontSize: "14px", margin: "4px 0 0" }}>
                        {users.length} مستخدم مسجل
                    </p>
                </div>
                <button
                    onClick={() => setCreateOpen(true)}
                    style={{ display: "flex", alignItems: "center", gap: "8px", padding: "10px 20px", borderRadius: "10px", border: "none", backgroundColor: "#2563eb", color: "white", cursor: "pointer", fontSize: "14px", fontWeight: "500" }}>
                    <Plus size={18} />
                    إضافة مستخدم
                </button>
            </div>

            {/* Role Stats */}
            <div style={{ display: "flex", gap: "12px", marginBottom: "20px" }}>
                {Object.entries(roleLabel).map(([key, label]) => {
                    const RoleIcon = roleIcon[key] ?? Shield;
                    return (
                        <div key={key} style={{
                            display: "flex", alignItems: "center", gap: "10px",
                            padding: "12px 20px", borderRadius: "12px",
                            backgroundColor: `${roleColor[key]}10`,
                            border: `1px solid ${roleColor[key]}30`
                        }}>
                            <RoleIcon size={18} color={roleColor[key]} />
                            <div>
                                <p style={{ fontSize: "18px", fontWeight: "700", color: roleColor[key], margin: 0 }}>
                                    {roleCount(key)}
                                </p>
                                <p style={{ fontSize: "12px", color: "#64748b", margin: 0 }}>{label}</p>
                            </div>
                        </div>
                    );
                })}
            </div>

            {/* Search */}
            <div style={{ position: "relative", marginBottom: "24px" }}>
                <Search size={16} style={{ position: "absolute", right: "12px", top: "50%", transform: "translateY(-50%)", color: "#94a3b8" }} />
                <input
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                    placeholder="ابحث بالاسم أو البريد أو الجوال..."
                    style={{ width: "100%", padding: "10px 40px 10px 12px", border: "1px solid #e2e8f0", borderRadius: "10px", fontSize: "14px", boxSizing: "border-box", backgroundColor: "white" }}
                />
            </div>

            {/* Content */}
            {isLoading ? (
                <div style={{ display: "flex", justifyContent: "center", padding: "80px" }}>
                    <div style={{ width: "40px", height: "40px", border: "3px solid #e2e8f0", borderTopColor: "#2563eb", borderRadius: "50%", animation: "spin 1s linear infinite" }} />
                </div>
            ) : filtered.length === 0 ? (
                <div style={{ textAlign: "center", padding: "80px", color: "#94a3b8" }}>
                    <Users size={48} style={{ margin: "0 auto 16px", opacity: 0.3 }} />
                    <p style={{ fontSize: "16px" }}>لا يوجد مستخدمين</p>
                </div>
            ) : (
                <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(340px, 1fr))", gap: "20px" }}>
                    {filtered.map(user => (
                        <UserCard
                            key={user.id}
                            user={user}
                            currentUserId={currentUser?.id ?? ""}
                            onEdit={setEditUser}
                            onDelete={handleDelete}
                            onChangePassword={setPassUser}
                        />
                    ))}
                </div>
            )}

            {/* Modals */}
            <CreateUserModal
                isOpen={createOpen}
                onClose={() => setCreateOpen(false)}
                onSave={data => createMutation.mutate({ ...data })}
            />

            <EditUserModal
                user={editUser}
                onClose={() => setEditUser(null)}
                onSave={(id, data) => updateMutation.mutate({ id, ...data })}
            />

            <ChangePasswordModal
                user={passUser}
                onClose={() => setPassUser(null)}
                onSave={(userId, oldPassword, newPassword) =>
                    passwordMutation.mutate({ userId, oldPassword, newPassword })}
            />

            <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
        </div>
    );
}