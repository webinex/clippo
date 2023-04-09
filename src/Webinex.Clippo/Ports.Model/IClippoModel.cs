using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.Ports.Model
{
    /// <summary>
    ///     Clippo model service. If you use <see cref="IClippoModelDefinition{TClip}"/> than
    ///     <see cref="IClippoModel{TClip}"/> would be registered based on <see cref="IClippoModelDefinition{TClip}"/>.
    ///     But you can still override it.
    ///
    ///     If you don't use EFCore, you can skip <see cref="IClippoModelDefinition{TClip}"/> and provide only
    ///     implementation for <see cref="IClippoModel{TClip}"/>
    /// </summary>
    /// <typeparam name="TClip"></typeparam>
    public interface IClippoModel<TClip>
    {
        TClip New(IDictionary<string, object> values);
        void SetValues(TClip clip, IDictionary<string, object> values);

        string GetId([NotNull] TClip clip);
        string GetOwnerType([NotNull] TClip clip);
        void SetOwnerType([NotNull] TClip clip, [MaybeNull] string ownerType);
        string GetOwnerId([NotNull] TClip clip);
        void SetOwnerId([NotNull] TClip clip, [MaybeNull] string ownerId);
        string GetDirectory([NotNull] TClip clip);
        void SetDirectory([NotNull] TClip clip, [MaybeNull] string directory);
        bool GetActive([NotNull] TClip clip);
        void SetActive([NotNull] TClip clip, bool active);
        int GetSizeBytes([NotNull] TClip clip);
        void SetSizeBytes([NotNull] TClip clip, int sizeBytes);
        string GetFileName([NotNull] TClip clip);
        void SetFileName([NotNull] TClip clip, [MaybeNull] string fileName);
        string GetMimeType([NotNull] TClip clip);
        void SetMimeType([NotNull] TClip clip, [MaybeNull] string mimeType);
        string GetRef([NotNull] TClip clip);
        void SetRef([NotNull] TClip clip, [MaybeNull] string reference);
    }
}