export interface Client {
    id:         string;
    fullName:   string;
    phone:      string;
    email:      string | null;
    leadStatus: string;
    source:     string | null;
    createdAt:  string;
}