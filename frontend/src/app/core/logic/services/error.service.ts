import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiError } from '../model/api-error.model';

/**
 * Centralised error factory and handler.
 *
 * This service exists so that ALL error creation and handling flows
 * through one single place. Benefits:
 *   - You can add global logging/monitoring (e.g. Sentry) in one place
 *   - Components and other services never need to import ApiError directly
 *   - Consistent console output with clear prefixes for debugging
 *
 * It is provided at the root level so it is a singleton across the entire app.
 */

@Injectable({ providedIn: 'root' })
export class ErrorService {
  /**
   * Creates a CLIENT-side error and logs it.
   *
   * Use this when your own code detects an invalid state,
   * like a missing required field before making an HTTP call.
   *
   * Example:
   *   this.errorService.createClientError('invalid_id', 'User ID is required')
   */
  createClientError(code: string, message: string, metadata?: Record<string, unknown>): ApiError {
    const err = ApiError.fromClient(code, message, metadata);
    console.error('[CLIENT ERROR]: ', err);
    return err;
  }

  /**
   * Creates a SERVER-side error from Angular's HttpErrorResponse.
   *
   * Called by the error interceptor automatically on any 4xx/5xx response.
   * You rarely need to call this directly.
   */
  createServerError(res: HttpErrorResponse): ApiError {
    return ApiError.fromResponse(res);
  }

  /**
   * The universal error handler — converts any unknown thrown value into an ApiError.
   *
   * Why do we need this?
   * In TypeScript, catch blocks receive `unknown`, meaning the error could be:
   *   - An ApiError (already processed, just return it)
   *   - An HttpErrorResponse (Angular HTTP error, convert it)
   *   - A native Error (unexpected JS error, wrap it)
   *   - A string, null, or anything else (handle gracefully)
   *
   * By funnelling everything through this method, the rest of the app
   * can always assume they're dealing with an ApiError.
   */
  handle(error: unknown): ApiError {
    if (error instanceof ApiError) return error;
    if (error instanceof HttpErrorResponse) return this.createServerError(error);
    if (error instanceof Error) {
      return ApiError.fromClient('unexpected', error.message);
    }

    return ApiError.fromClient('unknown', 'An unknown error occurred');
  }
}
