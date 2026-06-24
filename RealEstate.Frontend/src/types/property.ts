export interface Property {
    id:          string;
    title:       string;
    type:        string;
    status:      string;
    price:       number;
    area:        number;
    bedrooms:    number | null;
    bathrooms:   number | null;
    city:        string;
    district:    string;
    isFeatured:  boolean;
    createdAt:   string;
}

export interface CreatePropertyRequest {
    title:       string;
    type:        string;
    price:       number;
    area:        number;
    bedrooms?:   number;
    bathrooms?:  number;
    city:        string;
    district:    string;
    description?: string;
}