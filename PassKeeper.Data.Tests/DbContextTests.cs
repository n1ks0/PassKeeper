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
        var usersQuery = dbContext.GetAll<User>();
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
        mockedDbContext.ChangeTracker.Clear();
        await using var dbContext = await CreateFakeDbContextFactory(mockedDbContext).CreateDbContextAsync();
        
        //Act
        await dbContext.DeleteAsync<User>(user.Id);
        var deletedCount = await dbContext.SaveChangesAsync();
        
        //Assert
        Assert.That(deletedCount, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateAsync_UpdateEntityProperty_PropertyUpdated()
    {
        //Arrange
        var mockedDbContext = CreateMockedDbContext();
        var user = new User("Test", "Test", "Test", UserRole.User);
        await mockedDbContext.Users.AddAsync(user);
        await mockedDbContext.SaveChangesAsync();
        mockedDbContext.ChangeTracker.Clear();
        await using var dbContext = await CreateFakeDbContextFactory(mockedDbContext).CreateDbContextAsync();
        const string newValue = "Updated";
        
        //Act
        var userToUpdate = new User("Test", "Test", "Test", UserRole.User)
        {
            Id = user.Id,
            Name = newValue
        };
        await dbContext.UpdateAsync(userToUpdate);
        var updatedCount = await dbContext.SaveChangesAsync();
        var usersQuery = dbContext.GetAll<User>();
        user = await dbContext.FirstOrDefaultAsync(usersQuery);
        
        //Assert
        Assert.That(updatedCount, Is.EqualTo(1));
        Assert.That(user.Name, Is.EqualTo(newValue));
    }

    [Test]
    public async Task GetByIdAsync_RetrieveSingleEntityById_RetrievedSingleEntity()
    {
        //Arrange
        var mockedDbContext = CreateMockedDbContext();
        var user = new User("Test", "Test", "Test", UserRole.User);
        await mockedDbContext.Users.AddAsync(user);
        await mockedDbContext.SaveChangesAsync();
        mockedDbContext.ChangeTracker.Clear();
        await using var dbContext = await CreateFakeDbContextFactory(mockedDbContext).CreateDbContextAsync();
        
        //Act
        var retrievedUser = await dbContext.GetByIdAsync<User>(user.Id);
        Assert.That(retrievedUser.Id, Is.EqualTo(user.Id));
    }

    [Test]
    public async Task FindByCondition_RetrieveUserWithSpecificEmail_UserRetrieved()
    {
        //Arrange
        const string specificEmail = "test@test.com";
        var mockedDbContext = CreateMockedDbContext();
        await mockedDbContext.Users.AddAsync(new User("Test", specificEmail, "Test", UserRole.User));
        await mockedDbContext.SaveChangesAsync();
        mockedDbContext.ChangeTracker.Clear();
        
        //Act
        await using var dbContext = await CreateFakeDbContextFactory(mockedDbContext).CreateDbContextAsync();
        var userQuery = dbContext.FindByCondition<User>(u => u.Email == specificEmail);
        var user = await dbContext.FirstOrDefaultAsync(userQuery);
        
        //Assert
        Assert.That(user.Email, Is.EqualTo(specificEmail));
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