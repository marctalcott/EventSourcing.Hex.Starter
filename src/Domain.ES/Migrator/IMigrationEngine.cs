using System.Threading.Tasks;

namespace Domain.ES.Migrator
{
    public interface IMigrationEngine
    {
        void RegisterMigration(IMigrator migrator);

        Task StartAsync(string instanceName);

        Task StopAsync();
    }
}