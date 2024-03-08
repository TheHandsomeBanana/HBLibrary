namespace HBLibrary.Services.IO.Operations;
public class IOOperationErrorResponse : IOOperationResponse {
    public IOOperationErrorResponse(string error) {
        Success = false;
        ErrorMessage = error;
    }
}
