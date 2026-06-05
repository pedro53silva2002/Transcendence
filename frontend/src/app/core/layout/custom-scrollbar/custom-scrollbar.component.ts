import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  OnDestroy,
  inject,
  signal,
  viewChild,
} from '@angular/core';

@Component({
  selector: 'app-custom-scrollbar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'custom-scrollbar-host' },
  template: `
    <div #viewport class="scrollbar-viewport" (scroll)="onScroll()">
      <ng-content />
    </div>

    @if (isScrollable()) {
      <div
        class="scrollbar-track"
        role="scrollbar"
        aria-orientation="vertical"
        [attr.aria-valuenow]="scrollPercent()"
        aria-valuemin="0"
        aria-valuemax="100"
        (mousedown)="onTrackClick($event)"
      >
        <div
          #thumb
          class="scrollbar-thumb"
          [style.height.%]="thumbHeight()"
          [style.top.%]="thumbTop()"
          (mousedown)="onThumbDragStart($event)"
        ></div>
      </div>
    }
  `,
  styleUrl: './custom-scrollbar.component.scss',
})
export class CustomScrollbarComponent implements AfterViewInit, OnDestroy {
  private readonly el = inject(ElementRef);

  readonly viewport = viewChild.required<ElementRef<HTMLDivElement>>('viewport');
  readonly thumb = viewChild<ElementRef<HTMLDivElement>>('thumb');

  readonly thumbHeight = signal(100);
  readonly thumbTop = signal(0);
  readonly scrollPercent = signal(0);
  readonly isScrollable = signal(false);

  private dragStartY = 0;
  private dragStartScrollTop = 0;
  private readonly onMouseMove = this.handleMouseMove.bind(this);
  private readonly onMouseUp = this.stopDrag.bind(this);
  private resizeObserver!: ResizeObserver;
  private mutationObserver!: MutationObserver;

  ngAfterViewInit(): void {
    const vp = this.viewport().nativeElement;

    this.resizeObserver = new ResizeObserver(() => this.updateThumb());
    this.resizeObserver.observe(vp);

    this.mutationObserver = new MutationObserver(() => this.updateThumb());
    this.mutationObserver.observe(vp, { childList: true, subtree: true });

    this.updateThumb();
  }

  ngOnDestroy(): void {
    this.resizeObserver.disconnect();
    this.mutationObserver.disconnect();
    document.removeEventListener('mousemove', this.onMouseMove);
    document.removeEventListener('mouseup', this.onMouseUp);
  }

  onScroll(): void {
    this.updateThumb();
  }

  updateThumb(): void {
    const vp = this.viewport().nativeElement;
    const maxScrollTop = vp.scrollHeight - vp.clientHeight;
    this.isScrollable.set(maxScrollTop > 0);

    const ratio = vp.clientHeight / vp.scrollHeight;
    const height = Math.max(ratio * 100, 8);
    const top = maxScrollTop > 0 ? (vp.scrollTop / maxScrollTop) * (100 - height) : 0;

    this.thumbHeight.set(height);
    this.thumbTop.set(top);
    this.scrollPercent.set(Math.round((vp.scrollTop / (maxScrollTop || 1)) * 100));
  }

  onTrackClick(event: MouseEvent): void {
    if (event.target === this.thumb()?.nativeElement) return;
    const track = event.currentTarget as HTMLElement;
    const clickY = event.clientY - track.getBoundingClientRect().top;
    const vp = this.viewport().nativeElement;
    const ratio = clickY / track.clientHeight;
    vp.scrollTop = ratio * (vp.scrollHeight - vp.clientHeight);
  }

  onThumbDragStart(event: MouseEvent): void {
    event.preventDefault();
    this.dragStartY = event.clientY;
    this.dragStartScrollTop = this.viewport().nativeElement.scrollTop;
    document.addEventListener('mousemove', this.onMouseMove);
    document.addEventListener('mouseup', this.onMouseUp);
  }

  private handleMouseMove(event: MouseEvent): void {
    const vp = this.viewport().nativeElement;
    const track = (this.el.nativeElement as HTMLElement).querySelector(
      '.scrollbar-track',
    ) as HTMLElement;
    const delta = event.clientY - this.dragStartY;
    const scrollRatio = delta / track.clientHeight;
    vp.scrollTop =
      this.dragStartScrollTop +
      scrollRatio * (vp.scrollHeight - vp.clientHeight);
  }

  private stopDrag(): void {
    document.removeEventListener('mousemove', this.onMouseMove);
    document.removeEventListener('mouseup', this.onMouseUp);
  }
}
