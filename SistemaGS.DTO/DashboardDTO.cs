namespace SistemaGS.DTO
{
    public class DashboardDTO
    {
        public int AyudasCM { get; set; }
        public int AyudasLM { get; set; }
        public double varLM => AyudasLM <= 0 ? 100 : ((double)(AyudasCM / AyudasLM) - 1) * 100;

        public int AyudasCY { get; set; }
        public int AyudasLY { get; set; }
        public double varLY => AyudasLY <= 0 ? 100 : ((double)(AyudasCY / AyudasLY) - 1) * 100;

        public int PersonasCM { get; set; }
        public int PersonasLM { get; set; }
        public double PvarLM => PersonasLM <= 0 ? 100 : ((double)(PersonasCM / PersonasLM) - 1) * 100;

        public int RechazadasCM { get; set; }
        public int RechazadasLM { get; set; }
        public double RvarLM => RechazadasLM <= 0 ? 100 : ((double)(RechazadasCM / RechazadasLM) - 1) * 100;
    }
}
