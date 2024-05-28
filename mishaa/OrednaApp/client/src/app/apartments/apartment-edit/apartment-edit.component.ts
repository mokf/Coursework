import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { Apartment } from 'src/app/_models/apartment';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ApartmentService } from 'src/app/_services/apartment.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-apartment-edit',
  templateUrl: './apartment-edit.component.html',
  styleUrls: ['./apartment-edit.component.css']
})
export class ApartmentEditComponent implements OnInit {
  user: User = {} as User;
  member: Member = {} as Member;
  apartments: Apartment[] = [];

  constructor(private accountService: AccountService, private memberService: MembersService, private apartmentService: ApartmentService, private router: Router) {
    this.loadUser();
  }

  ngOnInit(): void {
    this.loadMember();
    this.loadApartments();
  }

  loadUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user)
          this.user = user;
      }
    })
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => {
        if (member) {
          this.member = member;
        }
      }
    })
  }

  loadApartments() {
    if (!this.user || !this.member) return;
    this.memberService.getApartments(this.user.username).subscribe({
      next: apartmnents => {
        if (apartmnents) {
          this.apartments = apartmnents;
        }
      }
    });
  }

  deleteApartment(apartmentId: number) {
    this.apartmentService.deleteApartment(apartmentId).subscribe({
      next: () => {
        this.router.navigateByUrl('/apartments');
      }
    });
    //const apartment = this.apartments.find(x => x.id == apartmentId);
    this.apartments.splice(apartmentId);
  }
}
