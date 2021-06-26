import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate,
  Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class IsLoggedInGuard implements CanActivate {
  constructor(private router: Router,
    private authSvc: AuthenticationService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authSvc.isLoggedIn()) {
      return true;
    } else {
      console.info('Not logged in, redirecting');
      this.router.navigate(['/login']);
      return false;
    }
  }
}
