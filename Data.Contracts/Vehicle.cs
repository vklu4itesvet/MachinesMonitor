using System;

namespace Data.Contracts
{
    public class Vehicle
    {
        public string VIN { get; set; }

        public string RegNum { get; set; }

        public DateTime LastPingTime { get; set; }

        public Customer Owner { get; set; }
    }
}
