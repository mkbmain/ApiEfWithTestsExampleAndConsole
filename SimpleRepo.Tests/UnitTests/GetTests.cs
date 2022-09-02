using MockQueryable.Moq;
using Shouldly;
using Xunit;

namespace SimpleRepo.Tests.UnitTests;

public class GetTests
{
    [Fact]
    public async Task Ensure_we_Get_based_on_condition_and_projections()
    {
        var user = new User {Id = Guid.NewGuid(), Email = "whatEver"};
        var user2 = new User {Email = "whatEver2"};
        var user3 = new User {Id = Guid.NewGuid(), Email = "whatEver"}; // same but we should still only get one :)
        var (sut, mockContext) = RepoFactory.Get();

        mockContext.Setup(t => t.Set<User>()).Returns(new[] {user, user3, user2}.AsQueryable().BuildMockDbSet().Object);
        var users = await sut.Get<User, Guid>(f => f.Email == user.Email, f => f.Id);

        // we ensure we got back what we expected 
        users.ShouldBe(user.Id);
    }
}