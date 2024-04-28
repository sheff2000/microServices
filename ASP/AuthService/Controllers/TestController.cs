using Microsoft.AspNetCore.Mvc;
using AuthService.Data;
using MongoDB.Driver;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly MongoDbContext _dbContext;

    public TestController(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _dbContext.Users.Find(_ => true).ToListAsync();
        return Ok(users);
    }
}
