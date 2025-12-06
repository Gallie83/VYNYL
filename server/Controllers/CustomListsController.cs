using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/users/{userId}/lists")]
public class CustomListsController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomListsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomList>>> GetCustomLists(int userId)
    {
        var customLists = await _context.CustomLists
            .Where(cl => cl.UserId == userId)
            .OrderByDescending(cl => cl.CreatedAt)
            .ToListAsync();

        return customLists;
    }

    [HttpPost]
    public async Task<ActionResult<CustomList>> CreateCustomList(
        int userId,
        [FromBody] CreateCustomListDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if(user == null)
        {
            return NotFound();
        }

        var customList = new CustomList
        {
            UserId = userId,
            Name = dto.Name,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        _context.CustomLists.Add(customList);
        await _context.SaveChangesAsync();

        return Ok(customList);
    }

    [HttpPut("{listId}")]
    public async Task<ActionResult<CustomList>> UpdateCustomListName(int userId, int listId, [FromBody] string name)
    {

        if(string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name cannot be empty");
        }

        // Find list and check it belongs to user
        var customList = await _context.CustomLists
            .FirstOrDefaultAsync(cl => cl.Id == listId && cl.UserId == userId);

        if(customList == null)
        {
            return NotFound("List not found or does not belong to user");
        }

        customList.Name = name;

        await _context.SaveChangesAsync();
        return customList;
    }

    [HttpDelete("{listId}")]
    public async Task<ActionResult> DeleteCustomListById(int userId, int listId)
    {
        // Find list and check it belongs to user
        var list = await _context.CustomLists
            .FirstOrDefaultAsync(cl => cl.Id == listId && cl.UserId == userId);

        if(list == null) 
        {
            return NotFound("List not found or does not belong to user");
        }

        _context.CustomLists.Remove(list);
        await _context.SaveChangesAsync();

        return NoContent();
    } 
}