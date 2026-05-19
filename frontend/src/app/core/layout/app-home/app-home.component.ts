import { Component } from '@angular/core';
import { LanguageButtonComponent } from "../../../shared/components/language-button/language-button.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from "../../../shared/dashboard-card/dashboard-card.component";

@Component({
  selector: 'app-app-home',
  imports: [LanguageButtonComponent, TranslocoModule, MatButtonModule, DashboardCardComponent],
  templateUrl: './app-home.component.html',
  styleUrl: './app-home.component.scss',
})
export class AppHomeComponent {}
