using System;
using Data.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Infrastructure.MSSQL
{
    public static class Setup
    {
        public static void AddSqlStorage(this IServiceCollection services, string connectionString)
        {
            MssqlContext.ConnectionString = connectionString;
            services.AddTransient<IVehiclesRepository,VehiclesRepository>();

            using (var context = new MssqlContext())
            {
                if(!context.Database.EnsureCreated())
                {
                    return;
                }

                context.Vehicles.AddRange(TestData.GetVehicles());
                context.SaveChanges();
            }
        }
    }
}
