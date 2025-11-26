public class AddUserAlbumDto
{
    public string LastFmId { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public float Rating { get; set; }
    public DateOnly DateListened { get; set; }
}