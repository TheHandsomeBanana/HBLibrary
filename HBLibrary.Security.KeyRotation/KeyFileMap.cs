using HBLibrary.Core;
using HBLibrary.Interface.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Security.KeyRotation;
public sealed class KeyFileMap : IEquatable<KeyFileMap> {
    public required IKey Key { get; init; }
    public List<string> Files { get; init; } = [];

    [JsonConstructor]
    public KeyFileMap() {

    }

    public bool Equals(KeyFileMap? other) {
        return Key == other?.Key && Files.SequenceEqual(other.Files);
    }

    public override bool Equals(object? obj) {
        return Equals(obj as KeyFileMap);
    }

    public override int GetHashCode() {
        int filesHashcode = HBHashCode.CombineSequence(Files);
        return HBHashCode.Combine(Key, filesHashcode);
    }
}
