using System;
using System.Threading.Tasks;

namespace TradingCardGame.Data.Seeders
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider);
    }
}
