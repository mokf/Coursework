import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Apartment } from 'src/app/_models/apartment';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-apartment-detail-edit',
  templateUrl: './apartment-detail-edit.component.html',
  styleUrls: ['./apartment-detail-edit.component.css']
})
export class ApartmentDetailEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  };
  public apartment: Apartment = {} as Apartment;
  private user: User = {} as User;

  constructor(private accountService: AccountService, private toastr: ToastrService, private apartmentService: ApartmentService, private route: ActivatedRoute) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user;
        }
      }
    })
  }

  ngOnInit(): void {
    this.loadApartment();
  }

  loadApartment() {
    var id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.apartmentService.getApartment(Number.parseInt(id)).subscribe({
      next: apartment => {
        if (apartment) {
          this.apartment = apartment;
        }
      }
    });
  }

  updateApartment() {
    if (!this.user) return;
    this.apartmentService.updateApartment(this.apartment.id, this.editForm?.value).subscribe({
      next: () => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.apartment);
      }
    });
  }
}
