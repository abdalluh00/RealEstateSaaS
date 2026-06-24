// src/components/Sidebar.tsx
import { NavLink, useNavigate } from "react-router-dom";
import {
    LayoutDashboard, Building2, Users, FileText,
    Calendar, CreditCard, LogOut, UserCheck ,
} from "lucide-react";
import { Settings as SettingsIcon } from "lucide-react";
import { useAuthStore } from "../store/authStore";


const links = [
    { to: "/dashboard",    icon: LayoutDashboard, label: "الرئيسية" },
    { to: "/properties",   icon: Building2,        label: "العقارات" },
    { to: "/clients",      icon: Users,            label: "العملاء" },
    { to: "/contracts",    icon: FileText,         label: "العقود" },
    { to: "/appointments", icon: Calendar,         label: "المواعيد" },
    { to: "/payments",     icon: CreditCard,       label: "المدفوعات" },
    { to: "/Owners",       icon: UserCheck ,           label: "المالك" },
    { to: "/users", icon: Users, label: "المستخدمين" },
    { to: "/settings", icon: SettingsIcon, label: "الإعدادات" },
];

export default function Sidebar() {
    const navigate = useNavigate();
    const logout   = useAuthStore(s => s.logout);
    const user     = useAuthStore(s => s.user);

    function handleLogout() {
        logout();
        navigate("/login");
    }

    return (
        <aside style={{ width: "256px", minHeight: "100vh", backgroundColor: "#1e3a8a", display: "flex", flexDirection: "column" }}>

            {/* Logo */}
            <div style={{ padding: "24px", borderBottom: "1px solid #1e40af" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                    <div style={{ width: "40px", height: "40px", backgroundColor: "#2563eb", borderRadius: "12px", display: "flex", alignItems: "center", justifyContent: "center" }}>
                        <Building2 size={20} color="white" />
                    </div>
                    <div>
                        <div style={{ color: "white", fontWeight: "bold", fontSize: "14px" }}>نظام العقارات</div>
                        <div style={{ color: "#93c5fd", fontSize: "12px" }}>{user?.role}</div>
                    </div>
                </div>
            </div>

            {/* Links */}
            <nav style={{ flex: 1, padding: "16px", display: "flex", flexDirection: "column", gap: "4px" }}>
                {links.map(({ to, icon: Icon, label }) => (
                    <NavLink
                        key={to}
                        to={to}
                        style={({ isActive }) => ({
                            display: "flex",
                            alignItems: "center",
                            gap: "12px",
                            padding: "12px 16px",
                            borderRadius: "12px",
                            textDecoration: "none",
                            fontSize: "14px",
                            fontWeight: "500",
                            transition: "all 0.2s",
                            backgroundColor: isActive ? "#2563eb" : "transparent",
                            color: isActive ? "white" : "#bfdbfe",
                        })}
                        onMouseEnter={e => {
                            const target = e.currentTarget;
                            if (!target.classList.contains("active"))
                                target.style.backgroundColor = "#1e40af";
                        }}
                        onMouseLeave={e => {
                            const target = e.currentTarget;
                            if (!target.classList.contains("active"))
                                target.style.backgroundColor = "transparent";
                        }}
                    >
                        <Icon size={20} />
                        {label}
                    </NavLink>
                ))}
            </nav>

            {/* User + Logout */}
            <div style={{ padding: "16px", borderTop: "1px solid #1e40af" }}>
                <div style={{ display: "flex", alignItems: "center", gap: "12px", padding: "12px 16px", marginBottom: "8px" }}>
                    <div style={{ width: "32px", height: "32px", backgroundColor: "#2563eb", borderRadius: "50%", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontSize: "14px" }}>
                        {user?.fullName[0]}
                    </div>
                    <div style={{ flex: 1, overflow: "hidden" }}>
                        <div style={{ color: "white", fontSize: "14px", fontWeight: "500", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
                            {user?.fullName}
                        </div>
                        <div style={{ color: "#93c5fd", fontSize: "12px", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
                            {user?.email}
                        </div>
                    </div>
                </div>
                <button
                    onClick={handleLogout}
                    style={{ display: "flex", alignItems: "center", gap: "12px", padding: "12px 16px", borderRadius: "12px", color: "#bfdbfe", background: "none", border: "none", cursor: "pointer", fontSize: "14px", width: "100%", transition: "all 0.2s" }}
                    onMouseEnter={e => (e.currentTarget.style.backgroundColor = "#dc2626")}
                    onMouseLeave={e => (e.currentTarget.style.backgroundColor = "transparent")}
                >
                    <LogOut size={20} />
                    تسجيل الخروج
                </button>
            </div>
        </aside>
    );
}