using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TradingCardGame.Data.Seeders
{
    public class ApplicationSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var seeders = new List<ISeeder>()
            {
                new RoleSeeder(),
                new ChannelSeeder()
            };

            foreach(var seeder in seeders)
            {
                await seeder.SeedAsync(context, serviceProvider);
            }
        }
    }
}
