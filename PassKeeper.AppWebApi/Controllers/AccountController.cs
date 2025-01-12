using Microsoft.AspNetCore.Mvc;
using PassKeeper.Core.Account;

namespace PassKeeper.AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountManager accountManager) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<HttpResult> Register(string username, string password, string email)
    {
        return (await accountManager.RegisterAsync(username, password, email)).Result();
    }
}