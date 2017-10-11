using System;
using System.Globalization;
using Data.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Services
{
    public static class Setup
    {
        public static void AddVehicleMonitoringService(this IServiceCollection services, string pingTimeoutStr)
        {
            var pingTimeout = TimeSpan.Parse(pingTimeoutStr, new CultureInfo("en-US"));

            services.AddSingleton(di =>
                new VehicleMonitoringService(
                    di.GetService<IVehiclesRepository>(), pingTimeout));

            services.AddSingleton<Tuple<Action, Action>>(di =>
            {
                var vehiclesMonitor = di.GetService<VehicleMonitoringService>();
                Action vehicleCheckAction = vehiclesMonitor.RecheckVehiclesStatus;
                Action vehicleStatusPersistAction = vehiclesMonitor.PersistVehiclesStatus;
                var monitorActionsSet = new Tuple<Action, Action>(vehicleCheckAction, vehicleStatusPersistAction);

                vehiclesMonitor.Init().Wait();
                return monitorActionsSet;
            });
        }

    }
}
