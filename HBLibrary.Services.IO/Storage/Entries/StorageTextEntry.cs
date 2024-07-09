using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageTextEntry : IStorageEntry {
    private string? entry;
    private readonly IFileService fileService;
    public string Filename { get; }

    public StorageEntryContentType ContentType => StorageEntryContentType.Text;

    public Type? CurrentEntryType => entry?.GetType();

    internal StorageTextEntry(IFileService xmlService, string filename) {
        this.fileService = xmlService;
        this.Filename = filename;
    }

    public object? Get(Type type) {
        try {
            if (type != typeof(string)) {
                throw new InvalidOperationException($"A {nameof(StorageTextEntry)} can only contain a string");
            }

            if (entry is null) {
                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                entry = fileService.Read(file!);
            }

            return entry;
        }
        catch {
            return default;
        }
    }

    public void Set(object value) {
        if (value is not string s) {
            throw new InvalidOperationException($"A {nameof(StorageTextEntry)} can only contain a string");
        }

        entry = s;
    }

    public void Save(Type type) {
        if (entry is null) {
            throw new MissingFieldException(nameof(entry));
        }

        if (type != typeof(string)) {
            throw new InvalidOperationException($"A {nameof(StorageTextEntry)} can only contain a string");
        }

        fileService.Write(FileSnapshot.Create(Filename, true), entry);
    }

    public void Save() {
        if (CurrentEntryType is null) {
            throw new InvalidOperationException($"{nameof(entry)} is null.");
        }

        Save(CurrentEntryType);
    }
}
