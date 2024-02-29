using System;
using System.Linq;

namespace Webinex.Clippo;

internal static class VFolderStateExtensions
{
    public static VRow<TMeta, TData>[] ToRows<TMeta, TData>(this VFolderState<TData> state, TMeta meta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var folderRow = VRow<TMeta, TData>.NewFolder(state.Id, state.Type, meta);
        var fileRows = state.Files.Select(x => x.ToRow(folderRow.Folder, meta)).ToArray();

        return new[] { folderRow }.Concat(fileRows).ToArray();
    }
}