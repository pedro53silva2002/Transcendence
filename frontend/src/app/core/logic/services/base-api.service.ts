import { environment } from '../../../../environments/environment';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

/**
 * Standard API response wrapper.
 *
 * Every method in BaseApiService returns Promise<ApiResponse<T>>,
 * giving you a predictable object with the parsed data and HTTP status.
 * `data` is optional because some endpoints return empty bodies (e.g. 204 No Content).
 */
export type ApiResponse<T> = {
  data?: T;
  status: number;
};

/**
 * Abstract base class for all API services in the application.
 *
 * WHY ABSTRACT?
 * Cannot be instantiated directly. Every feature area (users, invoices, products...)
 * creates its own service that extends this class, inheriting all HTTP methods.
 * This avoids copy-pasting the same _get/_post/_put/_delete pattern everywhere.
 *
 * WHY NOT USE HttpClient DIRECTLY IN COMPONENTS?
 * Components should not know about HTTP at all. They talk to feature services
 * (e.g. InvoiceService), which extend this base. The base handles:
 *   - Prepending the API base URL automatically
 *   - Setting withCredentials to send session cookies on every request
 *   - Converting Observables → Promises for simpler async/await usage
 *   - Returning a consistent ApiResponse<T> shape every time
 *
 * WHY PROMISES INSTEAD OF OBSERVABLES?
 * Angular's HttpClient returns Observables, which are powerful but verbose
 * for simple request/response calls. We convert them to Promises with
 * firstValueFrom() so services and components can use async/await,
 * which is easier to read and reason about for most use cases.
 *
 * WHY withCredentials: true?
 * This tells the browser to send cookies with every request, which is required
 * for session-based authentication. The backend sets an HttpOnly session cookie
 * after login, and withCredentials ensures it is sent automatically on every call.
 */
export abstract class BaseApiService {
  protected readonly http = inject(HttpClient);
  protected readonly apiUrl = environment.apiUrl;
}
