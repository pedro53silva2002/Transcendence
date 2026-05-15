import { ChangeDetectorRef, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslocoModule } from '@jsverse/transloco';
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-auth-navbar',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, TranslocoModule],
  templateUrl: './auth-navbar.component.html',
  styleUrl: './auth-navbar.component.scss',
})
export class AuthNavbarComponent {
  constructor(
    private readonly dialog: MatDialog,
    private readonly cd: ChangeDetectorRef,
  ) {}

  isPopupOpen = false;

  openLogin(): void {
    const loginPopup = this.dialog.open(LoginComponent, {});

    this.isPopupOpen = true; //to activate the css of the button while the popup is open

    loginPopup.afterClosed().subscribe(() => {
      this.isPopupOpen = false; //to deactivate the css of the button when the popup is closed
      this.cd.detectChanges(); //to update the view after changing the isPopupOpen variable
    });
  }
  openRegister(): void {
    this.dialog.open(RegisterComponent, {
      panelClass: 'register-dialog',
    });
  }
}
