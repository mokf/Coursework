import { Photo } from "./photo"

export interface Apartment {
    photoUrl: string;
    title: string;
    details: string;
    city: string;
    country: string;
    price: number;
    maxPeople: number;
    hasTV: boolean;
    hasWifi: boolean;
    isAvaliable: boolean;
    photos: Photo[];
    id: number;
}
