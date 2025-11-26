using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/users/{userId}/albums")]
public class UserAlbumsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserAlbumsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserAlbum>>> GetUsersAlbums(int userId)
    {
        var userAlbums = await _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.UserId == userId)
            .ToListAsync();

        return userAlbums;
    }
}