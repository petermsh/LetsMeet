using LetsMeet;
using LetsMeet.Application;
using LetsMeet.Application.Common.Interfaces;
using LetsMeet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

app.UseInfrastructure();

app.Run();