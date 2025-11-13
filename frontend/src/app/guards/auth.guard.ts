import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isTokenExpired()) {
    console.log("Token expired or not present, redirecting to login.");
    router.navigate(['/login']);
    return false;
  }
  return true;
};
