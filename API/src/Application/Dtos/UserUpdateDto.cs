
using System.ComponentModel.DataAnnotations;

public class UserUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string AvatarUrl { get; set; } = string.Empty;

    [Required]
    public string Bio { get; set; } = string.Empty;

}

