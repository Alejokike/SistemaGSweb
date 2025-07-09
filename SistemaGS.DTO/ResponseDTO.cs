namespace SistemaGS.DTO
{
    public class ResponseDTO<T>
    {
        public T? Resultado { get; set; }
        public bool EsCorrecto { get; set; }
        public string? Mensaje { get; set; }
    }
}
