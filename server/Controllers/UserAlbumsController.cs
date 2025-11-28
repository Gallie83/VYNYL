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
    public async Task<ActionResult<List<UserAlbum>>> GetUsersAlbums(
        int userId,
        [FromQuery] AlbumListType listType,
        [FromQuery] int? customListId = null)
    {
        var query = _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.UserId == userId && ua.ListType == listType);

        // If CustomList then ensure listId is provided
        if(listType == AlbumListType.CustomList && customListId.HasValue)
        {
            query = query.Where(ua => ua.CustomListId == customListId.Value);
        }

        var userAlbums = await query.ToListAsync();

        return userAlbums;
    }

    [HttpPost]
    public async Task<ActionResult<UserAlbum>> AddUserAlbum(
        int userId,
        [FromBody] AddUserAlbumDto dto)
    {
        // Check if album is in Albums table
        var album = await _context.Albums
            .FirstOrDefaultAsync(a => a.LastFmId == dto.LastFmId);

        // If not, create
        if(album == null) 
        {
            album = new Album
            {
                LastFmId = dto.LastFmId,
                Title = dto.Title,
                Artist = dto.Artist,
            };

        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        }

        // Check if user already has this album
        var existingUserAlbum = await _context.UserAlbums
            .FirstOrDefaultAsync(ua => ua.UserId == userId 
                                    && ua.AlbumId == album.Id
                                    && ua.ListType == dto.ListType);

        if (existingUserAlbum != null)
        {
            return Conflict("User already has this album");
        }

        // If it's a custom list ensure it has CustomListId
        if (dto.ListType == AlbumListType.CustomList && dto.CustomListId == null)
        {
            return BadRequest("CustomListId is required when ListType is CustomList");
        }

        // Ensure custom list exists and belongs to this user
        if (dto.ListType == AlbumListType.CustomList)
        {
            var customListExists = await _context.CustomLists
                .AnyAsync(cl => cl.Id == dto.CustomListId && cl.UserId == userId);

            if (!customListExists)
            {
                return NotFound("Custom List not found or does not belong to this user");
            }
        }

        var userAlbum = new UserAlbum
        {
            UserId = userId,
            AlbumId = album.Id,
            ListType = dto.ListType,
            CustomListId = dto.CustomListId,
            Rating = dto.Rating,
            DateListened = dto.DateListened
        };

        _context.UserAlbums.Add(userAlbum);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsersAlbums), new { userId }, userAlbum);
    }
}