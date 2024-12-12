using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using LetsMeet.Application.User.Commands.RegisterUser;
using Microsoft.Extensions.DependencyInjection;

namespace LetsMeet.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);
        services.AddFluentValidationAutoValidation();

        return services;
    }
}