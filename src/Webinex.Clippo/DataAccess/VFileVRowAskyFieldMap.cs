using System;
using System.Linq.Expressions;
using Webinex.Asky;

namespace Webinex.Clippo;

internal class VFileVRowAskyFieldMap<TMeta, TData> : IAskyFieldMap<VRow<TMeta, TData>>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    public Expression<Func<VRow<TMeta, TData>, object>>? this[string fieldId] => fieldId switch
    {
        "id" => x => x.Id,
        "folder.id" => x => x.Folder.Id,
        "folder.type" => x => x.Folder.Type,
        _ => null,
    };
}