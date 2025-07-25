using System;

namespace Webinex.Clippo;

internal static class VFileValueExtensions
{
    public static VRow<TMeta, TData> ToRow<TMeta, TData>(
        this IVFileValue<TData> fileValue,
        VFolderId vFolderId,
        TMeta meta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        return VRow<TMeta, TData>.NewFile(
            vFolderId,
            fileValue.Path,
            fileValue.Name,
            fileValue.MimeType,
            fileValue.Bytes,
            fileValue.Ref,
            meta,
            fileValue.Data);
    }
}