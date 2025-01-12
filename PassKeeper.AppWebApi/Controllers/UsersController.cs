using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassKeeper.Core.Entities;
using PassKeeper.Core.Infrastructure;

namespace PassKeeper.AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IDbContextFactory dbContextFactory) : ControllerBase
{
    [HttpGet("GetAll")]
    public async IAsyncEnumerable<User> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await foreach (var user in context.GetAll<User>().AsAsyncEnumerable().WithCancellation(cancellationToken))
            yield return user;
    }
    
    [HttpGet("GetById")]
    public async Task<Results<Ok<User>, NotFound>> GetById(Guid id)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var user = await context.GetByIdAsync<User>(id);
        return user == null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }
}