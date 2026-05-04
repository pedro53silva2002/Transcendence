import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-input-label',
  imports: [MatFormFieldModule,
            MatInputModule,
            MatIconModule,
            ReactiveFormsModule
  ],
  templateUrl: './input-label.component.html',
  styleUrl: './input-label.component.scss',
})
export class InputLabelComponent {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() icon?: string;
  @Input() type: 'text' | 'password' | 'email' | 'number' = 'text';
  @Input() control: FormControl = new FormControl();
}
