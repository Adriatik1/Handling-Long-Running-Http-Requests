using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var rabbitMQHostAddress = builder.Configuration.GetValue<string>("RabbitMQHostAddress");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configs =>
{
    configs.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMQHostAddress);
        cfg.ConfigureEndpoints(context);
    });

});

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
