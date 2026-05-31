using System;

namespace Webinex.Clippo;

internal static class AddVFileArgsExtensions
{
    public static VRow<TMeta, TData> ToRow<TMeta, TData>(
        this AddVFileArgs<TMeta, TData> arg,
        VFolderId vFolderId,
        TMeta defaultMeta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        return arg.State.ToRow(vFolderId, arg.Meta ?? defaultMeta);
    }
}