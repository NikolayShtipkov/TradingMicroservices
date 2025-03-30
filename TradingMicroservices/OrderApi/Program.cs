using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Services;
using OrderApi.Services.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseMessageRetry(r =>
        {
            r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        });

        cfg.Message<OrderMessage>(e =>
        {
            e.SetEntityName("order-exchange");
        });

        cfg.Publish<OrderMessage>(e =>
        {
            e.ExchangeType = "direct";
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
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
