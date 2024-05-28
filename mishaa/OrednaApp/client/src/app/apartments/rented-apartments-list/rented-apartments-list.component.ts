import { Component } from '@angular/core';
import { Apartment } from 'src/app/_models/apartment';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-rented-apartments-list',
  templateUrl: './rented-apartments-list.component.html',
  styleUrls: ['./rented-apartments-list.component.css']
})
export class RentedApartmentsListComponent {
  apartments: Apartment[] = [];

  constructor(private apartmentService: ApartmentService) {}

  ngOnInit(): void {
    this.loadApartments();
  }

  loadApartments() {
    this.apartmentService.getRentedApartment().subscribe({
      next: response => {
        if (response) {
          this.apartments = response;
        }
      }
    })
  }
}
