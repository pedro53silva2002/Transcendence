import { Component, inject, OnInit, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from '../../../shared/dashboard-card/dashboard-card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { SessionService } from '../../logic/services/session.service';
import { MeDto } from '../../feature/auth/dtos/auth.dto';
import { FooterComponent } from '../footer/footer.component';

@Component({
  selector: 'app-user-dashboard',
  imports: [TranslocoModule, MatButtonModule, DashboardCardComponent, NavbarComponent, FooterComponent],
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.scss',
})
export class UserDashboardComponent implements OnInit {
  private readonly authService = inject(SessionService);
  public user = signal<MeDto | null>(null);

  ngOnInit(): void {
    try {
      const response = this.authService.me();
      if (response) {
        this.user.set(response);
      }
    } catch (error) {
      console.error('Erro ao carregar utilizador:', error);
      this.user.set(null);
    }
  }
}
