import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { TranslocoService } from '@jsverse/transloco';

@Component({
  selector: 'app-language-button',
  imports: [MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './language-button.component.html',
  styleUrl: './language-button.component.scss',
})
export class LanguageButtonComponent {

	constructor(private translocoService: TranslocoService) {}

	changeLanguage(lang: string) {
		//calls the transloco service, checks the JSON file in public/i18n and loads the page
		this.translocoService.setActiveLang(lang);
	}
}
