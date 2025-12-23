namespace SistemaGS.DTO.AuthDTO
{
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Expires { get; set; }
        public bool Enabled {  get; set; }
        public int Cedula { get; set; }
    }
}
