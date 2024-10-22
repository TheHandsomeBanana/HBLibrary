using System;
using System.Collections.Generic;
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
}
