import { Component } from '@angular/core';
import { AuthNavbarComponent } from '../auth-navbar/auth-navbar.component';
import { NavbarComponent } from "../navbar/navbar.component";

@Component({
  selector: 'app-landing-page',
  imports: [AuthNavbarComponent, NavbarComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss',
})
export class LandingPageComponent {}
