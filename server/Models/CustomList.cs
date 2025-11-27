public class CustomList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
    public DateOnly CreatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<UserAlbum> UserAlbums { get; set; } = new List<UserAlbum>();
}