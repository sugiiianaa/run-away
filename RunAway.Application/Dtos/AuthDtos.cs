namespace RunAway.Application.Dtos
{
    // Dtos
    public record RegisterUserDto(string Email, string Password, string Name);
    public record LoginUserDto(string Email, string Password);
    public record TokenDto(string Token, DateTime ExpiresAt);

    // Mapper 
    public static class AuthMapper
    {
        public static TokenDto ToTokenDto(string Token, DateTime ExpiresAt)
        {
            return new TokenDto(Token, ExpiresAt);
        }
    }
}
