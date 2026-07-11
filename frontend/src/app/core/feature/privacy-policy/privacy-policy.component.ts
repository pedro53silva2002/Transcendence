import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { SessionService } from '../../logic/services/session.service';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { TranslocoModule } from '@jsverse/transloco';
import { FooterComponent } from "../../layout/footer/footer.component";

@Component({
	selector: 'app-privacy-policy',
	imports: [NavbarComponent, TranslocoModule, FooterComponent],
	templateUrl: './privacy-policy.component.html',
	styleUrl: './privacy-policy.component.scss',
})
export class PrivacyPolicyComponent {

	private readonly sessionService = inject(SessionService);

	protected isAuthenticated = computed(() => !!this.sessionService.me());

}
