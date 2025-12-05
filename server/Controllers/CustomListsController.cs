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
            CreateAt = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.CustomLists.Add(customList);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomList), new {userId, listId = customList.Id}, customList);
    }
}