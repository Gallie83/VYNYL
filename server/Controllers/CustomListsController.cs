using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/users/lists/[controller]")]
[Authorize]
public class CustomListsController: AuthenticatedControllerBase
{

    public CustomListsController(ApplicationDbContext context) : base(context)
    {

    }

    // Get list of all user's CustomLists
    [HttpGet]
    public async Task<ActionResult<List<CustomList>>> GetCustomLists()
    {

        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        var customLists = await _context.CustomLists
            .Where(cl => cl.UserId == user.Id)
            .OrderByDescending(cl => cl.CreatedAt)
            .ToListAsync();

        return customLists;
    }

    // Create new CustomList for user
    [HttpPost]
    public async Task<ActionResult<CustomList>> CreateCustomList(CreateCustomListDto dto)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        var customList = new CustomList
        {
            UserId = user.Id,
            Name = dto.Name,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        _context.CustomLists.Add(customList);
        await _context.SaveChangesAsync();

        return Ok(customList);
    }

    // Update user's CustomList name
    [HttpPut("{listId}")]
    public async Task<ActionResult<CustomList>> UpdateCustomListName(int listId, [FromBody] string name)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        if(string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name cannot be empty");
        }

        // Find list and check it belongs to user
        var customList = await _context.CustomLists
            .FirstOrDefaultAsync(cl => cl.Id == listId && cl.UserId == user.Id);

        if(customList == null)
        {
            return NotFound("List not found or does not belong to user");
        }

        customList.Name = name;

        await _context.SaveChangesAsync();
        return customList;
    }

    // Delete CustomList
    [HttpDelete("{listId}")]
    public async Task<ActionResult> DeleteCustomListById(int listId)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        // Find list and check it belongs to user
        var list = await _context.CustomLists
            .FirstOrDefaultAsync(cl => cl.Id == listId && cl.UserId == user.Id);

        if(list == null) 
        {
            return NotFound("List not found or does not belong to user");
        }

        _context.CustomLists.Remove(list);
        await _context.SaveChangesAsync();

        return NoContent();
    } 
}