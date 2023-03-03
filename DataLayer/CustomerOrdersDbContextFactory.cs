using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLayer;

public class CustomerOrdersDbContextFactory : IDesignTimeDbContextFactory<CustomerOrdersDbContext>
{
    public CustomerOrdersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CustomerOrdersDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CustomerOrdersDb;user id=sa;password=A1234567a");

        return new CustomerOrdersDbContext(optionsBuilder.Options);
    }
}