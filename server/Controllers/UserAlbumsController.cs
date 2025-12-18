using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserAlbumsController : AuthenticatedControllerBase
{

    public UserAlbumsController(ApplicationDbContext context) : base(context)
    {

    }

    // Get all user's albums
    [HttpGet]
    public async Task<ActionResult<List<UserAlbum>>> GetUsersAlbums(
        [FromQuery] AlbumListType listType,
        [FromQuery] int customListId = 0)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

        var query = _context.UserAlbums
            .Include(ua => ua.Album)
            .Where(ua => ua.UserId == user.Id 
                && ua.ListType == listType);

        // If CustomList then ensure listId is provided
        if(listType == AlbumListType.CustomList)
        {
            if(customListId <= 0) 
            {
                return BadRequest("Invalid CustomListId");
            }
            query = query.Where(ua => ua.CustomListId == customListId);
        }
        else
        {
            query = query.Where(ua => ua.CustomListId == 0);
        }

        var userAlbums = await query.ToListAsync();

        return userAlbums;
    }

    // Add album to user's list
    [HttpPost]
    public async Task<ActionResult<UserAlbum>> AddUserAlbum(AddUserAlbumDto dto)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");

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
            .FirstOrDefaultAsync(ua => ua.UserId == user.Id 
                                    && ua.AlbumId == album.Id
                                    && ua.ListType == dto.ListType);

        if (existingUserAlbum != null)
        {
            return Conflict("User's list already has this album");
        }

        // If it's a custom list ensure it has CustomListId
        if (dto.ListType == AlbumListType.CustomList && dto.CustomListId <= 0)
        {
            return BadRequest("CustomListId is required when ListType is CustomList");
        }

        // Ensure custom list exists and belongs to this user
        if (dto.ListType == AlbumListType.CustomList && dto.CustomListId > 0)
        {
            var customListExists = await _context.CustomLists
                .AnyAsync(cl => cl.Id == dto.CustomListId && cl.UserId == user.Id);

            if (!customListExists)
            {
                return NotFound("Custom List not found or does not belong to this user");
            }
        }

        if(dto.ListType != AlbumListType.CustomList && dto.CustomListId != 0)
        {
            return BadRequest("CustomListId must be 0 for non-CustomList types");
        }

        var userAlbum = new UserAlbum
        {
            UserId = user.Id,
            AlbumId = album.Id,
            ListType = dto.ListType,
            CustomListId = dto.CustomListId,
            Rating = dto.Rating,
            DateListened = dto.DateListened
        };

        _context.UserAlbums.Add(userAlbum);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsersAlbums), null, userAlbum);
    }

    // Remove album from user's list
    [HttpDelete("{albumId}")]
    public async Task<ActionResult> RemoveAlbumFromList(
        int albumId,
        [FromQuery] AlbumListType listType,
        [FromQuery] int customListId = 0)
    {
        // Validate current cognito user
        var user = await GetAuthenticatedUserAsync();
        if (user == null) return Unauthorized("Invalid token or user not found");
        
        // Validation for CustomList
        if (listType == AlbumListType.CustomList)
        {
            if (customListId <= 0)
            {
                return BadRequest("Invalid customListId");
            }
        }
        else if (customListId != 0)
        {
            // If it's not CustomList but customListId was provided
            return BadRequest("customListId must be 0 for non-CustomList types");
        }

        // Find the specific UserAlbum
        var userAlbum = await _context.UserAlbums
            .FirstOrDefaultAsync(ua => ua.UserId == user.Id 
                                    && ua.AlbumId == albumId
                                    && ua.ListType == listType
                                    && ua.CustomListId == customListId);

        if (userAlbum == null)
        {
            return NotFound("Album not found in the specified list");
        }

        _context.UserAlbums.Remove(userAlbum);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}