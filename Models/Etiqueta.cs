using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Etiqueta
    {
        public string? codEtiqueta { get; set; }
        // codexi NUMERIC (13, 0)
        public decimal codexi { get; set; }

        // codchi CHAR (5)
        public string codchi { get; set; }

        // nlhmag CHAR (20)
        public string nlhmag { get; set; }

        // cremang NUMERIC (15, 6)
        public decimal cremang { get; set; }

        // unimed CHAR (2)
        public string unimed { get; set; }

        // caemag NUMERIC (4, 0)
        public decimal caemag { get; set; }

        // umemag CHAR (2)
        public string umemag { get; set; }

        // codigo VARCHAR (43)
        public string codigo { get; set; }

        // codalg NUMERIC (3, 0)
        public decimal codalg { get; set; }

        // codcia NUMERIC (2, 0)
        public decimal codcia { get; set; }

        // tmvma1 CHAR (1)
        public string tmvma1 { get; set; }

        // tmvmag CHAR (1)
        public string tmvmag { get; set; }

        // ademag NUMERIC (3, 0)
        public decimal ademag { get; set; }

        // codtex NUMERIC (2, 0)
        public decimal codtex { get; set; }

        // codprv CHAR (8)
        public string codprv { get; set; }

        // trhmag NUMERIC (6, 2)
        public decimal trhmag { get; set; }

        // urhmag CHAR (4)
        public string urhmag { get; set; }

        // fecmag DATE
        public DateTime fecmag { get; set; }

        // ltomag CHAR (10)
        public string ltomag { get; set; }
    }
}
