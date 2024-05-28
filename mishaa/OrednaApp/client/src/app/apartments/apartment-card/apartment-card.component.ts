import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Apartment } from 'src/app/_models/apartment';
import { Member } from 'src/app/_models/member';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-apartment-card',
  templateUrl: './apartment-card.component.html',
  styleUrls: ['./apartment-card.component.css']
})
export class ApartmentCardComponent {
  @Input() apartment: Apartment  = {} as Apartment;

  constructor(private apartmentService: ApartmentService, private router: Router) {}

  rentApartment() {
    this.apartmentService.rentApartment(this.apartment.id).subscribe({
      next: () => {
        this.router.navigateByUrl('/rented-apartments');
      }
    });
  }
}
