using System;
using System.Threading.Tasks;
using Data.Infrastructure.MSSQL;
using NUnit.Framework;

namespace MachinesMonitor.Integration.Tests
{
    [SetUpFixture]
    internal abstract class ApiIntegrationTestsBase
    {
        private string _connectionString;

        protected string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MachinesMonitor_{Guid.NewGuid()};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                }

                return _connectionString;
            }
        }

        [OneTimeSetUp]
        public async Task CreateDB()
        {
            MssqlContext.ConnectionString = ConnectionString;

            using (var context = new MssqlContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }

        [OneTimeTearDown]
        public async Task DropDB()
        {
            using (var context = new MssqlContext())
            {
                await context.Database.EnsureDeletedAsync();
            }
        }
    }
}
