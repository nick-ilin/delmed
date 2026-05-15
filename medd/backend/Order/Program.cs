using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using Order.Infrastructure.Messaging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MedicineAddedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Environment.IsDevelopment()
            ? "rabbitmq_medd"
            : "rabbitmq_medd";

        cfg.Host(host, h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("medicine-added-queue", e =>
        {
            e.ConfigureConsumer<MedicineAddedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

// CORS — используем правильное имя политики
app.UseCors("AllowFrontend");

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();