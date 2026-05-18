import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LanguageButtonComponent } from "../../../shared/components/language-button/language-button.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from "../../../shared/dashboard-card/dashboard-card.component";

@Component({
  selector: 'app-app-home',
  imports: [RouterOutlet, LanguageButtonComponent, TranslocoModule, MatButtonModule, DashboardCardComponent],
  templateUrl: './app-home.component.html',
  styleUrl: './app-home.component.scss',
})
export class AppHomeComponent {}
