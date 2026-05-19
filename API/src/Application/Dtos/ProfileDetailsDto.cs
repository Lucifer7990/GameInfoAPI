public class ProfileDetailsDto
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    // Profile Customization
    public string AvatarUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    // User Statistics
    public int GamesReviewedCount { get; set; } = 0;
    public int FollowersCount { get; set; } = 0;

}

