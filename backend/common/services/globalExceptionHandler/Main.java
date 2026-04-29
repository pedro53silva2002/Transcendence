//package transcendence.backend.common.services.globalExceptionHandler;
/* class Main {
     public static void main(String[] args) {
         try {
             // Force an exception (division by zero)
             int result = 10 / 0;
 
             System.out.println("Result: " + result);
         } catch (Exception e) {
            StackTraceElement f = e.getStackTrace()[0];
            System.out.println(f.getFileName() + ":" + f.getLineNumber() + " - " + e.getMessage());
         }

         System.out.println("Program continues after exception handling.");
     }
} */


public class Main {
    public static void main(String[] args) {

        // Test each exception
        testException(new BadRequestException("Invalid input data"));
        testException(new UnauthorizedException("You must be logged in"));
        testException(new PaymentRequiredException("Subscription required"));
        testException(new ForbiddenException("You don't have permission"));
        testException(new NotFoundException("Resource not found"));
        testException(new MethodNotAllowedException("Method not allowed"));
        testException(new NotAcceptableException("Not acceptable"));
        testException(new ProxyAuthenticationRequiredException("Proxy authentication required"));
        testException(new RequestTimoutException("Request timed out"));
        testException(new ConflictException("Conflict detected"));
        testException(new GoneException("Resource gone"));
        testException(new LengthRequiredException("Content-Length header missing"));
        testException(new PreconditionFailedException("Precondition failed"));
        testException(new ContentTooLargeException("Content too large"));
        testException(new UriTooLongException("URI too long"));
        testException(new UnsupportedMediaTypeException("Unsupported media type"));
        testException(new RangeNotSatisfiableException("Range not satisfiable"));
        testException(new ExpectationFailedException("Expectation failed"));
        testException(new ImATeapotException("I'm a teapot"));
        testException(new MisdirectedRequestException("Misdirected request"));
        testException(new UnprocessableContentException("Unprocessable content"));
        testException(new LockedException("Resource is locked"));
        testException(new FailedDependencyException("Failed dependency"));
        testException(new TooEarlyException("Too early"));
        testException(new UpgradeRequiredException("Upgrade required"));
        testException(new PreconditionRequiredException("Precondition required"));
        testException(new TooManyRequestsException("Too many requests"));
        testException(new RequestHeaderFieldsTooLargeException("Request header fields too large"));
        testException(new UnavailableForLegalReasonsException("Unavailable for legal reasons"));
        testException(new InternalServerErrorException("Internal server error"));
        testException(new NotImplementedException("Not implemented"));
        testException(new BadGatewayException("Bad gateway"));
        testException(new ServiceUnavailableException("Service unavailable"));
        testException(new GatewayTimeoutException("Gateway timeout"));
        testException(new HttpVersionNotSupportedException("HTTP version not supported"));
        testException(new VariantAlsoNegotiatesException("Variant also negotiates"));
        testException(new InsufficientStorageException("Insufficient storage"));
        testException(new LoopDetectedException("Loop detected"));
        testException(new NotExtendedException("Not extended"));
        testException(new NetworkAuthenticationRequiredException("Network authentication required"));
        testException(new EntityNotFoundException("User not found in database"));
        testException(new DuplicateEntryException("Email already exists"));
        testException(new ConnectionFailureException("Could not connect to database"));
        testException(new EmptyFieldException("Name field is empty"));
        testException(new InvalidFormatException("Invalid email format"));
        testException(new ValueOutOfRangeException("Age must be between 0 and 120"));
        testException(new TypeMismatchException("Expected integer, got string"));
        testException(new InvalidTokenException("Token is expired"));
        testException(new InvalidCredentialsException("Wrong email or password"));
        testException(new InsufficientPermissionsException("Admin role required"));
        testException(new InvalidJSONException("Malformed JSON body"));
        testException(new MissingFieldException("Field 'email' is required"));
        testException(new TypeConversionException("Cannot convert 'abc' to integer"));
        testException(new LimitExceededException("Max 10 items per page"));
        testException(new InvalidFilterException("Filter 'color' is not allowed here"));
        testException(new InvalidSortException("Cannot sort by 'password'"));
        testException(new APICallTimeoutException("Payment API is taking too long to respond"));
        testException(new ThirdPartyUnavailableException("Google Maps service is unavailable"));
        testException(new APIInvalidResponseException("Received invalid response from payment API"));
        testException(new FileNotFoundException("profile_picture.png not found"));
        testException(new UnsupportedFileFormatException("File format .exe is not supported"));
        testException(new FileTooLargeException("File exceeds the 10MB limit"));

        // Test that catching by parent class works
        System.out.println("\n-- Catching by parent class --");
        try {
            throw new NotFoundException("Trip not found");
        } catch (DomainException e) {
			StackTraceElement f = e.getStackTrace()[0];
        	System.out.println(f.getFileName() + ":" + f.getLineNumber() + " - " + e.getCode() + " | " + e.getMessage());
            //System.out.println("Caught as DomainException: " + e.getCode() + " | " + e.getMessage());
        }
    }

    /**
     * Helper to test any DomainException subclass.
     *
     * @param e the exception to test
     */
    static void testException(DomainException e) {
        System.out.println("Code: " + e.getCode() + " | Message: " + e.getMessage());
    }
}