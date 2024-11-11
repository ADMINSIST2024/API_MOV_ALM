using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DtosInputs.DtosGeneralInputs
{
  public  class BloquearRegistroCorrelativoNullDtoInputs
    {
        public int codCia { get; set; }
        public int codAlg { get; set; }
        public string tmvcor { get; set; }
        public string? pcName { get; set; }
    }
}
