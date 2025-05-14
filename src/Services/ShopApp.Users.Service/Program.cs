using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShopApp.Users.Service.Database;
using ShopApp.Users.Service.Features.CreateUser;
using ShopApp.Users.Service.Features.DeleteUser;
using ShopApp.Users.Service.Features.GetUserById;
using ShopApp.Users.Service.Features.ListUsers;

var builder = WebApplication.CreateBuilder(args);

//add dbcontext postgres
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateUserEndpoint.Map(app);
GetUserByIdEndpoint.Map(app);
ListUsersEndpoint.Map(app);
DeleteUserEndpoint.Map(app);
app.MapGet("/", () => "Users Service is running!");

app.Run();
