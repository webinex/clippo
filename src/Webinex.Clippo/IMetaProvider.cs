using System.Threading.Tasks;

namespace Webinex.Clippo;

public interface IMetaProvider<TMeta, TData>
{
    Task<TMeta> GetAsync();
}