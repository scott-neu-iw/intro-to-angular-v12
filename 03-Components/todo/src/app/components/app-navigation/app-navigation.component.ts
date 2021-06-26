import { Component, OnInit } from '@angular/core';
import { AppSettings } from '../../core/models/app-settings.model';
import { AppSettingsService } from '../../core/services/app-settings.service';

@Component({
  selector: 'app-app-navigation',
  templateUrl: './app-navigation.component.html',
  styleUrls: ['./app-navigation.component.scss']
})
export class AppNavigationComponent implements OnInit {

  constructor(private appSettingsSvc: AppSettingsService) {
    this.settings = appSettingsSvc.settings;
  }

  public settings: AppSettings;

  ngOnInit(): void {
  }
}
