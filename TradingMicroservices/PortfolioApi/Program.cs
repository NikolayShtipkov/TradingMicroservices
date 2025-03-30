using MassTransit;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Consumers;
using PortfolioApi.Data;
using PortfolioApi.Services;
using PortfolioApi.Services.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // The consumer will listen for the "order-created" event
        cfg.ReceiveEndpoint("order-queue", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context);
        });

        // Add logging
        cfg.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddTransient<IPortfolioService, PortfolioService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    dbContext.Database.Migrate();
}

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
