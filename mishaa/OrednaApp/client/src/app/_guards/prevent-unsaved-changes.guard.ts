import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { ApartmentDetailEditComponent } from '../apartments/apartment-edit/apartment-detail-edit/apartment-detail-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<ApartmentDetailEditComponent> {
  canDeactivate(component: ApartmentDetailEditComponent): boolean {
      if (component.editForm?.dirty) {
        return confirm('Are you sure you want to continues? Any unsaved changes will be lost');
      }
      return true;
  }
  
}
