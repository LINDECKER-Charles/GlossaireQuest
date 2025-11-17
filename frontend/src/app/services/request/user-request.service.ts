import { Injectable } from '@angular/core';
import { AbstractRequestService } from './abstract-request.service';
import { Observable } from 'rxjs/internal/Observable';
import { User } from 'src/app/models/user';
import { Tries, Try } from 'src/app/models/tries';

@Injectable({
  providedIn: 'root'
})

export class UserRequestService extends AbstractRequestService {
  
  private url = this.baseUrl + '/user';

  public getUser(): Observable<User> {
    return this.request<User>('GET', this.url + '/me');
  }


  public getAllUsers(): Observable<User[]> {
    return this.request<User[]>('GET', this.url);
  }

  public getUserByEmail(email: string): Observable<User> {
    return this.request<User>('GET', `${this.url}/${email}`);
  }
  
  public isVerified(): Observable<boolean> {
    return this.request<boolean>('GET', this.url + '/isVerified');
  }

  public patchUserPassword(password: string): Observable<User> {
    return this.request<User>('PATCH', this.url, { "password" : password });
  }

  public patchUserEmail(email: string): Observable<User> {
    return this.request<User>('PATCH', this.url, { "email" : email });
  }

  public patchUserName(name: string): Observable<User> {
    return this.request<User>('PATCH', this.url, { "name" : name });
  }

  public deleteUser(): Observable<void> {
    return this.request<void>('DELETE', this.url);
  }



  public getTrys(amount?: number): Observable<any> {
    const param = amount !== undefined ? `/${amount}` : "";
    return this.request<any>('GET', `${this.url}/try${param}`);
  }


  public addTry(result: number, quizzId: number): Observable<any> {
    return this.request<any>('POST', this.url + '/try', { result, quizzId });
  }

  public getTries(amount?: number, scope?: number, userEmail?: string): Observable<Tries> {
    const param = amount !== undefined ? `/${amount}` : "";
    const scopeParam = scope !== undefined ? `/${scope}` : "";
    const emailParam = userEmail !== undefined ? `/${userEmail}` : "";
    return this.request<Tries>('GET', `${this.url}/show/try${param}${scopeParam}${emailParam}`);
  }

  public isAdmin(): boolean {
    return localStorage.getItem('role') === 'ADMIN';
  }

}
