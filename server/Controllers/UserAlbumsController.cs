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
                LastFmId = Album.LastFmId,
                Title = Album.Title,
                Artist = Album.Artist,
            };

        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        }

        // Check if user already has this album
        var existingUserAlbum = await _context.UserAlbums
        .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AlbumId == album.Id);

        if(existingUserAlbum != null)
        {
            return Conflict("User already has this album");
        }

        var userAlbum = new UserAlbum
        {
            UserId = UserAlbum.UserId;
            AlbumId = UserAlbum.AlbumId;
            Rating = UserAlbum.Rating;
            DateListened = UserAlbum.DateListened;
        }

        _context.UserAlbums.Add(userAlbum);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsersAlbums), new { userId }, userAlbum);
    }
}