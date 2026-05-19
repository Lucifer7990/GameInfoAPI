using System.ComponentModel.DataAnnotations;


public class OtpVerifyRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string OtpHash { get; set; } = string.Empty;


}