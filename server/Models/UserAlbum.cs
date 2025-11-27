public class UserAlbum
{
    // Foreign Keys
    public User User { get; set; } = null!;
    public Album Album { get; set; } = null!;
    // Navigation Properties
    public int UserId { get; set; }
    public int AlbumId { get; set; }

    public float? Rating { get; set; }
    public DateOnly? DateListened { get; set; }
}