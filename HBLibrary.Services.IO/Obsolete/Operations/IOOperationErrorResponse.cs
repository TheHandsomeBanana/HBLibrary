namespace HBLibrary.Services.IO.Obsolete.Operations;
public class IOOperationErrorResponse : IOOperationResponse
{
    public IOOperationErrorResponse(string error)
    {
        Success = false;
        ErrorMessage = error;
    }
}
