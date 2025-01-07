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
    public async Task Add_SingleEntity_AddedSingleEntity()
    {
        //Arrange
        await using var dbContext = await CreateFakeDbContextFactory().CreateDbContextAsync();
        
        var userToAdd = new User("Test", "Test", "Test", UserRole.User);
        
        const int expectedResult = 1;
        
        //Act
        await dbContext.AddAsync(userToAdd);
        await dbContext.SaveChangesAsync();
        var usersQuery = dbContext.GetQueryable<User>();
        var users = await dbContext.ToListAsync(usersQuery);
        
        //Assert
        Assert.That(users.Count, Is.EqualTo(expectedResult));
    }

    private static IDbContextFactory CreateFakeDbContextFactory()
    {
        var options = new DbContextOptionsBuilder<PostgresContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContextToMock = new PostgresContext(options);
        var mockedDbContext = new MockedDbContextBuilder<PostgresContext>()
            .UseDbContext(dbContextToMock)
            .UseConstructorWithParameters(options)
            .MockedDbContext;
        var fakeContextFactory = new Mock<IDbContextFactory>();
        fakeContextFactory.Setup(x => x.CreateDbContextAsync())
            .ReturnsAsync(mockedDbContext);
        var factory = fakeContextFactory.Object;
        return factory;
    }
}