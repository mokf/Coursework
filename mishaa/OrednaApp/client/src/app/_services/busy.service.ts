import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  public busyRequestCount = 0;

  constructor(private spinnerServices: NgxSpinnerService) { }

  busy(): void
  {
    this.busyRequestCount++;
    this.spinnerServices.show(undefined, {
      type: 'line-spin-fade',
      color: '#fff',
      bdColor: 'rgba(0, 0, 0, 0.8)'
    });
  }

  idle()
  {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0)
    {
      this.busyRequestCount = 0;
      this.spinnerServices.hide();
    }
  }
}
