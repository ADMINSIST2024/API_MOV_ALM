namespace API_MOV_ALM.Models
{
    public class Almacen
    {
        public int CodCia { get; set; }
        public int CodAlg { get; set; } 

        public string DesAlg { get; set; } 

        public string TipAlg { get; set; } 

        public string CodCos { get; set; }

        public int StaAlg { get; set; } 

        public string RnoAlg { get; set; } 

        public string RcoAlg { get; set; }

        public string TdcAlg { get; set; }

        public string RncAlg { get; set; } 

        public DateTime? FhAct { get; set; }
        public string TipoMovimiento { get; set; }
        public int nota { get; set; }
        public string? codigoEtiqueta { get; set; }

        public string pcName { get; set; }
    }
}
