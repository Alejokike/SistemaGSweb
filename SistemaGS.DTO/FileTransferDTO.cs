namespace SistemaGS.DTO
{
    public class FileTransferDTO
    {
        public string FileName { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string ruta { get; set; } = null!;
        public byte[]? rawdata { get; set; }
	}
}
