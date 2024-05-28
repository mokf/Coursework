import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './forms/login/login.component';
import { RegisterComponent } from './forms/register/register.component';
import { HomeComponent } from './home/home.component';
import { TextInputComponent } from './forms/text-input/text-input.component';
import { ApartmentDetailComponent } from './apartments/apartment-detail/apartment-detail.component';
import { SharedModule } from './_modules/shared.module';
import { ApartmentListComponent } from './apartments/apartment-list/apartment-list.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { ApartmentCardComponent } from './apartments/apartment-card/apartment-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { ApartmentEditComponent } from './apartments/apartment-edit/apartment-edit.component';
import { PhotoEditorComponent } from './apartments/photo-editor/photo-editor.component';
import { ApartmentDetailEditComponent } from './apartments/apartment-edit/apartment-detail-edit/apartment-detail-edit.component';
import { RentedApartmentsListComponent } from './apartments/rented-apartments-list/rented-apartments-list.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { CreateApartmentComponent } from './apartments/create-apartment/create-apartment.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    TextInputComponent,
    ApartmentDetailComponent,
    ApartmentListComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    ApartmentCardComponent,
    ApartmentEditComponent,
    PhotoEditorComponent,
    ApartmentDetailEditComponent,
    RentedApartmentsListComponent,
    CreateApartmentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
