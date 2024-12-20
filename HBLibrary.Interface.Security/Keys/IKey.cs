namespace HBLibrary.Interface.Security.Keys;
public interface IKey : IDisposable {
    bool IsDisposed { get; }
    string Name { get; }
}
