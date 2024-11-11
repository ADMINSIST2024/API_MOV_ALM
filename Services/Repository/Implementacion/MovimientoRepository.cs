using Models;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Services.Repository.Implementacion
{
    public class MovimientoRepository : IMovimientoRepository<Movimientos>
    {
        private readonly string? CadenaAS400;

        public MovimientoRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public List<Movimientos> ConsultaMovimientos(Movimientos obj)
        {
            string jsonResultados = "";
            DateTime fecmagDate = DateTime.ParseExact(obj.fecmag, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            List<Movimientos> ListaMovimientos = new List<Movimientos>();

            try
            {
                if (string.IsNullOrEmpty(CadenaAS400))
                {
                    Console.WriteLine("La cadena de conexión es nula o vacía");
                }
                else
                {
                    using (var con = new OleDbConnection(CadenaAS400))
                    {
                        con.Open();

                        using (OleDbCommand cmd = new("SP_API_CONSULTA_MOVIMIENTO", con))
                        {
                           
                            cmd.Parameters.AddWithValue("@FECMAG", fecmagDate.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CODALG", obj.codalg);
                            cmd.Parameters.AddWithValue("@CODEXI", obj.codexi);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {
                                    Movimientos obj_BE = new Movimientos();

                                    obj_BE.mov = lector[0].ToString().Trim();
                                    obj_BE.cscmag = Convert.ToInt32(lector[1]);
                                    obj_BE.codalg = Convert.ToInt32(lector[2]);
                                    obj_BE.ademag = Convert.ToInt32(lector[3]);
                                    obj_BE.orden_csc = lector[4].ToString().Trim();
                                    obj_BE.caemag = Convert.ToInt32(lector[5]);
                                    obj_BE.canmag = Convert.ToDecimal(lector[6]);
                                    obj_BE.codexi = Convert.ToInt64(lector[7]);
                                    obj_BE.codchi = lector[8].ToString().Trim();
                                    obj_BE.codprv = lector[9].ToString().Trim();
                                    obj_BE.nlhmag = lector[10].ToString().Trim();
                                    obj_BE.trhmag = Convert.ToDecimal(lector[11]);
                                    obj_BE.urhmag = lector[12].ToString().Trim();
                                    obj_BE.codigo = lector[13].ToString().Trim();
                                    obj_BE.codtmv = lector[14].ToString().Trim();
                                    obj_BE.cencos = Convert.ToInt32(lector[15]);

                                    ListaMovimientos.Add(obj_BE);
                                }

                                lector.Close();
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrames().Where(f => !String.IsNullOrEmpty(f.GetFileName())
                     && f.GetILOffset() != StackFrame.OFFSET_UNKNOWN
                     && f.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN
                     && !f.GetMethod().Module.Assembly.GetName().Name.Contains("mscorlib")).First();

                string MachineName = System.Environment.MachineName;
                string UserName = System.Environment.UserName.ToUpper();
                string Mensaje = ex.Message;
                int LineaError = frame.GetFileLineNumber();
                string Proyecto = frame.GetMethod().Module.Assembly.GetName().Name;
                string Clase = frame.GetMethod().DeclaringType.Name;
                string metodo = frame.GetMethod().Name;
                string codigoError = Convert.ToString(frame.GetHashCode());
            }
            return ListaMovimientos;
        }
    }
}
