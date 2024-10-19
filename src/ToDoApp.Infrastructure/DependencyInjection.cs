using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Core.Interfaces;
using ToDoApp.Infrastructure.Persistence;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IToDoRepository, ToDoRepository>();
        
        return services;
    }
}