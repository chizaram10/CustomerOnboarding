using Common.Core.Entities;
using Common.Infrastructure.Data;

namespace CustomerOnboarding.API
{
    public class DbSeeder
    {
        public static void SeedStatesAndLGAs(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.States.Any())
            {
                return;
            }

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

            context.States.AddRange(states);
            context.SaveChanges();
        }
    }
}
