using System;
using System.Collections.Generic;

namespace Webinex.Clippo;

public class VFolderId : Equatable
{
    public string Type { get; protected init; } = null!;
    public string Id { get; protected init; } = null!;

    public VFolderId(string type, string id)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    protected VFolderId()
    {
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Id;
    }

    public override string ToString()
    {
        return $"{Type}::{Id}";
    }

    public VFolderId Clone() => new(Type, Id);
}