import { Injectable } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const NoAuthGuard: CanActivateFn = (route, state) => {
  
  const authService = inject(AuthService);
  const router = inject(Router);

  // Si l'utilisateur a un token valide, il est déjà connecté
  if (!authService.isTokenExpired()) {
    console.log("User already authenticated, redirecting to home.");
    router.navigate(['/home']);
    return false; 
  }

  return true;
};
