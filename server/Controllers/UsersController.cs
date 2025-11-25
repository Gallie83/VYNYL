using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        return await _context.Users.ToListAsync(); 
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddNewUser([FromBody] User newUser)
    {
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAllUsers), 
            new { id = newUser.Id }, 
            newUser
            );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ?  NotFound() : user;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUserById(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);

        if(user == null) 
        {
            return NotFound();
        } 

        user.CognitoId = updatedUser.CognitoId;
        user.Username = updatedUser.Username;
        user.Email = updatedUser.Email;

        await _context.SaveChangesAsync();
        return user;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null) 
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}