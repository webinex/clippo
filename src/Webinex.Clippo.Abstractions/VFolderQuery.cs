using Webinex.Asky;

namespace Webinex.Clippo;

public class VFolderQuery
{
    public FilterRule? FilterRule { get; }
    public SortRule? SortRule { get; }
    public PagingRule? PagingRule { get; }

    public VFolderQuery(
        FilterRule? filterRule = null,
        SortRule? sortRule = null,
        PagingRule? pagingRule = null)
    {
        FilterRule = filterRule;
        SortRule = sortRule;
        PagingRule = pagingRule;
    }
}