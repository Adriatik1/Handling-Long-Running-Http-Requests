using MassTransit;
using Microsoft.Extensions.Hosting;
using SmartCalculations.MessageComponents.Consumers;
using SmartCalculations.MessageComponents.SagaComponents;
using SmartCalculations.MessageComponents.StateMachines;

Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => 
        {
            services.AddMassTransit(configs =>
            {
                configs.AddConsumer<SmartCalculationConsumer>();

                configs.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });

                // Add Saga State Machines
                const string redisConfigurationString = "127.0.0.1";
                // Passing a definition allows us to configure 
                configs.AddSagaStateMachine<CalculationStateMachine, CalculationState>(typeof(CalculationStateDefinition))
                   // Redis repository to store state instances. By default, redis runs on localhost.
                   .RedisRepository(r =>
                   {
                       r.DatabaseConfiguration(redisConfigurationString);

                       r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                   });
            });
    
        })
        .Build()
        .RunAsync();


Console.ReadKey();