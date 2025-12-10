using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AlbumsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get specific album instance by Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbumById(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if(album == null) 
        {
            return NotFound();
        }

        return album;
    }
}