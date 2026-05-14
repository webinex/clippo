using System;
using System.Linq;

namespace Webinex.Clippo;

internal static class VFolderStateExtensions
{
    public static VRow<TMeta, TData>[] ToRows<TMeta, TData>(this VFolderState<TData> state, TMeta meta)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var folderRow = VRow<TMeta, TData>.NewFolder(state.Id, state.Type, state.Path?.Value, meta);
        var fileRows = state.Files.Select(x => x.ToRow(folderRow.Folder, folderRow.Path, meta)).ToArray();

        return new[] { folderRow }.Concat(fileRows).ToArray();
    }
}