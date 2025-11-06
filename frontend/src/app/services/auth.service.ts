import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl + '/auth';

  private loggedIn$ = new BehaviorSubject<boolean>(this.hasValidToken());

  constructor(private http: HttpClient) {}

  // === AUTH ===

  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap((response: any) => {
          if (response.token) {
            localStorage.setItem('token', response.token);
            localStorage.setItem('email', email);
            this.loggedIn$.next(true); // notifie connexion
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
            this.loggedIn$.next(true);
          }
        })
      );
  }

  public logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    this.loggedIn$.next(false); 
  }

  // === OBSERVABLES ===

  /** Observable public pour écouter les changements d’état */
  public get isLoggedIn$(): Observable<boolean> {
    return this.loggedIn$.asObservable();
  }

  /** Vérifie si un token valide existe */
  public isLoggedIn(): boolean {
    const valid = this.hasValidToken();
    this.loggedIn$.next(valid); // synchronise le BehaviorSubject
    return valid;
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

  // === PRIVATE HELPERS ===

  private hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;
    if (this.isTokenExpired()) {
      this.logout();
      return false;
    }
    return true;
  }
}
