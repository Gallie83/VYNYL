using System.ComponentModel.DataAnnotations;

public class CreateCustomListDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
}