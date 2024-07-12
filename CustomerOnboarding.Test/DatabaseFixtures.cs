using Common.Core.Entities;
using Common.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOnboarding.Test
{
    public class DatabaseFixture : IDisposable
    {
        public AppDbContext Context { get; set; }
        public DatabaseFixture()
        {
            //Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

            //var builder = new DbContextOptionsBuilder<AppDbContext>()
            //.UseSqlServer(@"Test database Connection string");


            //Context = new AppDbContext(builder.Options);
            //Context.Database.EnsureDeleted();
            //Context.Database.EnsureCreated();
            //Context.Customers.AddRange(GetCustomers());
            //Context.SaveChanges();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            Context = new AppDbContext(options);

            Seed();
        }

        private void Seed()
        {
            var customers = GetCustomers();
            var states = GetStates();

            Context.States.AddRange(states);
            Context.Customers.AddRange(customers);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private List<Customer> GetCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    PhoneNumber = "123-456-7890",
                    Email = "customer1@example.com",
                    Password = "password1",
                    IsVerified = true,
                    LGAId = 1
                },
                new Customer
                {
                    Id = 2,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    PhoneNumber = "234-567-8901",
                    Email = "customer2@example.com",
                    Password = "password2",
                    IsVerified = false,
                    LGAId = 1
                },
                new Customer
                {
                    Id = 3,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    PhoneNumber = "345-678-9012",
                    Email = "customer3@example.com",
                    Password = "password3",
                    IsVerified = true,
                    LGAId = 1
                },
                new Customer
                {
                    Id = 4,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    PhoneNumber = "456-789-0123",
                    Email = "customer4@example.com",
                    Password = "password4",
                    IsVerified = false,
                    LGAId = 1
                }
            };

            return customers;
        }

        private List<State> GetStates()
        {
            var states = new List<State>
            {
                new State
                {
                    Name = "Bayelsa",
                    LGAs = new List<LGA>
                    {
                        new LGA { Name = "Brass" },
                        new LGA { Name = "Ekeremor" },
                        new LGA { Name = "Kolokuma/Opokuma" },
                        new LGA { Name = "Nembe" },
                        new LGA { Name = "Ogbia" },
                        new LGA { Name = "Sagbama" },
                        new LGA { Name = "Southern Ijaw" },
                        new LGA { Name = "Yenagoa" }
                    }
                },
                new State
                {
                    Name = "Gombe",
                    LGAs = new List<LGA>
                    {
                        new LGA { Name = "Akko" },
                        new LGA { Name = "Balanga" },
                        new LGA { Name = "Billiri" },
                        new LGA { Name = "Dukku" },
                        new LGA { Name = "Funakaye" },
                        new LGA { Name = "Gombe" },
                        new LGA { Name = "Kaltungo" },
                        new LGA { Name = "Kwami" },
                        new LGA { Name = "Nafada" },
                        new LGA { Name = "Shongom" },
                        new LGA { Name = "Yamaltu/Deba" }
                    }
                }
            };

            return states;
        }
    }
}
