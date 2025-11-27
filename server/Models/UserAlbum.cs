public class UserAlbum
{
    // Foreign Keys
    public User User { get; set; } = null!;
    public Album Album { get; set; } = null!;

    // What type of list is it?
    public AlbumListType ListType { get; set; }
    
    public int? CustomListId { get; set; }

    // Navigation Properties
    public int UserId { get; set; }
    public int AlbumId { get; set; }
    public CustomList? CustomList { get; set; }

    // Optional fields
    public float? Rating { get; set; }
    public DateOnly? DateListened { get; set; }
}