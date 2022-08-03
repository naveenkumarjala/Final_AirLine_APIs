using Airline.Booking.DBContext;
using Airline.Booking.Events;
using Airline.Booking.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using ServiceDiscovery.Config;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer{token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
    x.AddRider(rider =>
    {
        rider.AddProducer<BookingEvent>(nameof(BookingEvent));
        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:9092");
        });
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddConsuConfig();
builder.Services.AddDbContext<ApplicationBookDbcontext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("FlightBookingDb")));
builder.Services.AddTransient<IBookingRepository, BookingsRepository>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = "TestKey";
    x.DefaultChallengeScheme = "TestKey";
}).AddJwtBearer("TestKey", o =>
{

    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//var factory = new ConnectionFactory
//{
//    Uri = new Uri("amqp://guest@localhost:5672")
//};
//using var connection = factory.CreateConnection();
//using var channel = connection.CreateModel();
//QueueProducer.Publish(channel);
app.UseConsul();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
