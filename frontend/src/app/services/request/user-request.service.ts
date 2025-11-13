import { Injectable } from '@angular/core';
import { AbstractRequestService } from './abstract-request.service';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';

export interface User {
  name: string,
  email: string,
  role: string,
  isVerified: boolean
}

export interface Try {
  tryId: number;
  quizId: number;
  quizName: string;
  result: number;
  date: string;
}

export interface Tries {
  message: string;
  tries: Try[];
}

@Injectable({
  providedIn: 'root'
})

export class UserRequestService extends AbstractRequestService {
  
  private url = this.baseUrl + '/user';

  public getUser(): Observable<User> {
    return this.request<User>('GET', this.url + '/me');
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



  public getTrys(): Observable<any> {
    return this.request<any>('GET', this.url + '/try');
  }

  public addTry(result: number, quizzId: number): Observable<any> {
    return this.request<any>('POST', this.url + '/try', { result, quizzId });
  }

  public getTries(): Observable<Tries> {
    return this.request<Tries>('GET', this.url + '/show/try');
  }

  public isAdmin(): Observable<boolean> {
    return this.getUser().pipe(
      map(user => user.role === 'ADMIN')
    );
  }

}
