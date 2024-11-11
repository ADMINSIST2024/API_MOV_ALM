using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DtosOuputs.DtosAlmacen
{
    public class ObtenerAlmacenXCodigoDtoOuputs
    {

        [Display(Name = "CODALG")]
        public int codigoAlmacen { get; set; }

        [Display(Name = "DESALG")]
        public string? descripcionAlmacen { get; set; }

        [Display(Name = "RNOALG")]
        public string? requiereNroOrden { get; set; }

        [Display(Name = "RCOALG")]
        public string? requiereCscOrden { get; set; }

        [Display(Name = "TDCALG")]
        public string? tipoDocumento { get; set; }


        [Display(Name = "RNCALG")]
        public string? requiereNroCarga { get; set; }

        
        public string? centroCosto { get; set; }

    }
}
