using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using TradingCardGame.Data.Models;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Data.Seeders
{
    public class ChannelSeeder : ISeeder
    {
        private readonly List<Channel> channels = new List<Channel>()
        {
            new Channel()
            {
                Name = "Global Channel",
                Security = Enums.ChannelType.Public,
                IsDeleted = false,
                MaxUsers = int.MaxValue,
            }
        };

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var channelService = serviceProvider.GetRequiredService<IChannelService>();
            foreach (var channel in channels)
            {
                await SeedRoleAsync(channelService, channel);
            }
        }

        public static async Task SeedRoleAsync(IChannelService channelService, Channel channel)
        {
            var channelExists = channelService.Exists(channel.Name);
            if (!channelExists)
            {
                await channelService.CreateAsync(channel);
            }
        }
    }
}
