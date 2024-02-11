namespace HBLibrary.Services.Security.Cryptography.Keys; 
public interface IKey {
    string Name { get; }
    byte[] Key { get; }
}
