import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';

import { AppSettingsService } from './app-settings.service';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { User } from '../models/user.model';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private httpClient: HttpClient, private appSettingsSvc: AppSettingsService,
    private router: Router) {
    this.jwtHelper = new JwtHelperService();
  }

  private TOKEN_NAME = 'accessToken';
  public currentUser?: User;
  public jwtHelper: JwtHelperService;

  public login(loginRequest: LoginRequest): Observable<boolean> {
    const url = `${this.appSettingsSvc.settings.apiUrl}/v2/authentication/login`;

    return this.httpClient.post<LoginResponse>(url, loginRequest)
    .pipe(
      tap(data => this.loginUser(data)),
      map(() => this.isLoggedIn()),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public getLoginToken(): string | null {
    return sessionStorage.getItem(this.TOKEN_NAME);
  }

  public isLoggedIn(): boolean {
    const token = this.getLoginToken();
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  public logoff(): void {
    sessionStorage.clear();
    this.currentUser = undefined;
    this.router.navigate(['/login']);
  }

  private loginUser(data: LoginResponse): void {
    sessionStorage.setItem(this.TOKEN_NAME, data.accessToken);
    this.extractUser();
  }

  private extractUser() {
    const token = this.getLoginToken();
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      const user: User = {
        id: decodedToken.sub,
        email: decodedToken.email,
        firstName: decodedToken.given_name,
        lastName: decodedToken.family_name,
        roles: decodedToken.user_authorization.split(',')
      };
      this.currentUser = user;
    } else {
      this.currentUser = undefined;
    }
  }
}
