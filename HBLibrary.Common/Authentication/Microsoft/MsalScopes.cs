namespace HBLibrary.Common.Authentication.Microsoft;
public static class MsalScopes {
    // User scopes
    public const string UserRead = "User.Read";
    public const string UserReadBasicAll = "User.ReadBasic.All";
    public const string UserReadWrite = "User.ReadWrite";
    public const string UserReadAll = "User.Read.All";
    public const string UserReadWriteAll = "User.ReadWrite.All";

    // Mail scopes
    public const string MailRead = "Mail.Read";
    public const string MailReadWrite = "Mail.ReadWrite";
    public const string MailSend = "Mail.Send";
    public const string MailboxSettingsRead = "MailboxSettings.Read";
    public const string MailboxSettingsReadWrite = "MailboxSettings.ReadWrite";

    // Calendar scopes
    public const string CalendarsRead = "Calendars.Read";
    public const string CalendarsReadWrite = "Calendars.ReadWrite";
    public const string CalendarsReadShared = "Calendars.Read.Shared";
    public const string CalendarsReadWriteShared = "Calendars.ReadWrite.Shared";

    // Contacts scopes
    public const string ContactsRead = "Contacts.Read";
    public const string ContactsReadWrite = "Contacts.ReadWrite";

    // Files/OneDrive scopes
    public const string FilesRead = "Files.Read";
    public const string FilesReadAll = "Files.Read.All";
    public const string FilesReadWrite = "Files.ReadWrite";
    public const string FilesReadWriteAll = "Files.ReadWrite.All";
    public const string FilesReadSelected = "Files.Read.Selected";
    public const string FilesReadWriteAppFolder = "Files.ReadWrite.AppFolder";
    public const string FilesReadWriteSelected = "Files.ReadWrite.Selected";

    // Group scopes
    public const string GroupReadAll = "Group.Read.All";
    public const string GroupReadWriteAll = "Group.ReadWrite.All";
    public const string GroupMemberReadAll = "GroupMember.Read.All";
    public const string GroupMemberReadWriteAll = "GroupMember.ReadWrite.All";

    // Directory scopes
    public const string DirectoryReadAll = "Directory.Read.All";
    public const string DirectoryReadWriteAll = "Directory.ReadWrite.All";
    public const string DirectoryAccessAsUserAll = "Directory.AccessAsUser.All";

    // Application scopes
    public const string ApplicationReadAll = "Application.Read.All";
    public const string ApplicationReadWriteAll = "Application.ReadWrite.All";

    // Policy scopes
    public const string PolicyReadAll = "Policy.Read.All";
    public const string PolicyReadWriteApplicationConfiguration = "Policy.ReadWrite.ApplicationConfiguration";
    public const string PolicyReadWriteConditionalAccess = "Policy.ReadWrite.ConditionalAccess";

    // Device scopes
    public const string DeviceRead = "Device.Read";
    public const string DeviceReadWrite = "Device.ReadWrite";

    // Identity Provider scopes
    public const string IdentityProviderReadAll = "IdentityProvider.Read.All";
    public const string IdentityProviderReadWriteAll = "IdentityProvider.ReadWrite.All";

    // Role Management scopes
    public const string RoleManagementRead = "RoleManagement.Read";
    public const string RoleManagementReadWrite = "RoleManagement.ReadWrite";

    // Audit Log scopes
    public const string AuditLogReadAll = "AuditLog.Read.All";
}
