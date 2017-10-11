using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MachinesMonitor.Controllers
{
    [Produces("application/json")]
    [Route("VehiclesStatus")]
    public class VehiclesStatusController : Controller
    {
        private readonly VehicleMonitoringService _vehicleService;

        public VehiclesStatusController(VehicleMonitoringService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<VehicleInfo>> Monitoring()
        {
            return await _vehicleService.GetVehicles();
        }

        [HttpPut("[action]")]
        public IActionResult PutStatus(string vin)
        {
            try
            {
                _vehicleService.PingVehicle(vin);
                return Ok();
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}