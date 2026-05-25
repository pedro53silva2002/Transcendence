import { computed, Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {

	//signal that keeps track of active http requests
	private activeRequests = signal<number>(0);

	//public signal that is set to true if http requests count it > 0
	//we use computed to keep the previous variable private
	public isLoading = computed (() => this.activeRequests() > 0);

	public show(): void {
		this.activeRequests.update(count => count + 1);
	}

	public hide(): void {
		//Math.max returns the max value between those 2, so it prevents the count from being negative
		this.activeRequests.update(count => Math.max(0, count - 1));
	}
}
