using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IVehiclesRepository
    {
        Task<IEnumerable<Vehicle>> GetAll();

        Task UpdateRange(IEnumerable<Vehicle> vehicles);
    }
}
