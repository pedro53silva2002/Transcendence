import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../logic/services/auth.service';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-auth-callback',
  imports: [ LoadingSpinnerComponent],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss',
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) { }

  async ngOnInit(): Promise<void> {
    //verifies URL to look for the word 'error'
    const error = this.route.snapshot.queryParamMap.get('error');
    if (error) {
      this.router.navigate(['/'], { queryParams: { authError: error } });
      return;
    }

    //call to BE to verify identity
    try {
      //waits for the BE response without blocking the browser
      await this.authService.loadMe();

      if (this.authService.me()) {
        console.log('Login successful:', this.authService.me());
        this.router.navigate(['/home']);
      } else {
        throw new Error('User not found.');
      }
    } catch (err) {
      console.error('Authentication failed:', err);
      this.router.navigate(['/'], { queryParams: { error: 'session_expired' } });
    }
  }
}
