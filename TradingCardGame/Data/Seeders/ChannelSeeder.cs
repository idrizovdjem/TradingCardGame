using System;
using System.Threading.Tasks;
using TradingCardGame.Services;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TradingCardGame.Data.Seeders
{
    public class ChannelSeeder : ISeeder
    {
        private readonly List<string> channels = new List<string>()
        {
            "Global Channel"
        };

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var channelService = serviceProvider.GetRequiredService<IChannelService>();
            foreach (var channelName in channels)
            {
                await SeedRoleAsync(channelService, channelName);
            }
        }

        public static async Task SeedRoleAsync(IChannelService channelService, string channelName)
        {
            var channelExists = channelService.Exists(channelName);
            if (!channelExists)
            {
                await channelService.CreateAsync(channelName, null);
            }
        }
    }
}
