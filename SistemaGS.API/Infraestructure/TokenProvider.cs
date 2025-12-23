using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using System.Security.Claims;
using System.Text;

namespace SistemaGS.API.Infraestructure
{
    public class TokenProvider
    {
        private readonly IConfiguration _configuration;
        public TokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Token GenerateToken(UsuarioDTO userAccount)
        {
            string accessToken = GenerateAccessToken(userAccount);
            RefreshToken refreshToken = GenerateRefreshToken();
            return new Token 
            { 
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.Now.AddDays(7),
                CreatedDate = DateTime.Now,
                Enabled = true
            };

            return refreshToken;
        } 
        private string GenerateAccessToken(UsuarioDTO userAccount)
        {
            string secretKey = _configuration["JWT:SecretKey"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, userAccount.Cedula.ToString()!),
                    new Claim(ClaimTypes.Name, userAccount.NombreUsuario),
                    new Claim(ClaimTypes.Role, userAccount.Rol.Nombre),
                    new Claim(ClaimTypes.Email, userAccount.Correo)
                    ]),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };
            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
    public class Token
    {
        public string AccessToken { get; set; } = "";

        public RefreshToken RefreshToken { get; set; }
    }
}
