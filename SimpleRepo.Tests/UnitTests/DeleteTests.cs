using MockQueryable.Moq;
using Moq;
using Xunit;

namespace SimpleRepo.Tests.UnitTests;

public class DeleteTests
{
    [Fact]
    public async Task Ensure_we_can_delete()
    {
        var user = new User() {Email = "whatEver"};
        var (sut, mockContext) = RepoFactory.Get();
        var users = new[] {user}.AsQueryable().BuildMockDbSet();

        mockContext.Setup(t => t.Set<User>()).Returns(users.Object);
        await sut.Delete(user);

        // mocks we verify :) 
        users.Verify(t => t.Remove(user), Times.Once);
        mockContext.Verify(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}