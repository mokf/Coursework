import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Apartment } from 'src/app/_models/apartment';
import { Photo } from 'src/app/_models/photo';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-apartment-detail',
  templateUrl: './apartment-detail.component.html',
  styleUrls: ['./apartment-detail.component.css']
})
export class ApartmentDetailComponent implements OnInit {
  public apartment: Apartment = {} as Apartment;
  public photos: Photo[] = [];
  public galleryOptions: NgxGalleryOptions[] = [];
  public galleryImages: NgxGalleryImage[] = [];

  constructor(private apartmentService: ApartmentService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.loadApartment();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      }
    ]

    //this.galleryImages = this.getImages();
  }

  loadApartment() {
    var id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.apartmentService.getApartment(Number.parseInt(id)).subscribe({
      next: apartment => {
        if (apartment) {
          this.apartment = apartment;
          this.photos = apartment.photos;
          this.galleryImages = this.getImages();
        }
      }
    });
  }

  getImages() {
    if (!this.apartment || this.apartment == null)
      return [];

    const imageUrls = [];
    for (const photo of this.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      });
    }

    return imageUrls;
  }

  cancelRentApartment() {
    this.apartmentService.cancelRent(this.apartment.id).subscribe({
      next: () => this.router.navigateByUrl('/rented-apartments')
    });
  }

  rentApartment() {
    this.apartmentService.rentApartment(this.apartment.id).subscribe({
      next: () => this.router.navigateByUrl('/rented-apartments')
    });
  }
}
