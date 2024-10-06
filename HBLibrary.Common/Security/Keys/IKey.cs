namespace HBLibrary.Common.Security.Keys;
public interface IKey {
    string Name { get; }
    byte[] Key { get; }
}
