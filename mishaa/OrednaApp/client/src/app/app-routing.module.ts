import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './forms/register/register.component';
import { LoginComponent } from './forms/login/login.component';
import { ApartmentDetailComponent } from './apartments/apartment-detail/apartment-detail.component';
import { AuthGuard } from './_guards/auth.guard';
import { ApartmentListComponent } from './apartments/apartment-list/apartment-list.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { ApartmentEditComponent } from './apartments/apartment-edit/apartment-edit.component';
import { ApartmentDetailEditComponent } from './apartments/apartment-edit/apartment-detail-edit/apartment-detail-edit.component';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { RentedApartmentsListComponent } from './apartments/rented-apartments-list/rented-apartments-list.component';
import { CreateApartmentComponent } from './apartments/create-apartment/create-apartment.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  {
    path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard], children: [
      { path: 'rented-apartments', component: RentedApartmentsListComponent },
      { path: 'apartments', component: ApartmentListComponent },
      { path: 'apartments/:id', component: ApartmentDetailComponent },
      { path: 'profile/edit', component: ApartmentEditComponent },
      { path: 'profile/edit/:id', component: ApartmentDetailEditComponent, canDeactivate: [PreventUnsavedChangesGuard] },
      { path: 'profile/create-apartment', component: CreateApartmentComponent },
    ]
  },
  { path: 'home', component: HomeComponent },
  { path: 'errors', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
