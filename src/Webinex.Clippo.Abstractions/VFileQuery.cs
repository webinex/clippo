using Webinex.Asky;

namespace Webinex.Clippo;

public class VFileQuery
{
    public FilterRule? FilterRule { get; }
    public SortRule? SortRule { get; }
    public PagingRule? PagingRule { get; }

    public VFileQuery(
        FilterRule? filterRule,
        SortRule? sortRule,
        PagingRule? pagingRule)
    {
        FilterRule = filterRule;
        SortRule = sortRule;
        PagingRule = pagingRule;
    }
}