# Workshop 4 - Links and code snippets
### Slide 11
login-request.model.ts
```
export interface LoginRequest {
  username: string;
  password: string;
}
```
login-response.model.ts
```
export interface LoginResponse {
  accessToken: string;
  expires: number;
}
```
user.model.ts
```
export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: Array<string>;
}
```
### Slide 12
authentication.service.ts
```
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';

import { AppSettingsService } from './app-settings.service';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';

@Injectable()
export class AuthenticationService {
  constructor(private httpClient: HttpClient, private appSettingsSvc: AppSettingsService) { }

  private TOKEN_NAME = 'accessToken';

  public login(loginRequest: LoginRequest): Observable<boolean> {
    const url = `${this.appSettingsSvc.settings.apiUrl}/v2/authentication/login`;

    return this.httpClient.post<LoginResponse>(url, loginRequest)
    .pipe(
      tap(data => this.loginUser(data)),
      map(() => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  loginUser(data: LoginResponse): void {
    sessionStorage.setItem(this.TOKEN_NAME, data.accessToken);
    // todo: extract user from jwt
  }
}
```
### Slide 14/15
login.component.ts
```
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoginRequest } from '../../core/models/login-request.model';
import { AuthenticationService } from '../../core/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor(private router: Router, private authService: AuthenticationService) { }

  public model: LoginRequest;
  public showBadLogin = false;
  public isLoggingIn = false;

  ngOnInit() {
    this.model = { username: '', password: '' };
  }

  public login() {
    this.showBadLogin = false;
    this.isLoggingIn = true;

    this.authService.login(this.model).subscribe(result => {
      this.isLoggingIn = false;
      if (result === true) {
        this.router.navigate(['/todo']);
      } else {
        this.showBadLogin = true;
      }
    });
  }
}
```
login.component.html
```
<form (ngSubmit)="login()" #loginForm="ngForm">
  <div class="input-container">
    <mat-form-field>
      <input matInput placeholder="Username"
        id="username" name="username" [(ngModel)]="model.username"
        required [disabled]="isLoggingIn">
    </mat-form-field>
    <mat-form-field>
      <input type="password" matInput placeholder="Password"
        id="password" name="password" [(ngModel)]="model.password"
        required [disabled]="isLoggingIn">
    </mat-form-field>
  </div>
  <div *ngIf="showBadLogin">
    * Invalid username or password
  </div>
  <div>
    <button mat-stroked-button type="submit" color="primary"
      [disabled]="!loginForm.form.valid || isLoggingIn">Login</button>
  </div>
</form>
```
login.component.scss
```
.input-container {
  display: flex;
  flex-direction: column;
  width: 400px;
}

.input-container > * {
  width: 100%;
}
```
### Slide 16
app-routing.module.ts
```
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  // otherwise redirect to todo
  { path: '**', redirectTo: '/todo' }
];
```
### Slide 17/18
authentication.service.ts
```
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

import { AppSettingsService } from './app-settings.service';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { User } from '../models/user.model';

@Injectable()
export class AuthenticationService {
  constructor(private httpClient: HttpClient, private appSettingsSvc: AppSettingsService) {
                this.jwtHelper = new JwtHelperService();
              }

  private TOKEN_NAME = 'accessToken';

  public currentUser: User;
  private jwtHelper: JwtHelperService;

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

  public isLoggedIn(): boolean {
    const token = this.getLoginToken();
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  public getLoginToken(): string {
    return sessionStorage.getItem(this.TOKEN_NAME);
  }

  private loginUser(data: LoginResponse): void {
    sessionStorage.setItem(this.TOKEN_NAME, data.accessToken);
    this.extractUser();
  }

  private extractUser() {
    const decodedToken = this.jwtHelper.decodeToken(this.getLoginToken());
    const user: User = {
      id: decodedToken.sub,
      email: decodedToken.email,
      firstName: decodedToken.given_name,
      lastName: decodedToken.family_name,
      roles: decodedToken.user_authorization.split(',')
    };
    this.currentUser = user;
  }
}
```
### Slide 19
authentication.service.ts
```
  public logoff() {
    sessionStorage.clear();
    this.currentUser = null;
    this.router.navigate(['/login']);
  }
```
### Slide 20
app-navigation.component.html
```
    <!-- This fills the remaining space of the current row -->
    <span class="fill-remaining-space"></span>

    <a (click)="logoff()" href="javascript:void(0);">Logoff</a>

```
app-navigation.component.scss
```
.fill-remaining-space {
  /* This fills the remaining space, by using flexbox.
     Every toolbar row uses a flexbox row layout. */
  flex: 1 1 auto;
}
```
app-navigation.component.ts
```
  public logoff() {
    this.authService.logoff();
  }
  ```
### Slide 23
is-logged-in.guard.ts
```
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot,
         RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class IsLoggedInGuard implements CanActivate {
  constructor(private router: Router,
              private authSvc: AuthenticationService) { }

  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): boolean {
    if (this.authSvc.isLoggedIn()) {
      return true;
    } else {
      // tslint:disable-next-line:no-console
      console.info('Not logged in, redirecting');
      this.router.navigate(['/login']);
      return false;
    }
  }
}
```
### Slide 24
todo-routing.module.ts
```
const routes: Routes = [
  {
    path: 'todo',
    children: [
      {
        path: '',
        component: TodoListComponent,
        canActivate: [IsLoggedInGuard]
      },
      {
        path: ':id',
        component: TodoItemComponent,
        canActivate: [IsLoggedInGuard]
      }
    ]
  }
];

```
### Slide 28
app-settings.model.ts
```
export interface AppSettings {
  environment: string;
  apiUrl: string;
  jwtAttachDomains: Array<string | RegExp>;
  jwtIgnoreRoutes: Array<string | RegExp>;
}
```
app-settings.service.ts
```
  constructor() {
    this.appSettings = {
      environment: 'dev',
      apiUrl: 'http://localhost:8080',
      jwtAttachDomains: ['localhost:8080'],
      jwtIgnoreRoutes: ['/v2/authentication/login']
    };
  }
```
### Slide 29/30
authenticated.interceptor.ts
```
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppSettingsService } from './app-settings.service';
import { AuthenticationService } from './authentication.service';
import { parse, UrlWithParsedQuery } from 'url';

@Injectable()
export class AuthenticatedInterceptor implements HttpInterceptor {
  constructor(private appSettingsSvc: AppSettingsService, private authSvc: AuthenticationService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.appSettingsSvc.settings) {
      const requestUrl = parse(request.url, true, true);

      if (this.isWhitelistedDomain(requestUrl, this.appSettingsSvc.settings.jwtAttachDomains)
        && !this.isBlacklistedRoute(requestUrl, this.appSettingsSvc.settings.jwtIgnoreRoutes)) {

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${this.authSvc.getLoginToken()}`
          }
        });
      }
    }

    return next.handle(request);
  }

  // https://github.com/auth0/angular2-jwt/blob/master/src/jwt.interceptor.ts

  /**
   * Determine if the domain is whitelisted
   * @param requestUrl the request
   * @param whitelistedDomains whitelisted domains
   */
  private isWhitelistedDomain(requestUrl: UrlWithParsedQuery, whitelistedDomains: Array<string | RegExp>): boolean {
    return (
      requestUrl.host === null ||
      whitelistedDomains.findIndex(
        domain =>
          typeof domain === 'string'
            ? domain === requestUrl.host
            : domain instanceof RegExp
              ? domain.test(requestUrl.host)
              : false
      ) > -1
    );
  }

  /**
   * Determines if the route is backlisted
   * @param requestUrl the request
   * @param blacklistedRoutes blacklisted domains
   */
  private isBlacklistedRoute(requestUrl: UrlWithParsedQuery, blacklistedRoutes: Array<string | RegExp>): boolean {
    return (
      blacklistedRoutes.findIndex(
        route =>
          typeof route === 'string'
            ? route === requestUrl.pathname
            : route instanceof RegExp
              ? route.test(requestUrl.pathname)
              : false
      ) > -1
    );
  }
}

```
### Slide 31
app.module.ts
```
  providers: [
    { provide: HTTP_INTERCEPTORS,
      useClass: AuthenticatedInterceptor,
      multi: true },
  ],
```
### Slide 34
assets\app-settings.json
```
{
  "environment": "dev",
  "apiUrl": "http://localhost:8080",
  "jwtAttachDomains": ["localhost:8080"],
  "jwtIgnoreRoutes": ["/v2/authentication/login"]
}
```
### Slide 35
app-settings.service.ts
```
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { AppSettings } from '../models/app-settings.model';
import { tap } from 'rxjs/operators';

@Injectable()
export class AppSettingsService {
  constructor(private http: HttpClient) { }

  private appSettings: AppSettings;
  public get settings(): AppSettings {
    return this.appSettings;
  }

  public loadConfig(): Promise<AppSettings> {
    return this.http.get<AppSettings>('./assets/app-settings.json')
      .pipe(
        tap((config) => {
          this.appSettings = config;
        })
      )
      .toPromise();
  }
}
```
### Slide 36
app.module.ts
```
export function appSettingsLoader(appSettingSvc: AppSettingsService) {
  return () => appSettingSvc.loadConfig();
}
```
```
  providers: [
    { provide: APP_INITIALIZER,
      useFactory: appSettingsLoader,
      deps: [AppSettingsService],
      multi: true },
    { provide: HTTP_INTERCEPTORS,
      useClass: AuthenticatedInterceptor,
      multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

```

