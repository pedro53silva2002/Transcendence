import { Component, inject, OnInit, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from '../../../shared/dashboard-card/dashboard-card.component';
import { SessionService } from '../../logic/services/session.service';
import { MeDto } from '../../logic/dtos/auth.dto';
import { RouterModule } from '@angular/router';
import { StatCardsComponent } from "../../../shared/stat-cards/stat-cards.component";
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { FooterComponent } from '../../layout/footer/footer.component';

@Component({
  selector: 'app-user-dashboard',
  imports: [TranslocoModule, MatButtonModule, DashboardCardComponent, NavbarComponent, FooterComponent, RouterModule, StatCardsComponent],
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.scss',
})
export class UserDashboardComponent implements OnInit {

  private readonly authService = inject(SessionService);
  public user = signal<MeDto | null>(null);

  ngOnInit(): void {	
      const response = this.authService.me();
      if (response) {
        this.user.set(response);
      }
  }
}
