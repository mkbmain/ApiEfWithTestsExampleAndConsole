using Moq;
using SimpleRepo.Repo;

namespace SimpleRepo.Tests.UnitTests;

// I would probably use a class context to dynamically reflect all things that it requires in constructor
// and build a generic base for this if I could be bother but for this project 1 step at time
public class RepoFactory
{
    public static (Repo<SimpleDbContext>, Mock<SimpleDbContext>) Get()
    {
        var mock = new Mock<SimpleDbContext>();
        return (new Repo<SimpleDbContext>(mock.Object), mock);
    }
}