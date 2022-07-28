using System.Threading.Tasks;

namespace Utility.CosmosValidator
{
    public interface IValidatorEngine
    {
        void RegisterComparer(IValidator validator);

        Task StartAsync(string instanceName);

        Task StopAsync();
    }
}