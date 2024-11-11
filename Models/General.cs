using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models
{
    public class General
    {
        public string? fechaSistema { get; set; }
        public int horaSistema { get; set; }
        public int minutoSistema { get; set; }
        public int segundoSistema { get; set; }
        public string? codigoEtiqueta { get; set; }
        public string? pcName { get; set; }
        public int nroOrden { get; set; }
        public string? fecMen { get; set; }
        public string? fecAct { get; set; }


        public double canmag { get; set; }

        public double cremag { get; set; }
        public string? codigoEtiquetas { get; set; }
        public double scoma1 { get; set; }
        public double stockreal { get; set; }


        public int codCia { get; set; }
        public int codAlg { get; set; }
        public string tmvcor { get; set; }
        public int codTex { get; set; }
        public string nCorre { get; set; }


        
        public string tmamag { get; set; }
        public long nmamag { get; set; }
        public long csamag { get; set; }
        public long seama2 { get; set; }
        public long alamag { get; set; }
        public int codtex { get; set; }
        public long codexi { get; set; }
        public string codprv { get; set; }
        public string nlhmag { get; set; }
        public double trhmag { get; set; }
        public string urhmag { get; set; }

        public string nomUsu { get; set; }
        public string pawUsu { get; set; }
        public long codPer { get; set; }
        public int faUsu { get; set; }
        public int staUsu { get; set; }

        public string anio { get; set; }
        public string tipoMovimiento { get; set; }
        public string codigoProceso { get; set; }
        public string descripcionProceso { get; set; }
        public int nroCarga { get; set; }
        public string nroCargaMaxima { get; set; }
        public string estadoOrden { get; set; }
        public int estadoCarga { get; set; }
        public int consecutivo { get; set; }
        public string estadoConsecutivo { get; set; }

    }
}
