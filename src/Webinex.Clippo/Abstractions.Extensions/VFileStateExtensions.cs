using System;

namespace Webinex.Clippo;

internal static class VFileStateExtensions
{
    public static VRow<TMeta, TData> ToRow<TMeta, TData>(
        this VFileState<TData> state,
        VFolderId vFolderId,
        TMeta meta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        return VRow<TMeta, TData>.NewFile(
            vFolderId,
            state.Name,
            state.MimeType,
            state.Bytes,
            state.Ref,
            meta,
            state.Data);
    }
}