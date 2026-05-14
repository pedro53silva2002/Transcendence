import { Component, ViewEncapsulation } from '@angular/core';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { environment } from '../../../../environments/environment';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
	GoogleAuthButtonComponent
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  encapsulation: ViewEncapsulation.None, //permite alterar o css de componentes do angular materials sem usar ng-deep
})
export class LoginComponent {
}
