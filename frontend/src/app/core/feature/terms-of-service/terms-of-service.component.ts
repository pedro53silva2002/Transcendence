import { Component, computed, inject } from '@angular/core';
import { SessionService } from '../../logic/services/session.service';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { TranslocoModule } from '@jsverse/transloco';
import { FooterComponent } from "../../layout/footer/footer.component";

@Component({
  selector: 'app-terms-of-service',
  imports: [NavbarComponent, TranslocoModule, FooterComponent],
  templateUrl: './terms-of-service.component.html',
  styleUrl: './terms-of-service.component.scss',
})
export class TermsOfServiceComponent {
	private readonly sessionService = inject(SessionService);

	protected isAuthenticated = computed(() => !!this.sessionService.me());
}
