import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-utrip-bubble',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoModule],
  templateUrl: './utrip-bubble.component.html',
  styleUrl: './utrip-bubble.component.scss',
})
export class UtripBubbleComponent {}
