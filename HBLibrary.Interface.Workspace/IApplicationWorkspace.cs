using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Account;
using HBLibrary.Interface.Security.Keys;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Workspace;
public interface IApplicationWorkspace {
    public IAccountInfo? Owner { get; set; }
    public IAccountInfo[] SharedAccess { get; set; }
    public string? FullPath { get; set; }
    public bool UsesEncryption { get; set; }

    [JsonIgnore]
    public string? Name { get; }

    [JsonIgnore]
    public bool IsOpen { get; }
    [JsonIgnore]
    public IAccount? OpenedBy { get; set; }

    public event Action? OnOpened;

    public void OnCreated();
    public void Save();
    public Task SaveAsync();
    public Task OpenAsync(IAccount openedBy);
    public void Close();
    public Task CloseAsync();
}
