using DTOs.DtosInputs.DtosAlta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DtosInputs.DtosGeneralInputs
{
   public class ProcesarGuardadoDtoInputs
    {
        
        public List<ObtenerDatosEtiquetaDtoInputs> TotalEtiquetas{get; set;}
        public int codcompania{get; set;}
        public int codalg{get; set;}
        public string tmvmag{get; set;}
        public string nmvmag{get; set;}
        public string codtmv{get; set;}
        public string cencos{get; set;}
        public int ademag{get; set;}
        public string refere{get; set;}
        public string ctdor1{get; set;}
        public string anoor1{get; set;}
        public string nroor1{get; set;}
        public string cscor2{get; set;}
        public string fecmag{get; set;}
        public string ncrma2{get; set;}
        public int almacen{get; set;}
        public string fechaMovimiento{get; set;}
        public string pcName{get; set;}
        public List<DtoObtenerDatosStockEmpaqueInput> etiquetasStock{get; set;}
        public string obtenerTodasEtiquetas{get; set;}
        public string usuario { get; set; }

    }
}
