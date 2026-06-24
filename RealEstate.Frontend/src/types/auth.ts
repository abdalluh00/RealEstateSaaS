export interface LoginRequest {
    email:    string;
    password: string;
}

export interface AuthUser {
    id:        string;
    token:     string;
    fullName:  string;
    email:     string;
    role:      string;
    companyId: string;
    expiresAt: string;
}