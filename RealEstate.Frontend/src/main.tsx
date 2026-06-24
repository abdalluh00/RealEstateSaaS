// src/main.tsx
import React from "react";
import ReactDOM from "react-dom/client";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "react-hot-toast";
import "./index.css";
import App from "./App.tsx";

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: 1,
            staleTime: 1000 * 60 * 5, // 5 دقائق
        }
    }
});

ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <App />
            <Toaster
                position="top-center"
                toastOptions={{
                    duration: 3000,
                    style: {
                        fontFamily: "Tajawal, sans-serif",
                        fontSize: "14px",
                        borderRadius: "10px",
                        direction: "rtl"
                    },
                    success: { style: { background: "#f0fdf4", color: "#16a34a", border: "1px solid #bbf7d0" } },
                    error:   { style: { background: "#fef2f2", color: "#dc2626", border: "1px solid #fecaca" } }
                }}
            />
        </QueryClientProvider>
    </React.StrictMode>
);