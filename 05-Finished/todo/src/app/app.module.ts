import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';

// other modules
import { CoreModule } from './core/core.module';
import { TodoModule } from './todo/todo.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatToolbarModule } from '@angular/material/toolbar';

// import components
import { AppComponent } from './app.component';
import { AppNavigationComponent } from './components/app-navigation/app-navigation.component';
import { LoginComponent } from './components/login/login.component';
import { AuthenticatedInterceptor } from './core/services/authenticated.interceptor';
import { AppSettingsService } from './core/services/app-settings.service';

export function appSettingsLoader(appSettingSvc: AppSettingsService) {
  return () => appSettingSvc.loadConfig();
}

@NgModule({
  declarations: [
    AppComponent,
    AppNavigationComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatToolbarModule,
    CoreModule,
    TodoModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
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
