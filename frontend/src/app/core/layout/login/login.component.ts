import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatDialogModule,
			MatFormFieldModule,
			MatInputModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  encapsulation: ViewEncapsulation.None, //permite alterar o css de componentes do angular materials sem usar ng-deep
})
export class LoginComponent {}
