namespace SistemaGS.DTO.Email
{
    public class EmailDTO
    {
        public string? From { get; set; }
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
    public class SMTPhost
    {
        public string host { get; set; } = null!;
        public int port { get; set; }
        public Auth auth { get; set; } = null!;
    }
    public class Auth
    {
        public string user { get; set; } = null!;
        public string pass { get; set; } = null!;
    }
}
