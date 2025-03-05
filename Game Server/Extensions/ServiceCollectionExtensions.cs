using Microsoft.Extensions.DependencyInjection;
using TicTacToeGameServer.Configurations;
using TicTacToeGameServer.Models;
using TicTacToeGameServer.Handlers;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Interfaces.WebSocketInterfaces;
using TicTacToeGameServer.Managers;
using TicTacToeGameServer.Services;
using TicTacToeGameServer.Services.ClientRequests;
using TicTacToeGameServer.Services.HostedServices;
using TicTacToeGameServer.Services.Redis;
using TicTacToeGameServer.Services.Requests;

namespace TicTacToeGameServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddConfiguration()
                .AddSingleton<IMatchWebSocketService, MatchWebSocketService>()
                .AddSingleton<ICloseRequest, CloseRequest>()
                .AddSingleton<IConnectionRequest, ConnectionRequest>()
                .AddSingleton<IRedisBaseService, RedisBaseService>()
                .AddSingleton<IRatingRedisService, RatingRedisService>()
                .AddSingleton<IGamesOpenedAmountRedisService, GamesOpenedAmountRedisService>()
                .AddSingleton<IGamesActiveRedisService, GamesActiveRedisService>()
                .AddSingleton<IProcessRequest, ProcessRequest>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IReadyToPlayService, ReadyToPlayService>()
                .AddSingleton<ICreateRoomService, CreateRoomService>()
                .AddSingleton<IMatchIdRedisService, MatchIdRedisService>()
                .AddSingleton<IRandomizerService, RandomizerService>()
                .AddSingleton<IDateTimeService, DateTimeService>()
                .AddSingleton<ISendMoveRequest, SendMoveRequest>()
                .AddSingleton<IStopGameRequest, StopGameRequest>()
                .AddTransient<ConnectionHandler>()
                .AddSingleton<SessionManager>()
                .AddSingleton<SearchingManager>()
                .AddSingleton<MatchingManager>()
                .AddSingleton<IdToUserIdManager>()
                .AddSingleton<RoomsManager>()
                .AddSingleton<LocalDataManager>()
                .AddSingleton<LocalGameServerData>()
                .AddHostedService<MatchMakingHostedService>()
                .AddHostedService<MatchWebSocketHostedService>();
        }

        private static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection
           .AddOptions<WebSocketSharpConfiguration>()
           .BindConfiguration("WebSocketSharp")
           .ValidateDataAnnotations()
           .ValidateOnStart();

            return serviceCollection;
        }
    }
}
