using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;

internal class LatestTracks {
    private readonly Dictionary<string, object?> capturedTracks = [];

    internal bool AddOrUpdateIfNotEquals(string name, object? value) {
        if (capturedTracks.TryGetValue(name, out object? foundValue)) {
            if (value is null && foundValue is null || (value?.Equals(foundValue) ?? true)) {
                return false;
            }
        }

        capturedTracks[name] = value;
        return true;
    }
}
