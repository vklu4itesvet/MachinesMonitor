using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Data.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace MachinesParkEmulator
{
    class Program
    {
        static string PingMonitorAddress { get; set; }

        static void Main(string[] args)
        {
            SetSettings();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Console.WriteLine("Pings will be sent in a minute");

                    await Task.Delay(60000);
                    var allVehicles = await GetVehicles();

                    Console.WriteLine("Ping");

                    var pingVehicles = GetRandomVehiclesSubSet(allVehicles);
                    var notPingVehicles = allVehicles.Except(pingVehicles);

                    foreach(var vehicle in pingVehicles)
                    {
                        SendPing(vehicle);
                    }

                    foreach (var vehicle in notPingVehicles)
                    {
                        Console.WriteLine($"except {vehicle}");
                    }

                    Console.WriteLine(Environment.NewLine);
                }
            });

            Console.WriteLine("Started (Press Esc to shutdown)");

            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
        }

        static void SetSettings()
        {
            MssqlContext.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MachinesMonitorDb";
            PingMonitorAddress = "http://localhost:56570/vehiclesstatus/PutStatus/?vin={0}";
        }

        static async Task<IEnumerable<string>> GetVehicles() 
        {
            using (var dbContext = new MssqlContext())
            {
                return await (from v in dbContext.Vehicles
                                select v.VIN).ToListAsync();
            }
        }

        static IEnumerable<string> GetRandomVehiclesSubSet(IEnumerable<string> fullSet)
        {
            var randomSubSetLength = Convert.ToInt32(Math.Abs(fullSet.Count() * 0.7));
            var randomGen = new Random();
            var randomSubSet = new List<string>();

            for(var i = 0; i < randomSubSetLength; i++)
            {
                var randomVehicle = fullSet.ElementAt(randomGen.Next(randomSubSetLength));
                randomSubSet.Add(randomVehicle);
            }

            return randomSubSet;
        }

        static void SendPing(string vehicle)
        {
            var client = new System.Net.WebClient();

            try
            { 
            
                var pingVehicleTo = new Uri(string.Format(PingMonitorAddress, vehicle));
                client.UploadDataAsync(pingVehicleTo, "PUT", new byte[0]);
            }
            catch(WebException ex)
            {
                Console.WriteLine($"Failed to send ping for vehicle: {vehicle}");
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
