// src/services/api.ts
import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5171/api",
    headers: { "Content-Type": "application/json" }
});

// أضف Token تلقائياً لكل Request
api.interceptors.request.use(config => {
    const token = localStorage.getItem("token");
    if (token)
        config.headers.Authorization = `Bearer ${token}`;
    return config;
});

// معالجة الأخطاء تلقائياً
api.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.status === 401) {
            localStorage.removeItem("token");
            window.location.href = "/login";
        }
        return Promise.reject(error);
    }
); 

export default api;