import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { TranslocoModule } from '@jsverse/transloco';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-auth-navbar',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, TranslocoModule],
  templateUrl: './auth-navbar.component.html',
  styleUrl: './auth-navbar.component.scss',
})
export class AuthNavbarComponent {
  constructor(private dialog: MatDialog) {}

  openLogin(): void {
    this.dialog.open(LoginComponent, {});
  }
  openRegister(): void {
    this.dialog.open(RegisterComponent, {
      panelClass: 'register-dialog',
    });
  }
}
