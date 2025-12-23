namespace SistemaGS.DTO.AuthDTO
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = "";
        public RefreshToken RefreshToken { get; set; }
    }
}
