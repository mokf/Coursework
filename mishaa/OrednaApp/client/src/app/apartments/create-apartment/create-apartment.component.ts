import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ApartmentService } from 'src/app/_services/apartment.service';

@Component({
  selector: 'app-create-apartment',
  templateUrl: './create-apartment.component.html',
  styleUrls: ['./create-apartment.component.css']
})
export class CreateApartmentComponent implements OnInit {
  creatingForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  private user: User = {} as User;

  constructor(private formBuilder: FormBuilder, private apartmentService: ApartmentService, private accountService: AccountService, private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user;
        }
      }
    })
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    if (!this.user) return;
    this.creatingForm = this.formBuilder.group({
      title: new FormControl('', [Validators.required, Validators.maxLength(120)]),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      price: new FormControl(10, [Validators.required, Validators.minLength(2)]),
      maxPeople: new FormControl(1, [Validators.required, Validators.maxLength(2)]),
      hasTV: new FormControl(true, Validators.required),
      hasWifi: new FormControl(true, Validators.required),
    });
  }

  create() {
    if (!this.user) return;
    this.apartmentService.createApartment(this.creatingForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/apartments');
      }
    })
  }
}
