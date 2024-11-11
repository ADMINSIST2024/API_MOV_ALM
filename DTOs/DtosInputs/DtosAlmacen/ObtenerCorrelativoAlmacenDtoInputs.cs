using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DtosInputs.DtosAlmacen
{
    public class ObtenerCorrelativoAlmacenDtoInputs
    {
        public int codigoCia { get; set; }
        public int codigoAlmacen { get; set; }
        public string tipoMovimiento { get; set; }
    }
}
