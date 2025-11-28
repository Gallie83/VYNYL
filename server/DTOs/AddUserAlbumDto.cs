using System.ComponentModel.DataAnnotations;

public class AddUserAlbumDto
{
    [Required]
    public string? LastFmId { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Artist { get; set; }

    [Required]
    public AlbumListType ListType { get; set; }

    public int? CustomListId { get; set; }

    public float? Rating { get; set; }
    public DateOnly? DateListened { get; set; }
}