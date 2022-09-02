using MockQueryable.Moq;
using Moq;
using Xunit;

namespace SimpleRepo.Tests.UnitTests;

public class AddTests
{
    [Fact]
    public async Task Ensure_we_can_add()
    {
        var user = new User{Email = "whatEver"};
        var (sut, mockContext) = RepoFactory.Get();
        var users = new List<User>().AsQueryable().BuildMockDbSet();

        mockContext.Setup(t => t.Set<User>()).Returns(users.Object);
        await sut.Add(user);
        
        // mocks we verify :) 
        users.Verify(t=> t.Add(user),Times.Once);
        mockContext.Verify(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
