import {inject} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const isNotLoggedIn = localStorage.getItem('isLoggedIn') === null;

  if (isNotLoggedIn) {
    const router = inject(Router);

    return router.parseUrl('/log-in');
  }

  return true;
};
