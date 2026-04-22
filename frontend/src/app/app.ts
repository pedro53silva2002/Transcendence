import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UiButton } from './common/components/ui-button/ui-button';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, UiButton],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}
