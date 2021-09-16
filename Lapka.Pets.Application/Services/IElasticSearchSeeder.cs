using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IElasticSearchSeeder
    {
        Task SeedShelterPets();
    }
}