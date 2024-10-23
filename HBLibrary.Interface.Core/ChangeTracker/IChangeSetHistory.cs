using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public interface IChangeSetHistory {
    public object? this[string key] { get; set; }
    public void AddOrUpdate(string key, object? value);
    public object? GetNewestValue(string key);
    public IReadOnlyDictionary<string, object?> GetNewestValues();
    public IReadOnlyDictionary<DateTime, object?>? GetHistory(string key);
    public bool LastValueChanged(string key, object? value);
    /// <summary>
    /// Will add or update the value, if it changed to the previous value. 
    /// This only works if <see cref="object.Equals(object?)"/> is overriden, or the reference is going to be compared.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryAddOrUpdate(string key, object? value);
}
