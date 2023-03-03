using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DataLayer.IoC;

public static class DataLayerIoC
{
    public static void Add(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<CustomerOrdersDataSettings>(configuration.GetSection(nameof(CustomerOrdersDataSettings)));
        serviceCollection.AddDbContext<CustomerOrdersDbContext>((f, options) =>
            {
                options.UseSqlServer(f.GetService<IOptions<CustomerOrdersDataSettings>>().Value.ConnectionString);
                options.UseLazyLoadingProxies();
            }
        );
    }
}