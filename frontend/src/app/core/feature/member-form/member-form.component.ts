import { UserDto } from './../auth/dtos/user.dto';
import { NgOptimizedImage } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { TranslocoModule } from '@jsverse/transloco';
import { UserService } from '../auth/services/user.service';

@Component({
  selector: 'app-member-form',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    NgOptimizedImage,
    TranslocoModule,
  ],
  templateUrl: './member-form.component.html',
  styleUrl: './member-form.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberFormComponent implements OnInit {
  protected readonly loading = signal(false);
  protected readonly showFriendsOnly = signal(false);

  readonly members = new FormArray<
    FormGroup<{
      userId: FormControl<number>;
      role: FormControl<'ADMIN' | 'MEMBER'>;
    }>
  >([]);
}
