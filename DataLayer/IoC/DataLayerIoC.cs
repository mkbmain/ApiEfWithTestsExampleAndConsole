using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataLayer.IoC;

public static class DataLayerIoC
{
    public static void Add(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<AuthDataSettings>(configuration.GetSection(nameof(AuthDataSettings)));
        serviceCollection.AddDbContext<ExampleDbContext>((f, options) =>
            {
                options.UseSqlServer(f.GetService<IOptions<AuthDataSettings>>().Value.ConnectionString);
                options.UseLazyLoadingProxies();
            }
        );
    }
}