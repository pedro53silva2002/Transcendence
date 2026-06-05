import { ChangeDetectionStrategy, Component, computed, ElementRef, signal, viewChild } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';

interface ItineraryItem {
  id: number;
  title: string;
  sub: string;
  cost: string;
}

let _nextId = 100;
const nextId = () => ++_nextId;

const SEED: Record<number, ItineraryItem[]> = {
  1: [
    { id: 1, title: 'Museum', sub: 'Louvre', cost: '50' },
    { id: 2, title: 'Lunch', sub: 'Le Comptoir', cost: '35' },
    { id: 3, title: 'Cruise', sub: 'Seine River', cost: '28' },
  ],
  2: [
    { id: 4, title: 'Museum', sub: 'Louvre', cost: '50' },
    { id: 5, title: 'Museum', sub: 'Louvre', cost: '50' },
  ],
};

@Component({
  selector: 'app-itinerary',
  imports: [NavbarComponent],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ItineraryComponent {
  readonly showForm = signal(false);
  readonly currentDay = signal(2);
  readonly days = signal<Record<number, ItineraryItem[]>>(structuredClone(SEED));
  readonly titleValue = signal('');
  readonly costValue = signal('');
  readonly infoValue = signal('');

  readonly items = computed(() => this.days()[this.currentDay()] ?? []);
  readonly canSave = computed(() => this.titleValue().trim().length > 0);
  readonly prevDisabled = computed(() => this.currentDay() <= 1);

  readonly listRef = viewChild<ElementRef<HTMLUListElement>>('listRef');

  toggleForm(): void {
    this.showForm.update(v => !v);
  }

  saveItem(): void {
    const title = this.titleValue().trim();
    if (!title) return;
    const item: ItineraryItem = {
      id: nextId(),
      title,
      sub: this.infoValue().trim() || 'Added stop',
      cost: this.costValue().trim() || '0',
    };
    const day = this.currentDay();
    this.days.update(d => ({ ...d, [day]: [...(d[day] ?? []), item] }));
    this.titleValue.set('');
    this.costValue.set('');
    this.infoValue.set('');
    setTimeout(() => {
      const el = this.listRef()?.nativeElement;
      if (el) el.scrollTop = el.scrollHeight;
    });
  }

  removeItem(id: number): void {
    const day = this.currentDay();
    this.days.update(d => ({ ...d, [day]: d[day].filter(it => it.id !== id) }));
  }

  changeDay(delta: number): void {
    const next = this.currentDay() + delta;
    if (next < 1) return;
    this.currentDay.set(next);
    this.days.update(d => (d[next] ? d : { ...d, [next]: [] }));
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter') this.saveItem();
  }

  filterCost(event: Event): void {
    const input = event.target as HTMLInputElement;
    const filtered = input.value.replace(/[^\d.]/g, '');
    if (filtered !== input.value) input.value = filtered;
    this.costValue.set(filtered);
  }
}
