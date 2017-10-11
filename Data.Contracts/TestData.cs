using System;
using System.Collections.Generic;

namespace Data.Contracts
{
    public static class TestData
    {
        public static IEnumerable<Vehicle> GetVehicles()
        {
            var address1 = new Address
            {
                City = "Södertälje",
                Street = "Cementvägen",
                HouseNum = 8,
                PostCode = 11111
            };

            var customer1 = new Customer
            {
                Name = "Kalles",
                Surname = "Grustransporter",
                Address = address1
            };

            var vehicle1 = new Vehicle
            {
                VIN = "YS2R4X20005399401",
                RegNum = "ABC123",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer1
            };

            var vehicle2 = new Vehicle
            {
                VIN = "VLUR4X20009093588",
                RegNum = "DEF456",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer1
            };

            var vehicle3 = new Vehicle
            {
                VIN = "VLUR4X20009048066",
                RegNum = "GHI789",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer1
            };

            var address2 = new Address
            {
                City = "StockHolm",
                Street = "Balkvägen",
                HouseNum = 12,
                PostCode = 22222
            };

            var customer2 = new Customer
            {
                Name = "Johans",
                Surname = "Bulk",
                Address = address2
            };

            var vehicle4 = new Vehicle
            {
                VIN = "YS2R4X20005388011",
                RegNum = "JKL012",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer2
            };

            var vehicle5 = new Vehicle
            {
                VIN = "YS2R4X20005387949",
                RegNum = "MNO345",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer2
            };

            var address3 = new Address
            {
                City = "Uppsala",
                Street = "Budgetvägen",
                HouseNum = 1,
                PostCode = 33333
            };

            var customer3 = new Customer
            {
                Name = "Johans",
                Surname = "Bulk",
                Address = address3
            };

            var vehicle6 = new Vehicle
            {
                VIN = "YS2R4X20005387765",
                RegNum = "PQR678",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer3
            };

            var vehicle7 = new Vehicle
            {
                VIN = "YS2R4X20005387055",
                RegNum = "STU901",
                LastPingTime = DateTime.UtcNow.Date,
                Owner = customer3
            };

            return new List<Vehicle>
            {
                vehicle1,
                vehicle2,
                vehicle3,
                vehicle4,
                vehicle5,
                vehicle6,
                vehicle7
            };
        }
    }
}
