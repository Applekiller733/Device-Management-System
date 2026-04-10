using Device_Management_System_Backend.Helpers;
using Device_Management_System_Backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
