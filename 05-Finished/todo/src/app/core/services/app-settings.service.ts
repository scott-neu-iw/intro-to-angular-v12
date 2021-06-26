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
