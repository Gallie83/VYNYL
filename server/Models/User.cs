public class User 
{
    public int Id { get; set; }
    public required string? CognitoId { get; set; }
    public required string? Username { get; set; }
    public required string? Email { get; set; }

    public ICollection<UserAlbum> UserAlbums { get; set; } = new List<UserAlbum>();
}