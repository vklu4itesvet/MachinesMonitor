using System.Collections.Generic;

namespace Data.Contracts
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
