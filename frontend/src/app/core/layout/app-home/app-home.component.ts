import { Component } from '@angular/core';
import { LanguageButtonComponent } from "../../../shared/components/language-button/language-button.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from "../../../shared/dashboard-card/dashboard-card.component";
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-app-home',
  imports: [TranslocoModule,
            MatButtonModule,
            DashboardCardComponent,
            NavbarComponent],
  templateUrl: './app-home.component.html',
  styleUrl: './app-home.component.scss',
})
export class AppHomeComponent {}
