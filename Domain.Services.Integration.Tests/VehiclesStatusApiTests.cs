using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts;
using Data.Infrastructure.MSSQL;
using Domain.Services;
using MachinesMonitor.Controllers;
using NUnit.Framework;

namespace MachinesMonitor.Integration.Tests
{
    [TestFixture]
    internal class VehiclesStatusApiTests : ApiIntegrationTestsBase
    {
        [Test]
        public async Task TestVehicleGetsConnectedAfterPing()
        {
            var vehiclesDataIntoDB = TestData.GetVehicles();

            using (var dbContext = new MssqlContext())
            {
                dbContext.Vehicles.AddRange(vehiclesDataIntoDB);
                await dbContext.SaveChangesAsync();
            }

            var vehicleMonitoringService = new VehicleMonitoringService(new VehiclesRepository(), TimeSpan.FromMilliseconds(50));
            await vehicleMonitoringService.Init();
            var apiController = new VehiclesStatusController(vehicleMonitoringService);

            var vehiclesStatusInfo = await apiController.Monitoring();

            Assert.IsTrue(vehiclesStatusInfo.Count() == vehiclesDataIntoDB.Count(), "Count of vehicles in db and returned from api does not match");
        }
    }
}
