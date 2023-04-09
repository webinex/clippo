using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Webinex.Clippo.Ports.Model
{
    /// <summary>
    ///     Model definition, it's used to create EFCore queries and manipulate entities.
    ///
    ///     Note!: All fields except Id might have public getters and setters.
    /// </summary>
    /// <typeparam name="TClip"></typeparam>
    public interface IClippoModelDefinition<TClip>
    {
        Expression<Func<TClip, object>> Id { get; }
        Expression<Func<TClip, string>> OwnerType { get; }
        Expression<Func<TClip, string>> OwnerId { get; }
        Expression<Func<TClip, string>> Directory { get; }
        Expression<Func<TClip, bool>> Active { get; }
        Expression<Func<TClip, int>> SizeBytes { get; }
        Expression<Func<TClip, string>> FileName { get; }
        Expression<Func<TClip, string>> MimeType { get; }
        Expression<Func<TClip, string>> Reference { get; }
        TClip New(IDictionary<string, object> values);
        void SetValues(TClip clip, IDictionary<string, object> values);
    }
}