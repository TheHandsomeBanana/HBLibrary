using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Storage;
public interface IPersistable<T> {
    public void Save(T obj);
}

public interface IPersistable {
    public void Save(object obj);
    public void Save<T>(T obj);
}

public interface IAsyncPersistable<T> {
    public Task SaveAsync(T obj);
}

public interface IAsyncPersistable {
    public Task SaveAsync(object obj);
    public Task SaveAsync<T>(T obj);
}


