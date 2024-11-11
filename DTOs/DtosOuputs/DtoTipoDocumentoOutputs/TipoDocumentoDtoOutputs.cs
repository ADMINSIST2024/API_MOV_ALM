using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs.DtosOuputs.DtoTipoMovimientoOutputs
{
    public class TipoDocumentoDtoOutputs
    {
    
        public string codTipDoc { get; set; }
        
        public string desTipDoc { get; set; }
    }
}
