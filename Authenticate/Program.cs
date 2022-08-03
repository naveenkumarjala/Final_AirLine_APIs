using Authenticate.DBContext;
using Authenticate.Interfaces;
using Authenticate.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IJWTManagerRepository, JWTManagerRepository>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UsersApplicationDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("FlightBookingDb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
