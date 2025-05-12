namespace RunAway.Application.Dtos.User
{
    // Response
    public class LoginUserResponseDto
    {
        public required string Token { get; set; }
        public required DateTime ExpiredDate { get; set; }
    }
}
