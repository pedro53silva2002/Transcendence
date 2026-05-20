import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { firstValueFrom, map, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

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
  // inject() can be used outside a constructor in Angular 14+.
  // No need to declare a constructor just to inject dependencies.
  protected readonly http = inject(HttpClient);
  protected readonly apiUrl = environment.apiUrl;

  /**
   * GET request. Used for fetching data.
   * No body is allowed by the HTTP spec.
   *
   * @param path - Endpoint path (e.g. '/invoices/1'). Base URL is prepended automatically.
   */
  protected _get<T>(path: string): Promise<ApiResponse<T>> {
    return firstValueFrom(this.http.get<T>(this.apiUrl + path, { withCredentials: true })).then(
      (data) => ({ status: 200, data }),
    );
    // If the request fails, the errorInterceptor converts the error
    // to an ApiError before it reaches here. No try/catch needed.
  }

  protected _getO<T>(path: string): Observable<ApiResponse<T>> {
    return this.http
      .get<T>(this.apiUrl + path, { withCredentials: true })
      .pipe(map((data) => ({ status: 200, data })));
  }

  /**
   * POST request with a JSON body. Used for creating resources or triggering actions.
   *
   * Angular's HttpClient automatically sets Content-Type: application/json
   * when you pass a plain object — no need to set it manually.
   *
   * @param path - Endpoint path.
   * @param body - Request payload, serialised to JSON automatically.
   */
  protected _post<T>(path: string, body: unknown): Promise<ApiResponse<T>> {
    return firstValueFrom(
      this.http.post<T>(this.apiUrl + path, body, { withCredentials: true }),
    ).then((data) => ({ status: 200, data }));
  }

  /**
   * PUT request with a JSON body. Used for full resource replacements.
   *
   * @param path - Endpoint path.
   * @param body - The complete updated resource.
   */
  protected _put<T>(path: string, body: unknown): Promise<ApiResponse<T>> {
    return firstValueFrom(
      this.http.put<T>(this.apiUrl + path, body, { withCredentials: true }),
    ).then((data) => ({ status: 200, data }));
  }

  /**
   * DELETE request.
   * The body param is optional — most DELETE endpoints don't need one,
   * but the HTTP spec allows it (e.g. bulk delete with an array of IDs).
   *
   * @param path - Endpoint path, usually includes the resource ID (e.g. '/invoices/1').
   * @param body - Optional payload.
   */
  protected _delete<T>(path: string, body?: unknown): Promise<ApiResponse<T>> {
    return firstValueFrom(
      this.http.delete<T>(this.apiUrl + path, { body, withCredentials: true }),
    ).then((data) => ({ status: 200, data }));
  }

  /**
   * POST request with a FormData body. Used for file uploads.
   *
   * We do NOT set Content-Type manually here.
   * When the body is FormData, the browser must set Content-Type itself
   * to include the multipart boundary string. If you set it manually,
   * the boundary is missing and the server cannot parse the form.
   * Angular's HttpClient handles this correctly when FormData is passed.
   *
   * @param path - Endpoint path.
   * @param form - FormData containing files and/or text fields.
   */
  protected _postForm<T>(path: string, form: FormData): Promise<ApiResponse<T>> {
    return firstValueFrom(
      this.http.post<T>(this.apiUrl + path, form, { withCredentials: true }),
    ).then((data) => ({ status: 200, data }));
  }

  /**
   * PUT request with a FormData body.
   * Same as _postForm but for updating existing resources with file uploads.
   *
   * @param path - Endpoint path.
   * @param form - FormData object.
   */
  protected _putForm<T>(path: string, form: FormData): Promise<ApiResponse<T>> {
    return firstValueFrom(
      this.http.put<T>(this.apiUrl + path, form, { withCredentials: true }),
    ).then((data) => ({ status: 200, data }));
  }
}
