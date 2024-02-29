using System;
using System.Linq.Expressions;
using Webinex.Asky;

namespace Webinex.Clippo;

internal class VFolderVRowAskyFieldMap<TMeta, TData> : IAskyFieldMap<VRow<TMeta, TData>>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public Expression<Func<VRow<TMeta, TData>, object>>? this[string fieldId] => fieldId switch
    {
        "_id" => x => x.Id,
        "id" => x => x.Folder.Id,
        "type" => x => x.Folder.Type,
        _ => null,
    };
}