import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const AuthGuard: CanActivateFn = (route, state) => { 
  const token = localStorage.getItem('jwtToken'); 

  if (token) {
    return true; 
  }

  const router = inject(Router); 
  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
