using MockQueryable.Moq;
using Shouldly;
using Xunit;

namespace SimpleRepo.Tests.UnitTests;

public class GetAllTests
{
    [Fact]
    public async Task Ensure_we_GetAll()
    {
        var user = new User {Email = "whatEver"};
        var user2 = new User {Email = "whatEver2"};
        var (sut, mockContext) = RepoFactory.Get();

        mockContext.Setup(t => t.Set<User>()).Returns(new[] {user, user2}.AsQueryable().BuildMockDbSet().Object);
        var users = await sut.GetAll<User>();

        // we ensure we got back what we expected 
        users.Count.ShouldBe(2);
        users.ShouldContain(user);
        users.ShouldContain(user2);
    }

    [Fact]
    public async Task Ensure_we_GetAll_based_on_condition()
    {
        var user = new User {Email = "whatEver"};
        var user2 = new User {Email = "whatEver2"};
        var (sut, mockContext) = RepoFactory.Get();

        mockContext.Setup(t => t.Set<User>()).Returns(new[] {user, user2}.AsQueryable().BuildMockDbSet().Object);
        var users = await sut.GetAll<User>(f => f.Email == user.Email);

        // we ensure we got back what we expected 
        users.Count.ShouldBe(1);
        users.ShouldContain(user);
    }

    [Fact]
    public async Task Ensure_we_GetAll_based_on_condition_and_projections()
    {
        var user = new User {Email = "whatEver"};
        var user2 = new User {Email = "whatEver2"};
        var (sut, mockContext) = RepoFactory.Get();

        mockContext.Setup(t => t.Set<User>()).Returns(new[] {user, user2}.AsQueryable().BuildMockDbSet().Object);
        var users = await sut.GetAll<User, string>(f => f.Email == user.Email, f => f.Email);

        // we ensure we got back what we expected 
        users.Count.ShouldBe(1);
        users.ShouldContain(user.Email);
    }
}