package backend.common.services.globalExceptionHandler;
/*
 * =========================================================================
 * HTTP CLIENT ERRORS (4xx)
 * =========================================================================
 *
 * Bad Request                      400
 * Unauthorized                     401
 * Payment Required                 402
 * Forbidden                        403
 * Not Found                        404
 * Method Not Allowed               405
 * Not Acceptable                   406
 * Proxy Authentication Required    407
 * Request Timeout                  408
 * Conflict                         409
 * Gone                             410
 * Length Required                  411
 * Precondition Failed              412
 * Content Too Large                413
 * URI Too Long                     414
 * Unsupported Media Type           415
 * Range Not Satisfiable            416
 * Expectation Failed               417
 * I'm a Teapot                     418
 * Misdirected Request              421
 * Unprocessable Content            422
 * Locked                           423
 * Failed Dependency                424
 * Too Early                        425
 * Upgrade Required                 426
 * Precondition Required            428
 * Too Many Requests                429
 * Request Header Fields Too Large  431
 * Unavailable For Legal Reasons    451
 *
 * =========================================================================
 * HTTP SERVER ERRORS (5xx)
 * =========================================================================
 *
 * Internal Server Error            500
 * Not Implemented                  501
 * Bad Gateway                      502
 * Service Unavailable              503
 * Gateway Timeout                  504
 * HTTP Version Not Supported       505
 * Variant Also Negotiates          506
 * Insufficient Storage             507
 * Loop Detected                    508
 * Not Extended                     510
 * Network Authentication Required  511
 *
 * =========================================================================
 * DATABASE
 * =========================================================================
 *
 * Entity not found
 * Duplicate entry / unique constraint violation
 * Connection failure
 *
 * =========================================================================
 * VALIDATION
 * =========================================================================
 *
 * Null/empty fields
 * Invalid format (email, date, UUID...)
 * Value out of range
 * Type mismatch
 *
 * =========================================================================
 * Authentication / Authorization
 * =========================================================================
 *
 * Invalid token / expired token
 * Invalid credentials
 * Insufficient permissions
 *
 * =========================================================================
 * Serialization
 * =========================================================================
 *
 * Invalid JSON / XML
 * Missing required fields in request body
 * Type conversion error
 *
 * =========================================================================
 * Website
 * =========================================================================
 *
 * Exceeded limit (ex: max seats on a trip)
 * Invalid Filter
 * Invalid Sort
 *
 * =========================================================================
 * External Services
 * =========================================================================
 *
 * API call timeout
 * Third party service unavailable
 * Invalid response from external API
 *
 * =========================================================================
 * File / IO
 * =========================================================================
 *
 * File not found
 * Unsupported file format
 * File too large
 *
 * =========================================================================
 */


/**
 * Base para excepções de domínio.
 * Todas as excepções do projecto estendem esta.
 * O GlobalExceptionHandler trata subclasses.
 *
 //* @param code    Código máquina (ex: "NOT_FOUND", "INVALID_FILTER")
 //* @param message Mensagem legível (ex: "Trip not found")
 */
public abstract class DomainException extends RuntimeException {

    private final String code;

    protected DomainException(String code, String message) {
        super(message);
        this.code = code;
    }

    public String getCode() {
        return code;
    }
}

/**
 * HTTP Client Error 400
 * <br>
 * The server cannot or will not process the request due to something that is
 * perceived to be a client error (e.g., malformed request syntax, invalid
 * request message framing, or deceptive request routing).
 */
class BadRequestException extends DomainException {

    public BadRequestException(String message) {
        super("BAD_REQUEST", message);
    }
}

/**
 * HTTP Client Error 401
 * <br>
 * Although the HTTP standard specifies "unauthorized", semantically this
 * response means "unauthenticated". That is, the client must authenticate
 * itself to get the requested response.
 */
class UnauthorizedException extends DomainException {

    public UnauthorizedException(String message) {
        super("UNAUTHORIZED", message);
    }
}

/**
 * HTTP Client Error 402
 * <br>
 * The initial purpose of this code was for digital payment systems, however
 * this status code is rarely used and no standard convention exists.
 */
class PaymentRequiredException extends DomainException {

    public PaymentRequiredException(String message) {
        super("PAYMENT_REQUIRED", message);
    }
}

/**
 * HTTP Client Error 403
 * <br>
 * The client does not have access rights to the content; that is, it is
 * unauthorized, so the server is refusing to give the requested resource.
 * Unlike 401 Unauthorized, the client's identity is known to the server.
 */
class ForbiddenException extends DomainException {

    public ForbiddenException(String message) {
        super("FORBIDDEN", message);
    }
}

/**
 * HTTP Client Error 404
 * <br>
 * The server cannot find the requested resource. In the browser, this means the
 * URL is not recognized. In an API, this can also mean that the endpoint is
 * valid but the resource itself does not exist. Servers may also send this
 * response instead of 403 Forbidden to hide the existence of a resource from an
 * unauthorized client. This response code is probably the most well known due
 * to its frequent occurrence on the web.
 */
class NotFoundException extends DomainException {

    public NotFoundException(String message) {
        super("NOT_FOUND", message);
    }
}

/**
 * HTTP Client Error 405
 * <br>
 * The request method is known by the server but is not supported by the target
 * resource. For example, an API may not allow to DELETE on a resource, or the
 * TRACE method entirely.
 */
class MethodNotAllowedException extends DomainException {

    public MethodNotAllowedException(String message) {
        super("METHOD_NOT_ALLOWED", message);
    }
}

/**
 * HTTP Client Error 406
 * <br>
 * This response is sent when the web server, after performing server-driven
 * content negotiation, doesn't find any content that conforms to the criteria
 * given by the user agent.
 */
class NotAcceptableException extends DomainException {

    public NotAcceptableException(String message) {
        super("NOT_ACCEPTABLE", message);
    }
}

/**
 * HTTP Client Error 407
 * <br>
 * This is similar to 401 Unauthorized but authentication is needed to be done
 * by a proxy.
 */
class ProxyAuthenticationRequiredException extends DomainException {

    public ProxyAuthenticationRequiredException(String message) {
        super("PROXY_AUTHENTICATION_REQUIRED", message);
    }
}

/**
 * HTTP Client Error 408
 * <br>
 * This response is sent on an idle connection by some servers, even without any
 * previous request by the client. It means that the server would like to shut
 * down this unused connection. This response is used much more since some
 * browsers use HTTP pre-connection mechanisms to speed up browsing. Some
 * servers may shut down a connection without sending this message.
 */
class RequestTimoutException extends DomainException {

    public RequestTimoutException(String message) {
        super("REQUEST_TIMEOUT", message);
    }
}

/**
 * HTTP Client Error 409
 * <br>
 * This response is sent when a request conflicts with the current state of the
 * server. In WebDAV remote web authoring, 409 responses are errors sent to the
 * client so that a user might be able to resolve a conflict and resubmit the
 * request.
 */
class ConflictException extends DomainException {

    public ConflictException(String message) {
        super("CONFLICT", message);
    }
}

/**
 * HTTP Client Error 410
 * <br>
 * This response is sent when the requested content has been permanently deleted
 * from server, with no forwarding address. Clients are expected to remove their
 * caches and links to the resource. The HTTP specification intends this status
 * code to be used for "limited-time, promotional services". APIs should not
 * feel compelled to indicate resources that have been deleted with this status
 * code.
 */
class GoneException extends DomainException {

    public GoneException(String message) {
        super("GONE", message);
    }
}

/**
 * HTTP Client Error 411
 * <br>
 * Server rejected the request because the Content-Length header field is not
 * defined and the server requires it.
 */
class LengthRequiredException extends DomainException {

    public LengthRequiredException(String message) {
        super("LENGTH_REQUIRED", message);
    }
}

/**
 * HTTP Client Error 412
 * <br>
 * In conditional requests, the client has indicated preconditions in its
 * headers which the server does not meet.
 */
class PreconditionFailedException extends DomainException {

    public PreconditionFailedException(String message) {
        super("PRECONDITION_FAILED", message);
    }
}

/**
 * HTTP Client Error 413
 * <br>
 * The request body is larger than limits defined by server. The server might
 * close the connection or return a Retry-After header field.
 */
class ContentTooLargeException extends DomainException {

    public ContentTooLargeException(String message) {
        super("CONTENT_TOO_LARGE", message);
    }
}

/**
 * HTTP Client Error 414
 * <br>
 * The URI requested by the client is longer than the server is willing to
 * interpret.
 */
class UriTooLongException extends DomainException {

    public UriTooLongException(String message) {
        super("URI_TOO_LONG", message);
    }
}

/**
 * HTTP Client Error 415
 * <br>
 * The media format of the requested data is not supported by the server, so the
 * server is rejecting the request.
 */
class UnsupportedMediaTypeException extends DomainException {

    public UnsupportedMediaTypeException(String message) {
        super("UNSUPPORTED_MEDIA_TYPE", message);
    }
}

/**
 * HTTP Client Error 416
 * <br>
 * The ranges specified by the Range header field in the request cannot be
 * fulfilled. It's possible that the range is outside the size of the target
 * resource's data.
 */
class RangeNotSatisfiableException extends DomainException {

    public RangeNotSatisfiableException(String message) {
        super("RANGE_NOT_SATISFIABLE", message);
    }
}

/**
 * HTTP Client Error 417
 * <br>
 * This response code means the expectation indicated by the Expect request
 * header field cannot be met by the server.
 */
class ExpectationFailedException extends DomainException {

    public ExpectationFailedException(String message) {
        super("EXPECTATION_FAILED", message);
    }
}

/**
 * HTTP Client Error 418
 * <br>
 * The server refuses the attempt to brew coffee with a teapot.
 */
class ImATeapotException extends DomainException {

    public ImATeapotException(String message) {
        super("IM_A_TEAPOT", message);
    }
}

/**
 * HTTP Client Error 421
 * <br>
 * The request was directed at a server that is not able to produce a response.
 * This can be sent by a server that is not configured to produce responses for
 * the combination of scheme and authority that are included in the request URI.
 */
class MisdirectedRequestException extends DomainException {

    public MisdirectedRequestException(String message) {
        super("MISDIRECTED_REQUEST", message);
    }
}

/**
 * HTTP Client Error 422
 * <br>
 * The request was well-formed but was unable to be followed due to semantic
 * errors.
 */
class UnprocessableContentException extends DomainException {

    public UnprocessableContentException(String message) {
        super("UNPROCESSABLE_CONTENT", message);
    }
}

/**
 * HTTP Client Error 423
 * <br>
 * The resource that is being accessed is locked.
 */
class LockedException extends DomainException {

    public LockedException(String message) {
        super("LOCKED", message);
    }
}

/**
 * HTTP Client Error 424
 * <br>
 * The request failed due to failure of a previous request.
 */
class FailedDependencyException extends DomainException {

    public FailedDependencyException(String message) {
        super("FAILED_DEPENDENCY", message);
    }
}

/**
 * HTTP Client Error 425
 * <br>
 * Indicates that the server is unwilling to risk processing a request that
 * might be replayed.
 */
class TooEarlyException extends DomainException {

    public TooEarlyException(String message) {
        super("TOO_EARLY", message);
    }
}

/**
 * HTTP Client Error 426
 * <br>
 * The server refuses to perform the request using the current protocol but
 * might be willing to do so after the client upgrades to a different protocol.
 * The server sends an Upgrade header in a 426 response to indicate the required
 * protocol(s).
 */
class UpgradeRequiredException extends DomainException {

    public UpgradeRequiredException(String message) {
        super("UPGRADE_REQUIRED", message);
    }
}

/**
 * HTTP Client Error 428
 * <br>
 * The origin server requires the request to be conditional. This response is
 * intended to prevent the 'lost update' problem, where a client GETs a
 * resource's state, modifies it and PUTs it back to the server, when meanwhile
 * a third party has modified the state on the server, leading to a conflict.
 */
class PreconditionRequiredException extends DomainException {

    public PreconditionRequiredException(String message) {
        super("PRECONDITION_REQUIRED", message);
    }
}

/**
 * HTTP Client Error 429
 * <br>
 * The user has sent too many requests in a given amount of time (rate
 * limiting).
 */
class TooManyRequestsException extends DomainException {

    public TooManyRequestsException(String message) {
        super("TOO_MANY_REQUESTS", message);
    }
}

/**
 * HTTP Client Error 431
 * <br>
 * The server is unwilling to process the request because its header fields are
 * too large. The request may be resubmitted after reducing the size of the
 * request header fields.
 */
class RequestHeaderFieldsTooLargeException extends DomainException {

    public RequestHeaderFieldsTooLargeException(String message) {
        super("REQUEST_HEADER_FIELDS_TOO_LARGE", message);
    }
}

/**
 * HTTP Client Error 451
 * <br>
 * The user agent requested a resource that cannot legally be provided, such as
 * a web page censored by a government.
 */
class UnavailableForLegalReasonsException extends DomainException {

    public UnavailableForLegalReasonsException(String message) {
        super("UNAVAILABLE_FOR_LEGAL_REASONS", message);
    }
}

/**
 * HTTP Server Error 500
 * <br>
 * The server has encountered a situation it does not know how to handle. This
 * error is generic, indicating that the server cannot find a more appropriate
 * 5XX status code to respond with.
 */
class InternalServerErrorException extends DomainException {

    public InternalServerErrorException(String message) {
        super("INTERNAL_SERVER_ERROR", message);
    }
}

/**
 * HTTP Server Error 501
 * <br>
 * The request method is not supported by the server and cannot be handled. The
 * only methods that servers are required to support (and therefore must not
 * return this code) are GET and HEAD.
 */
class NotImplementedException extends DomainException {

    public NotImplementedException(String message) {
        super("NOT_IMPLEMENTED", message);
    }
}

/**
 * HTTP Server Error 502
 * <br>
 * This error response means that the server, while working as a gateway to get
 * a response needed to handle the request, got an invalid response.
 */
class BadGatewayException extends DomainException {

    public BadGatewayException(String message) {
        super("BAD_GATEWAY", message);
    }
}

/**
 * HTTP Server Error 503
 * <br>
 * The server is not ready to handle the request. Common causes are a server
 * that is down for maintenance or that is overloaded. Note that together with
 * this response, a user-friendly page explaining the problem should be sent.
 * This response should be used for temporary conditions and the Retry-After
 * HTTP header should, if possible, contain the estimated time before the
 * recovery of the service. The webmaster must also take care about the
 * caching-related headers that are sent along with this response, as these
 * temporary condition responses should usually not be cached.
 */
class ServiceUnavailableException extends DomainException {

    public ServiceUnavailableException(String message) {
        super("SERVICE_UNAVAILABLE", message);
    }
}

/**
 * HTTP Server Error 504
 * <br>
 * This error response is given when the server is acting as a gateway and
 * cannot get a response in time.
 */
class GatewayTimeoutException extends DomainException {

    public GatewayTimeoutException(String message) {
        super("GATEWAY_TIMEOUT", message);
    }
}

/**
 * HTTP Server Error 505
 * <br>
 * The HTTP version used in the request is not supported by the server.
 */
class HttpVersionNotSupportedException extends DomainException {

    public HttpVersionNotSupportedException(String message) {
        super("HTTP_VERSION_NOT_SUPPORTED", message);
    }
}

/**
 * HTTP Server Error 506
 * <br>
 * The server has an internal configuration error: during content negotiation,
 * the chosen variant is configured to engage in content negotiation itself,
 * which results in circular references when creating responses.
 */
class VariantAlsoNegotiatesException extends DomainException {

    public VariantAlsoNegotiatesException(String message) {
        super("VARIANT_ALSO_NEGOTIATES", message);
    }
}

/**
 * HTTP Server Error 507
 * <br>
 * The method could not be performed on the resource because the server is
 * unable to store the representation needed to successfully complete the
 * request.
 */
class InsufficientStorageException extends DomainException {

    public InsufficientStorageException(String message) {
        super("INSUFFICIENT_STORAGE", message);
    }
}

/**
 * HTTP Server Error 508
 * <br>
 * The server detected an infinite loop while processing the request.
 */
class LoopDetectedException extends DomainException {

    public LoopDetectedException(String message) {
        super("LOOP_DETECTED", message);
    }
}

/**
 * HTTP Server Error 510
 * <br>
 * The client request declares an HTTP Extension (RFC 2774) that should be used
 * to process the request, but the extension is not supported.
 */
class NotExtendedException extends DomainException {

    public NotExtendedException(String message) {
        super("NOT_EXTENDED", message);
    }
}

/**
 * HTTP Server Error 511
 * <br>
 * Indicates that the client needs to authenticate to gain network access.
 */
class NetworkAuthenticationRequiredException extends DomainException {

    public NetworkAuthenticationRequiredException(String message) {
        super("NETWORK_AUTHENTICATION_REQUIRED", message);
    }
}

/**
 * Database
 * <br>
 * Indicates that the entity was not found in the database.
 */
class EntityNotFoundException extends DomainException {

    public EntityNotFoundException(String message) {
        super("ENTITY_NOT_FOUND", message);
    }
}

/**
 * Database
 * <br>
 * Indicates that there's already an entry with the same value in the database.
 */
class DuplicateEntryException extends DomainException {

    public DuplicateEntryException(String message) {
        super("DUPLICATE_ENTRY", message);
    }
}

/**
 * Database
 * <br>
 * Failure while trying to connect with the database.
 */
class ConnectionFailureException extends DomainException {

    public ConnectionFailureException(String message) {
        super("CONNECTION_FAILURE", message);
    }
}

/**
 * Validation
 * <br>
 * The field in question is empty and must have some kind of value.
 */
class EmptyFieldException extends DomainException {

    public EmptyFieldException(String message) {
        super("EMPTY_FIELD", message);
    }
}

/**
 * Validation
 * <br>
 * The value put in the field has an invalid format
 */
class InvalidFormatException extends DomainException {

    public InvalidFormatException(String message) {
        super("INVALID_FORMAT", message);
    }
}

/**
 * Validation
 * <br>
 * The field value is out of range of the values set.
 */
class ValueOutOfRangeException extends DomainException {

    public ValueOutOfRangeException(String message) {
        super("VALUE_OUT_OF_RANGE", message);
    }
}

/**
 * Validation
 * <br>
 * The value put in the field is a whole different type
 */
class TypeMismatchException extends DomainException {

    public TypeMismatchException(String message) {
        super("TYPE_MISMATCH", message);
    }
}

/**
 * Authentication / Authorization
 * <br>
 * The token selected is not correct or expired.
 */
class InvalidTokenException extends DomainException {

    public InvalidTokenException(String message) {
        super("TYPE_MISMATCH", message);
    }
}

/**
 * Authentication / Authorization
 * <br>
 * The credentials put are invalid.
 */
class InvalidCredentialsException extends DomainException {

    public InvalidCredentialsException(String message) {
        super("INVALIDE_CREDENTIALS", message);
    }
}

/**
 * Authentication / Authorization
 * <br>
 * The user doesn't have enough permissions.
 */
class InsufficientPermissionsException extends DomainException {

    public InsufficientPermissionsException(String message) {
        super("INSUFFICIENT_PERMISSIONS", message);
    }
}

/**
 * Serialization
 * <br>
 * JSON file inserted is invalid.
 */
class InvalidJSONException extends DomainException {

    public InvalidJSONException(String message) {
        super("INVALID_JSON", message);
    }
}

/**
 * Serialization
 * <br>
 * One or more required fields of the body are empty.
 */
class MissingFieldException extends DomainException {

    public MissingFieldException(String message) {
        super("MISSING_FIELD", message);
    }
}

/**
 * Serialization
 * <br>
 * The type converted was not correct.
 */
class TypeConversionException extends DomainException {

    public TypeConversionException(String message) {
        super("TYPE_CONVERSION", message);
    }
}

/**
 * Website
 * <br>
 * The limit of this variable was exceeded.
 */
class LimitExceededException extends DomainException {

    public LimitExceededException(String message) {
        super("LIMIT_EXCEEDED", message);
    }
}

/**
 * Website
 * <br>
 * Filter invalid for this action.
 */
class InvalidFilterException extends DomainException {
    public InvalidFilterException(String message) {
        super("INVALID_FILTER", message);
    }
}

/**
 * Website
 * <br>
 * Sort invalid for this action.
 */
class InvalidSortException extends DomainException {
    public InvalidSortException(String message) {
        super("INVALID_SORT", message);
    }
}

/**
 * External Services
 * <br>
 * The API response is taking to much time.
 */
class APICallTimeoutException extends DomainException {
    public APICallTimeoutException(String message) {

        super("API_CALL_TIMEOUT", message);
    }
}

/**
 * External Services
 * <br>
 * The third party service is unavailable at the moment.
 */
class ThirdPartyUnavailableException extends DomainException {
    public ThirdPartyUnavailableException(String message) {

        super("THIRD_PARTY_UNAVAILABLE", message);
    }
}

/**
 * External Services
 * <br>
 * There's an invalid response from external API.
 */
class APIInvalidResponseException extends DomainException {
    public APIInvalidResponseException(String message) {

        super("API_INVALID_RESPONSE", message);
    }
}

/**
 * File / IO
 * <br>
 * The file you're searching was not found.
 */
class FileNotFoundException extends DomainException {
    public FileNotFoundException(String message) {

        super("FILE_NOT_FOUND", message);
    }
}

/**
 * File / IO
 * <br>
 * File format not suppoerted.
 */
class UnsupportedFileFormatException extends DomainException {
    public UnsupportedFileFormatException(String message) {

        super("UNSUPPORTED_FILE_FORMAT", message);
    }
}

/**
 * File / IO
 * <br>
 * File is too large.
 */
class FileTooLargeException extends DomainException {
    public FileTooLargeException(String message) {

        super("FILE_TOO_LARGE", message);
    }
}

/*
Debating

Transaction failure (Database)
Insufficient balance (Business Logic)
Booking conflict (Business Logic)
Invalid state transition (ex: can't cancel an already completed order) (Business Logic)
*/