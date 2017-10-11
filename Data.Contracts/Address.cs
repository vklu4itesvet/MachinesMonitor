using System.Collections.Generic;

namespace Data.Contracts
{
    public class Address
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int HouseNum { get; set; }

        public int PostCode { get; set; }

        public IEnumerable<Customer> Customers { get; set; }
    }
}
