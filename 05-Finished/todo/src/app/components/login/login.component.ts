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
