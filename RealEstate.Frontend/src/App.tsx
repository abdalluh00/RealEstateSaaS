// src/App.tsx
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Layout   from "./components/Layout";
import Login    from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Properties from "./pages/Properties";
import Clients from "./pages/Clients";
import Contracts from "./pages/Contracts";
import Appointments from "./pages/Appointments";
import Owners from "./pages/Owners";
import Payments from "./pages/Payments";
import UsersPage from "./pages/Users";
import Settings from "./pages/Settings";


export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/" element={<Layout />}>
                    <Route index element={<Navigate to="/dashboard" replace />} />
                    <Route path="dashboard" element={<Dashboard />} />
                    <Route path="properties" element={<Properties />} />
                    <Route path="clients" element={<Clients />} />
                    <Route path="contracts" element={<Contracts />} />
                    <Route path="appointments" element={<Appointments />} />
                    <Route path="owners" element={<Owners />} />
                    <Route path="payments" element={<Payments />} />
                    <Route path="users" element={<UsersPage />} />
                    <Route path="settings" element={<Settings />} />
                    
                    {/* باقي الصفحات نضيفها لاحقاً */}
                </Route>
            </Routes>
        </BrowserRouter>
    );
}




