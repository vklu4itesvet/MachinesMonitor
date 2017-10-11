using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Data.Infrastructure.MSSQL
{
    public class VehiclesRepository : IVehiclesRepository
    {
        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            using (var dbContext = new MssqlContext())
            {
                return await dbContext.Vehicles.Include(v => v.Owner).ToListAsync();
            }
        }

        public async Task UpdateRange(IEnumerable<Vehicle> vehicles)
        {
            using (var dbContext = new MssqlContext())
            {
                foreach(var v in vehicles)
                {
                    dbContext.Entry(v).State = EntityState.Modified;
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
