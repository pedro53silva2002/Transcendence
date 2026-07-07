import { Component, input, ViewEncapsulation } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-close-button',
  standalone: true,
  imports: [MatIconModule,
            MatButtonModule,
            MatDialogModule
  ],
  encapsulation: ViewEncapsulation.None,
  templateUrl: './close-button.component.html',
  styleUrl: './close-button.component.scss',
})
export class CloseButtonComponent {
  //same as @input, but this is the angular 21 way of doing it
  color = input<string>('var(--mat-sys-primary-container)');
}
