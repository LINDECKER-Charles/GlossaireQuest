import { Injectable } from '@angular/core';
import { AbstractRequestService } from './abstract-request.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SecurityRequestService extends AbstractRequestService {

  private url = this.baseUrl;

  sendVerificationEmail(): Observable<{ message: string }> {
    return this.request<{ message: string }>('POST', this.url + '/user/send-verify');
  }

  sendPasswordResetEmail(email: string): Observable<{ message: string }> {
    return this.requestNoAuth<{ message: string }>('POST', this.url + '/security/send-reset-password?email='+ encodeURIComponent(email), {});
  }

  changePassword(token: string, newPassword: string): Observable<{ message: string }>{
    return this.requestNoAuth<{ message: string }>('POST', this.url + '/security/reset-password?token=' + encodeURIComponent(token) + '&newPassword=' + encodeURIComponent(newPassword), {});
  }

  verifyEmail(token: string): Observable<{ message: string }> {
    return this.requestNoAuth<{ message: string }>('GET', this.url + '/user/verify?token=' + encodeURIComponent(token), {});
  }


  public getEmailAvailability(email: string): Observable<boolean> {
    return this.request<boolean>('GET', this.url + '/security/email-availability?email=' + encodeURIComponent(email), {});
  }
}
