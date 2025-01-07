using EntityFrameworkCore.Testing.Moq.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using PassKeeper.Core.Entities;
using PassKeeper.Core.Infrastructure;
using PassKeeper.Data.Repositories;

namespace PassKeeper.Data.Tests;

public class DbContextTests
{
    [Test]
    public async Task AddAsync_SingleEntity_AddedSingleEntity()
    {
        //Arrange
        await using var dbContext = await CreateFakeDbContextFactory(CreateMockedDbContext()).CreateDbContextAsync();
        
        var userToAdd = new User("Test", "Test", "Test", UserRole.User);
        
        const int expectedResult = 1;
        
        //Act
        await dbContext.AddAsync(userToAdd);
        await dbContext.SaveChangesAsync();
        var usersQuery = dbContext.GetQueryable<User>();
        var users = await dbContext.ToListAsync(usersQuery);
        
        //Assert
        Assert.That(users, Has.Count.EqualTo(expectedResult));
    }

    [Test]
    public async Task DeleteAsync_SingleEntity_RemovedSingleEntity()
    {
        //Arrange
        var mockedDbContext = CreateMockedDbContext();
        var user = new User("Test", "Test", "Test", UserRole.User);
        await mockedDbContext.Users.AddAsync(user);
        await mockedDbContext.SaveChangesAsync();
        var dbContext = await CreateFakeDbContextFactory(mockedDbContext).CreateDbContextAsync();
        
        //Act
        await dbContext.DeleteAsync<User>(user.Id);
        var deletedCount = await dbContext.SaveChangesAsync();
        
        //Assert
        Assert.That(deletedCount, Is.EqualTo(1));
    }

    private static PostgresContext CreateMockedDbContext()
    {
        var options = new DbContextOptionsBuilder<PostgresContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContextToMock = new PostgresContext(options);
        var mockedDbContext = new MockedDbContextBuilder<PostgresContext>()
            .UseDbContext(dbContextToMock)
            .UseConstructorWithParameters(options)
            .MockedDbContext;
        return mockedDbContext;
    }

    private static IDbContextFactory CreateFakeDbContextFactory(PostgresContext postgresContext)
    {
        var fakeContextFactory = new Mock<IDbContextFactory>();
        fakeContextFactory.Setup(x => x.CreateDbContextAsync())
            .ReturnsAsync(postgresContext);
        var factory = fakeContextFactory.Object;
        return factory;
    }
}