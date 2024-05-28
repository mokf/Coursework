import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Apartment } from 'src/app/_models/apartment';
import { ApartmentParams } from 'src/app/_models/apartmentParams';
import { Pagination } from 'src/app/_models/pagination';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-apartment-list',
  templateUrl: './apartment-list.component.html',
  styleUrls: ['./apartment-list.component.css']
})
export class ApartmentListComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  //apartments$: Observable<Apartment[]> | undefined;
  apartments: Apartment[] | undefined = [];
  pagination: Pagination | undefined;
  apartmentParams: ApartmentParams | undefined;

  searchText: string | undefined;

  priceList = [{ value: '', display: '' }, { value: 'priceAsc', display: 'Ascending' }, { value: 'priceDesc', display: 'Descending' }];
  peopleList = [{ value: '', display: '' }, { value: 'peopleAsc', display: 'Ascending' }, { value: 'peopleDesc', display: 'Descending' }];
  tvList = [{ value: '', display: '' }, { value: 'yes', display: 'Has TV' }, { value: 'no', display: 'Hasnt TV' }];
  wifiList = [{ value: '', display: '' }, { value: 'yes', display: 'Has Wifi' }, { value: 'no', display: 'Hasnt Wifi' }];
  //isAvailableList = [{value: 'yes', display: 'Available'}, {value: 'no', display: 'Not Available'}];

  constructor(private apartmentService: ApartmentService) {
    this.apartmentParams = this.apartmentService.getApartmentParams();
  }

  ngOnInit(): void {
    this.loadApartments();
  }

  resetFilters() {
    this.apartmentParams = this.apartmentService.resetApartmentParams();
    this.onReset();
    this.loadApartments();
  }

  applyFilters() {
    this.onSearch();
  }

  loadApartments() {
    if (this.apartmentParams) {
      this.apartmentService.setApartmentParams(this.apartmentParams);
      this.apartmentService.getApartments(this.apartmentParams, this.searchText).subscribe({
        next: response => {
          if (response && response.pagination) {
            this.apartments = response.result;
            this.pagination = response.pagination
          }
        }
      })
    }
  }

  pageChanged(event: any) {
    if (this.apartmentParams && this.apartmentParams?.pageSize !== event.page) {
      this.apartmentParams.pageNumber = event.page;
      this.apartmentService.setApartmentParams(this.apartmentParams);
      this.loadApartments();
    }
  }

  onSearch() {
    if (this.searchTerm) {
      this.searchText = this.searchTerm?.nativeElement.value;
    }
    this.loadApartments();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.searchText = '';
  }
}
