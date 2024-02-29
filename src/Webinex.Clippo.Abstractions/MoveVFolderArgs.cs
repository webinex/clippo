using System.Collections.Generic;

namespace Webinex.Clippo;

public class MoveVFolderArgs : Equatable
{
    public VFolderId FromId { get; }
    public VFolderId ToId { get; }
    
    public MoveVFolderArgs(VFolderId fromId, VFolderId toId)
    {
        FromId = fromId;
        ToId = toId;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FromId;
        yield return ToId;
    }
}