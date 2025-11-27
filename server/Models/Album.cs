public class Album 
{
    public int Id { get; set; }
    public string? LastFmId { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }

    public ICollection<UserAlbum> UserAlbums { get; set; } = new List<UserAlbum>();
}