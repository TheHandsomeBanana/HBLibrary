namespace HBLibrary.Core.Extensions;
public static class TypeExtensions {
    public static string GuidString(this Type type) => type.GUID.ToString();
}
