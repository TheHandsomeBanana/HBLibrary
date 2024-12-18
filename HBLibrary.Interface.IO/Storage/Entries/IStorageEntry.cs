﻿using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO.Storage.Settings;

namespace HBLibrary.Interface.IO.Storage.Entries;
public interface IStorageEntry : ITrackable {
    public string Filename { get; }
    public StorageEntryContentType ContentType { get; }
    public StorageEntrySettings Settings { get; set; }
    public Type? CurrentEntryType { get; }

    public object? Get(Type type);
    public void Set(object value);
    public void Save();

    public T? Get<T>();
    public void Set<T>(T value);
    public void Save<T>();

    public Task<object?> GetAsync(Type type);
    public Task SaveAsync();
    public Task<T?> GetAsync<T>();
    public Task SaveAsync<T>();
}

public enum StorageEntryContentType {
    Csv,
    Json,
    Xml,
}
