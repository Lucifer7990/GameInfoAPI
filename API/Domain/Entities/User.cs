
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    // Profile Customization [4]
    public string AvatarUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    // User Statistics [4]
    public int GamesReviewedCount { get; set; } = 0;
    public int FollowersCount { get; set; } = 0;

    // OTP Tracking (Note: In production, OTPs are often stored in a fast cache like Redis rather than the main database)
    public string CurrentOtp { get; set; } = string.Empty;
    public DateTime OtpExpiryTime { get; set; }

    public bool IsActive { get; set; } = false;
}

