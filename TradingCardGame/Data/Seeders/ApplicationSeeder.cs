using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
