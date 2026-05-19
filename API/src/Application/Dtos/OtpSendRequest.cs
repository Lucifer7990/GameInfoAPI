using System.ComponentModel.DataAnnotations;


public class OtpSendRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

}
