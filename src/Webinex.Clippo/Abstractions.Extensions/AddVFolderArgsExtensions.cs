using System;
using System.Linq;

namespace Webinex.Clippo;

internal static class AddVFolderArgsExtensions
{
    public static VRow<TMeta, TData>[] ToRows<TMeta, TData>(this AddVFolderArgs<TMeta, TData> arg, TMeta defaultMeta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var folderRow = VRow<TMeta, TData>.NewFolder(arg.Id, arg.Type, defaultMeta);
        var fileRows = arg.Files.Select(x => x.ToRow(folderRow.Folder, defaultMeta)).ToArray();

        return new[] { folderRow }.Concat(fileRows).ToArray();
    }
}