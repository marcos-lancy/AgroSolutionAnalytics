using AgroSolutions.Analytics.Service.Application.Interfaces;
using AgroSolutions.Analytics.Service.Application.Services;
using AgroSolutions.Analytics.Service.Domain.Interfaces;
using AgroSolutions.Analytics.Service.Infra.Contexts;
using AgroSolutions.Analytics.Service.Infra.Repositories;
using AgroSolutions.Analytics.Service.Infra.MongoDb;
using AgroSolutions.Analytics.Service.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

#region database

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseLazyLoadingProxies()
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region MongoDB

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("agrosolutions_sensores");
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddScoped<IMongoDbService, MongoDbService>();

#endregion

#region DIs

builder.Services.AddScoped<IAlertaService, AlertaService>();

/// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

#endregion

#region RabbitMQ / MassTransit

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<DadosSensorConsumer>();

    // Configurar receive endpoint explicitamente
    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["MessageBroker:Host"] ?? "localhost";
        var rabbitMqUsername = builder.Configuration["MessageBroker:Username"] ?? "admin";
        var rabbitMqPassword = builder.Configuration["MessageBroker:Password"] ?? "admin123";

        Log.Information("===============================================");
        Log.Information("Configurando RabbitMQ no Worker:");
        Log.Information("Host: {Host}", rabbitMqHost);
        Log.Information("Username: {Username}", rabbitMqUsername);
        Log.Information("===============================================");

        cfg.Host(rabbitMqHost, 5672, "/", hostConfigurator =>
        {
            hostConfigurator.Username(rabbitMqUsername);
            hostConfigurator.Password(rabbitMqPassword);
        });

        // Configurar receive endpoint para escutar o tipo correto
        cfg.ReceiveEndpoint("dados-sensor-queue", e =>
        {
            Log.Information("===============================================");
            Log.Information("Configurando receive endpoint: dados-sensor-queue");
            Log.Information("Consumer: {Consumer}", typeof(DadosSensorConsumer).FullName);
            Log.Information("===============================================");
            e.ConfigureConsumer<DadosSensorConsumer>(context);
        });
    });
});

#endregion

#region Logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown")
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSerilog();
#endregion

var host = builder.Build();
host.Run();
