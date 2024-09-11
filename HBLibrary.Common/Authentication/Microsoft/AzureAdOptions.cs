namespace HBLibrary.Common.Authentication.Microsoft;
public class AzureAdOptions {
    public string? ClientId { get; set; }
    public string? TenantId { get; set; }
    public string? RedirectUri { get; set; }
    public string Authority => $"https://login.microsoftonline.com/{TenantId}";
}
