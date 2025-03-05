using TicTacToeLobbyServer.Interfaces;
using TicTacToeLobbyServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeLobbyServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IRedisBaseService, RedisBaseService>()
                .AddSingleton<IPlayersRedisService, PlayersRedisService>()
                .AddSingleton<ILoginTimeRedisService, LoginTimeRedisService>()
                .AddSingleton<IRatingRedisService, RatingRedisService>()
                .AddSingleton<IDiamondsRedisService, DiamondsRedisService>();
        }
    }
}
