using System.Linq.Expressions;
using DataLayer;
using Microsoft.Extensions.Logging;
using SimpleRepo.Repo;

namespace ServiceLayer.Service;

public abstract class BaseService<T,T2> : IBaseService<T> where T : class
{
    protected readonly IRepo<CustomerOrdersDbContext> Repo;
    protected readonly ILogger<T2> Logger;

    public BaseService(IRepo<CustomerOrdersDbContext> repo, ILogger<T2> logger)
    {
        Logger = logger;
        Repo = repo;
    }
    
}