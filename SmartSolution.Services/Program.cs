using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SmartCalculations.MessageComponents.Consumers;
using SmartCalculations.MessageComponents.SagaComponents;
using SmartCalculations.MessageComponents.StateMachines;
using System.Reflection;

IConfigurationBuilder configBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
    .AddJsonFile($"appsettings.json")
    .AddEnvironmentVariables();

IConfigurationRoot config = configBuilder.Build();
var redisConnectionString = config.GetValue<string>("RedisConnectionString");
var rabbitMQHostAddress = config.GetValue<string>("RabbitMQHostAddress");

Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => 
        {
            services.AddMassTransit(configs =>
            {
                configs.AddConsumer<SmartCalculationConsumer>();

                configs.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQHostAddress);

                    cfg.ConfigureEndpoints(context);
                });

                // Passing a definition allows us to configure 
                configs.AddSagaStateMachine<CalculationStateMachine, CalculationState>(typeof(CalculationStateDefinition))
                   // Redis repository to store state instances. By default, redis runs on localhost.
                   .RedisRepository(r =>
                   {
                       r.DatabaseConfiguration(redisConnectionString);

                       r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                   });
            });
    
        })
        .Build()
        .RunAsync();

ManualResetEvent _quitEvent = new(false);

Console.CancelKeyPress += (sender, eArgs) =>
{
    _quitEvent.Set();
    eArgs.Cancel = true;
};

_quitEvent.WaitOne();