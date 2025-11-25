public class UserAlbum
{
    // Foreign Keys
    public User User { get; set; }
    public Album Album { get; set; }
    // Navigation Properties
    public int UserId { get; set; }
    public int AlbumId { get; set; }

    public float Rating { get; set; }
    public DateOnly DateListened { get; set; }
}