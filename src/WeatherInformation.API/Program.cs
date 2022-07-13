using Serilog;
using WeatherInformation.Infrastructure.IoC.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging(l => l.AddSerilog(Log.Logger));
builder.Services.AddSwaggerGen();
builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
