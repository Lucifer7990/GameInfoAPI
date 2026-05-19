using System.ComponentModel.DataAnnotations;

public class GameDetailDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [MinLength(1)]
    [Url]
    public string CoverImageUrl { get; set; } = string.Empty;
}