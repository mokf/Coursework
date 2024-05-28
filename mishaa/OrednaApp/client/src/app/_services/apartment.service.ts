import { Injectable } from '@angular/core';
import { BehaviorSubject, map, of, take } from 'rxjs';
import { Apartment } from '../_models/apartment';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Photo } from '../_models/photo';
import { CreateApartment } from '../_models/createApartment';
import { PaginatedResult } from '../_models/pagination';
import { ApartmentParams } from '../_models/apartmentParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class ApartmentService {
  private baseUrl = environment.apiUrl;
  private baseUrlUser = this.baseUrl + 'users/';
  private apartments: Apartment[] = [];
  //private apartmentCache = new Map();
  private user: User = {} as User;
  private apartmentParams: ApartmentParams | undefined;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.apartmentParams = new ApartmentParams(user);
          this.user = user;
        }
      }
    })
  }

  getApartmentParams() {
    return this.apartmentParams;
  }

  setApartmentParams(params: ApartmentParams) {
    this.apartmentParams = params;
  }

  resetApartmentParams() {
    if (this.user) {
      this.apartmentParams = new ApartmentParams(this.user);
      return this.apartmentParams;
    }
    return;
  }

  getApartments(apartmentParams: ApartmentParams, search?: string) {
    //const response = this.apartmentCache.get(Object.values(apartmentParams).join('-'))

    //if (response) return of(response);

    let params = this.getPaginationHeaders(apartmentParams.pageNumber, apartmentParams.pageSize);

    params = params.append('price', apartmentParams.price);
    params = params.append('people', apartmentParams.people);
    params = params.append('hasTV', apartmentParams.hasTV);
    params = params.append('hasWifi', apartmentParams.hasWifi);
    params = params.append('IsAvaliable', apartmentParams.isAvaliable);

    let url = this.baseUrl + 'apartments';
    if (search && search != '')
      url += '/search/' + search;

    return this.getPaginatedResult<Apartment[]>(url, params).pipe(
      map(response => {
        //this.apartmentCache.set(Object.values(apartmentParams).join('-'), response);
        return response;
      })
    );
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }

        return paginatedResult;
      })
    );
  }

  getApartment(id: number) {
    // const apartment = [...this.apartmentCache.values()].reduce((array, element) => array.concat(element.result), [])
    //   .find((apartment: Apartment) => apartment.id === id);

    //if (apartment) return of(apartment);

    return this.http.get<Apartment>(this.baseUrl + 'apartments/' + id);
  }

  updateApartment(apartmentId: number, apartment: Apartment) {
    return this.http.put(this.baseUrlUser + this.user.username + '/' + apartmentId + '/update', apartment);
  }

  setMainPhoto(photoId: number, apartmentId: number) {
    return this.http.put(this.baseUrlUser + this.user.username + '/' + apartmentId + '/set-main-photo/' + photoId, {})
  }

  deletePhoto(photoId: number, apartmentId: number) {
    return this.http.delete(this.baseUrlUser + this.user.username + '/' + apartmentId + '/delete-photo/' + photoId);
  }

  createApartment(model: any) {
    return this.http.post<CreateApartment>(this.baseUrlUser + this.user.username + '/create', model);
  }

  deleteApartment(apartmentId: number) {
    return this.http.delete(this.baseUrlUser + this.user.username + '/' + apartmentId + '/delete');
  }

  rentApartment(apartmentId: number) {
    return this.http.post(this.baseUrlUser + this.user.username + '/rent/' + apartmentId, {});
  }

  getRentedApartment() {
    return this.http.get<Apartment[]>(this.baseUrlUser + this.user.username + '/rented-apartments');
  }

  cancelRent(apartmentId: number) {
    return this.http.post(this.baseUrlUser + this.user.username + '/cancel-rent/' + apartmentId, {});
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }
}