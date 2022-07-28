using System.Threading.Tasks;

namespace Domain.ES.Projections
{
    public interface IViewRepository
    {
        Task<IView> LoadViewAsync(string name);
        Task<T> LoadTypedViewAsync<T>(string name);
        Task<bool> SaveViewAsync(string name, IView view);
    }
}