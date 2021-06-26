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

@Injectable({
  providedIn: 'root'
})
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

  private loginUser(data: LoginResponse): void {
    sessionStorage.setItem(this.TOKEN_NAME, data.accessToken);
    // todo: extract user from jwt
  }
}

```
### Slide 15/16
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

  public model!: LoginRequest;
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
### Slide 17
app-routing.module.ts
```
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  // otherwise redirect to todo
  { path: '**', redirectTo: '/todo' }
];
```
### Slide 18/19
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
  public currentUser?: User;
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
### Slide 20
authentication.service.ts
```
  public logoff() {
    sessionStorage.clear();
    this.currentUser = undefined;
    this.router.navigate(['/login']);
  }
```
### Slide 21
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
### Slide 24
is-logged-in.guard.ts
```
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
```
### Slide 25
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
authenticated.interceptor.ts
```
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable()
export class AuthenticatedInterceptor implements HttpInterceptor {
  constructor(private authSvc: AuthenticationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this.authSvc.isLoggedIn()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.authSvc.getLoginToken()}`
        }
      });
    }
    return next.handle(request);
  }
}
```
### Slide 29
app.module.ts
```
  providers: [
    { provide: HTTP_INTERCEPTORS,
      useClass: AuthenticatedInterceptor,
      multi: true },
  ],
```
### Slide 32
assets\app-settings.json
```
{
  "environment": "dev",
  "apiUrl": "http://localhost:8080"
}
```
### Slide 33
app-settings.service.ts
```
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';

import { AppSettings } from '../models/app-settings.model';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  constructor(private httpClient: HttpClient) {
  }

  private appSettings!: AppSettings;
  get settings(): AppSettings {
    return this.appSettings;
  }

  public loadConfig(): Promise<AppSettings> {
    return this.httpClient.get<AppSettings>('./assets/app-settings.json')
      .pipe(
        tap((config) => {
          this.appSettings = config;
        })
      )
      .toPromise();
  }
}
```
### Slide 34
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

