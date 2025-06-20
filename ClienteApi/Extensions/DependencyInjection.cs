using ClienteApi.Data;
using ClienteApi.Repositories;
using ClienteApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ClienteApi.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("ClientesDb"));

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}
