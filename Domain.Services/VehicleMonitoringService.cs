using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts;

namespace Domain.Services
{
    public class VehicleMonitoringService
    {
        #region Fields

        private readonly IVehiclesRepository _vehiclesRepository;

        private readonly ConcurrentDictionary<string, ConnectionStatus> _connectionStatusCache = new ConcurrentDictionary<string, ConnectionStatus>();

        #endregion

        #region Constructor

        public VehicleMonitoringService(IVehiclesRepository vehiclesRepository, TimeSpan pingTimeout)
        {
            if(vehiclesRepository == null)
            {
                throw new ArgumentNullException($"{nameof(vehiclesRepository)} should not be null.");
            }

            if(pingTimeout <= TimeSpan.Zero)
            {
                throw new ArgumentException($"{nameof(pingTimeout)} should be greater then zero.");
            }

            _vehiclesRepository = vehiclesRepository;
            PingTimeout = pingTimeout;
        }

        #endregion

        #region Properties

        public TimeSpan PingTimeout { get; private set;  }

        #endregion

        #region Methods

        public async Task<IEnumerable<VehicleInfo>> GetVehicles()
        {
            try
            {
                return from d in await _vehiclesRepository.GetAll()
                       select new VehicleInfo
                       {
                           IsConnected = CheckIsConnected(d),
                           VIN = d.VIN,
                           RegNum = d.RegNum,
                           CustomerName = $"{d.Owner.Name} {d.Owner.Surname}",
                           CustomerId = d.Owner.Id
                       };
            }
            catch
            {
                //TODO: make proper detailed exception handling
                throw;
            }
        }

        public void PingVehicle(string vin)
        {
            if (!_connectionStatusCache.ContainsKey(vin))
            {
                throw new ArgumentException("Wrong vin code.");
            }

            _connectionStatusCache[vin].LastPingTime = DateTime.UtcNow;
        }

        public async Task Init()
        {
            var vehicles = await _vehiclesRepository.GetAll();

            foreach (var v in vehicles)
            {
                var status = new ConnectionStatus
                {
                    LastPingTime = v.LastPingTime,
                    IsConnected = IsPingTimeStillValid(v.LastPingTime)
                };

                if (!_connectionStatusCache.TryAdd(v.VIN, status))
                {
                    Debug.WriteLine($"Init fatal: {v.VIN} info not loaded into cache");
                }
            }
        }

        public void RecheckVehiclesStatus()
        {
            foreach(var status in _connectionStatusCache.Values)
            {
                status.IsConnected = IsPingTimeStillValid(status.LastPingTime);
            }
        }

        public void PersistVehiclesStatus()
        {
            var vehicles = _vehiclesRepository.GetAll().Result;

            foreach (var v in vehicles)
            {
                var timeInCache = default(ConnectionStatus);

                if (!_connectionStatusCache.TryGetValue(v.VIN, out timeInCache))
                {
                    continue;
                }

                v.LastPingTime = timeInCache.LastPingTime;
            }

            _vehiclesRepository.UpdateRange(vehicles);
        }

        private bool CheckIsConnected(Vehicle vehicle)
        {
            var lastPingTime = _connectionStatusCache[vehicle.VIN].LastPingTime;
            var isActual = IsPingTimeStillValid(lastPingTime);

            return isActual;
        }

        private bool IsPingTimeStillValid(DateTime pingTime) => (DateTime.UtcNow - pingTime) < PingTimeout;

        #endregion

        #region Classes

        private class ConnectionStatus
        {
            public DateTime LastPingTime { get; set; }

            public bool IsConnected { get; set; }
        }

        #endregion
    }
}
