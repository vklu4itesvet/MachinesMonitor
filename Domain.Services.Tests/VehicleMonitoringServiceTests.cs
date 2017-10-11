using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts;
using Moq;
using NUnit.Framework;

namespace Domain.Services.Tests
{
    [TestFixture]
    internal class VehicleMonitoringServiceTests
    {
        [Test]
        public async Task TestVehicleGetsConnectedAfterPing()
        {
            var vehicleMonitoringService = GetReadyService();
            var vehiclesBeforePing = (await vehicleMonitoringService.GetVehicles()).ToList();
            var vehicleToPing = vehiclesBeforePing.First();

            vehicleMonitoringService.RecheckVehiclesStatus();
            vehicleMonitoringService.PingVehicle(vehicleToPing.VIN);
            vehicleMonitoringService.RecheckVehiclesStatus();

            var vehiclesAfterPing = await vehicleMonitoringService.GetVehicles();

            Assert.IsFalse(vehiclesBeforePing.Any(v => v.IsConnected), "Connected vehicles found before ping");
            Assert.IsTrue(vehiclesAfterPing.First(v => v.IsConnected).VIN == vehicleToPing.VIN, "After ping, target vehicle is not connected");
        }

        [Test]
        public async Task TestVehicleGetsDisconnectedAfterTimeout()
        {
            var vehicleMonitoringService = GetReadyService();
            var vehiclesBeforePing = (await vehicleMonitoringService.GetVehicles()).ToList();

            vehicleMonitoringService.PingVehicle(vehiclesBeforePing.First().VIN);
            await Task.Delay(vehicleMonitoringService.PingTimeout * 2);
            vehicleMonitoringService.RecheckVehiclesStatus();
            var vehiclesAfterTimeout = await vehicleMonitoringService.GetVehicles();

            Assert.IsFalse(vehiclesAfterTimeout.Any(v => v.IsConnected), "Connected vehicles found after timeout");
        }

        [Test]
        public void TestConstructorThrowsOnNullRepo()
        {
            Assert.Throws<ArgumentNullException>(() => new VehicleMonitoringService(null, TimeSpan.MaxValue));
        }

        [Test]
        public void TestConstructorThrowsOnZeroTimeout()
        {
            Assert.Throws<ArgumentException>(() => new VehicleMonitoringService(new Mock<IVehiclesRepository>().Object, TimeSpan.Zero));
        }

        private static VehicleMonitoringService GetReadyService()
        {
            var pingTimeout = TimeSpan.FromMilliseconds(50);
            var repoMock = new Mock<IVehiclesRepository>();

            repoMock.Setup(repo => repo.GetAll()).Returns(() => Task.FromResult(TestData.GetVehicles()));

            var vehicleMonitoringService = new VehicleMonitoringService(repoMock.Object, pingTimeout);
            vehicleMonitoringService.Init().Wait();

            return vehicleMonitoringService;
        }
    }
}
