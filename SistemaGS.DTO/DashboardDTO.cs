namespace SistemaGS.DTO
{
    public class DashboardDTO
    {
        public int AyudasCM { get; set; }
        public int AyudasLM { get; set; }
        public int varLM => AyudasLM <= 0 ? 100 : ((AyudasCM / AyudasLM) - 1) * 100;

        public int AyudasCY { get; set; }
        public int AyudasLY { get; set; }
        public int varLY => AyudasLY <= 0 ? 100 : ((AyudasCY / AyudasLY) - 1) * 100;

        public int PersonasCM { get; set; }
        public int PersonasLM { get; set; }
        public int PvarLM => PersonasLM <= 0 ? 100 : ((PersonasCM / PersonasLM) - 1) * 100;

        public int RechazadasCM { get; set; }
        public int RechazadasLM { get; set; }
        public int RvarLM => RechazadasLM <= 0 ? 100 : ((RechazadasCM / RechazadasLM) - 1) * 100;
    }
}
