import { Apartment } from "./apartment"

export interface Member {
    id: number;
    userName: string;
    knownAs: string;
    gender: string;
    apartments: Apartment[];
}