import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserRequestService } from '../services/request/user-request.service';

export const isAdminGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserRequestService);
  const router = inject(Router);

  if (!userService.isAdmin()) {
    console.log("User is not admin, redirecting to home.");
    router.navigate(['/home']);
    return false;
  }
  return true;
};
