import { Component, Input } from '@angular/core';
import { ActivatedRoute, RouterLinkActive } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Apartment } from 'src/app/_models/apartment';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ApartmentService } from 'src/app/_services/apartment.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent {
  @Input() apartment: Apartment = {} as Apartment;
  public apartmentId: number = {} as number;
  public uploader: FileUploader | undefined;
  public hasBaseDropZoneOver: boolean = false;

  public user: User = {} as User;

  private baseUrl: string = environment.apiUrl;

  constructor(private accountService: AccountService, private apartmentService: ApartmentService, private route: ActivatedRoute) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    })
  }

  ngOnInit(): void {
    if (!this.user) return;

    var id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.initializeUploader(Number.parseInt(id));
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo): void {
    if (!this.user) return;
    this.apartmentService.setMainPhoto(photo.id, this.apartment.id).subscribe({
      next: () => {
        if (this.user && this.apartment) {
          this.apartment.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
          this.apartment.photos.forEach(p => {
            if (p.isMain) p.isMain = false;
            if (p.id === photo.id) p.isMain = true;
          })
        }
      }
    })
  }

  deletePhoto(photo: Photo): void {
    if (!this.user) return;
    let photoId = photo.id;
    this.apartmentService.deletePhoto(photo.id, this.apartment.id).subscribe({
      next: () => {
        if (this.apartment) {
          this.apartment.photos = this.apartment.photos.filter(photo => photo.id != photoId);
        }
      }
    })
  }

  initializeUploader(apartmentId: number): void {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.user.username + '/' + apartmentId + '/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    })

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }


    this.uploader.onSuccessItem = (item, response, staus, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.apartment?.photos.push(photo);
        if (photo.isMain && this.user && this.apartment) {
          this.apartment.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    }
  }
}
