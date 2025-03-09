using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToeGameServer.Extensions;
using TicTacToeGameServer.Interfaces.WebSocketInterfaces;
using TicTacToeGameServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Load WebSocketSharp settings from configuration
var webSocketConfig = builder.Configuration.GetSection("WebSocketSharp");
int webSocketPort = webSocketConfig.GetValue<int>("Port");
string webSocketPath = webSocketConfig.GetValue<string>("Path");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddService(); // Register custom services

// Register MatchWebSocketService
// builder.Services.AddSingleton<IMatchWebSocketService, MatchWebSocketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
