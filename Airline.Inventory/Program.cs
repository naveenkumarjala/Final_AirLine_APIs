using Airline.Inventory.Consumer;
using Airline.Inventory.DBContext;
using Airline.Inventory.Events;
using Airline.Inventory.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceDiscovery.Config;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
 string GetUniqueName(string eventname)
{
    string hostname = Dns.GetHostName();
    string classAssembly = Assembly.GetCallingAssembly().GetName().Name;
    return $"{hostname}.{classAssembly}.{eventname}";
}
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<QueueConsumer>();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
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
        rider.AddConsumer<BookingEventConsumer>();
        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:9092");
            k.TopicEndpoint<BookingEvent>(nameof(BookingEvent), GetUniqueName(nameof(BookingEvent)), e => {
                e.CheckpointInterval = TimeSpan.FromSeconds(10);
                e.ConfigureConsumer<BookingEventConsumer>(context);
            });
        });
    });
});

builder.Services.AddMassTransitHostedService();

builder.Services.AddConsuConfig();
builder.Services.AddDbContext<InventoryDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("FlightBookingDb")));
builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();

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

app.UseConsul();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
