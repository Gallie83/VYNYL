using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : AuthenticatedControllerBase
{
    
    public UsersController(ApplicationDbContext context) : base(context)
    {
        
    }

    // Get current authenticated user
    // GET: api/users/me
    [HttpGet("me")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        return Ok(user);
    }

    // Add new user
    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var cognitoId = AuthHelper.GetCognitoIdFromClaims(User);

        if(string.IsNullOrEmpty(cognitoId))
        {
            return Unauthorized("Invalid token");
        }

        if(user.CognitoId != cognitoId)
        {
            return Forbid("Cannot create user for different CognitoId");
        }

        // Check if user already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.CognitoId == cognitoId);

        if(existingUser != null)
        {
            return Conflict("User already exists");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCurrentUser), new { }, user);
    }

    // Search for user by username
    // GET: api/users/profile/{username}
    [HttpGet("/profile{username}")]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user == null ?  NotFound("User not found") : Ok(user);
    }

    // Update current user
    // PUT: api/users/me
    [HttpPut("me")]
    public async Task<ActionResult<User>> UpdateCurrentUser(User updatedUser)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        user.Username = updatedUser.Username;
        user.Email = updatedUser.Email;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Error updating user");
        }

        return NoContent();
    }

    // Delete current user
    // DELETE: api/users/me
    [HttpDelete("me")]
    public async Task<ActionResult> DeleteCurrentUser()
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}