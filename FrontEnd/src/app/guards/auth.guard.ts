import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { getAuthToken } from '../helpers/storage-helper';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor( private router: Router) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      if (getAuthToken() != null) {
        return true;
      }
      this.router.navigate(['/landing-page'], { queryParams: { returnUrl: state.url }});
      return false;
  }
}
