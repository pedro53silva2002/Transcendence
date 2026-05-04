/**
 * Represents the origin of an error
 * CLIENT -> the error was created on frontend
 * SERVER -> the error came from the backend
 */

export type ApiErrorSource = 'CLIENT' | 'SERVER';

export interface ApiErrorOptions {
  source: ApiErrorSource;
  statusCode: number; // HTTP Status code (0 if client sided)
  code: string; // error code, like user_not_found
  message?: string; // can be null
  metadata?: Record<string, unknown>; // If we need to pass more information about the error
}

/**
 * A strongly typed error class used throug the app
 *
 * Instead of doing always a try catch, we map the errors to an ApiError. With this, we have:
 * - A consistent shape to work with in catch blocks;
 * - A status code to react to
 * - A machine code to display specific messages (useful in i18n)
 * - Metadata from the server response
 *
 * We extend it from the Error native class, because we want that:
 * - 'instanceof Error' checks still work
 * - Stack traces are preserved
 * - It integrates with Angular's HttpErrorResponse handling
 */
export class ApiError extends Error {
  source: ApiErrorSource;
  statusCode: number;
  code: string;
  metadata?: Record<string, unknown>;

  constructor({ source, statusCode, code, message, metadata }: ApiErrorOptions) {
    super(message ?? code);

    Object.setPrototypeOf(this, ApiError.prototype);

    this.name = 'ApiError';
    this.source = source;
    this.statusCode = statusCode;
    this.code = code;
    this.metadata = metadata;
  }
  /**
   * Method to build an ApiError from Angular's HttpErrorResponse
   *
   * Angular's HttpClient wraps all failed HTTP calls in HttpErrorResponse,
   * so this is the standard way to create server-side errors.
   *
   * The server body is expected to have 'code' and 'message' fields
   * If they are missing, we fall back to HTTP status text
   */
  static fromResponse(res: import('@angular/common/http').HttpErrorResponse): ApiError {
    const body = res.error;
    const code = body?.code ?? 'server_error';
    const message = body?.message ?? res.message;

    return new ApiError({
      source: 'SERVER',
      statusCode: res.status,
      code,
      message,
      metadata: body, // store the full server body
    });
  }

  /**
   * Method to build a client side ApiError
   *
   * Used when something goes wrong in the frontend itself,
   * not because of an HTTP response (e.g. missing data, unexpected state)
   *
   * statusCode is 0 to indicate "not an HTTP error".
   */
  static fromClient(code: string, message: string, metadata?: Record<string, unknown>): ApiError {
    return new ApiError({
      source: 'CLIENT',
      statusCode: 0,
      code,
      message,
      metadata,
    });
  }
}
