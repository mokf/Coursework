import { User } from "./user";

export class ApartmentParams {
    price: string = 'priceAsc';
    people: string = 'peopleAsc';
    hasTV: string = '';
    hasWifi: string = '';
    isAvaliable: string = 'yes';
    pageNumber = 1;
    pageSize = 6;

    constructor(user: User) {}
}