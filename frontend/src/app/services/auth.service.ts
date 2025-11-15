import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, interval, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl + '/auth';
  public loggedIn$ = new BehaviorSubject<boolean>(!this.isTokenExpired());
  public isAdmin$ = new BehaviorSubject<boolean>(this.isAdmin(localStorage.getItem('role') || ''));

  constructor(private readonly http: HttpClient, private readonly router: Router) {
    interval(10_000).subscribe(() => {
      if(this.isTokenExpired()) {
        this.loggedIn$.next(false);
        this.isAdmin$.next(false);
      }
    });

  }

  // === AUTH ===
  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap((response: any) => {
          if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('email', email);
            localStorage.setItem('role', response.role);

            this.loggedIn$.next(true);
            this.isAdmin$.next(this.isAdmin(response.role));
          }
        })
      );
  }

  public register(email: string, password: string, name: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { email, password, name })
      .pipe(
        tap((response: any) => {
          if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('email', email);
            localStorage.setItem('role', response.role);

            this.loggedIn$.next(true);
            this.isAdmin$.next(this.isAdmin(response.role));
          }
        })
      );
  }

  private isAdmin(role : string): boolean {
    return role === 'ADMIN';
  }

  private getLocalUser(){
    return {
      email: localStorage.getItem('email'),
      role: localStorage.getItem('role'),
      token: localStorage.getItem('token')
    }
  }

  public logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    this.loggedIn$.next(false);
    this.isAdmin$.next(false);
    this.router.navigate(['/login']);
  }

  // === TOKEN ===

  public getToken(): string | null {
    return localStorage.getItem('token');
  }

  public isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const now = Math.floor(Date.now() / 1000);
      return payload.exp < now; // true = expiré
    } catch (e) {
      console.error('Erreur de décodage du token :', e);
      return true;
    }
  }
}
