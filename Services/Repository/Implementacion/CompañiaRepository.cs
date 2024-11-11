using API_MOV_ALM.Models;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Services.Repository.Interface;
using Newtonsoft.Json;
using System.Linq;

namespace Services.Repository.Implementacion
{
    public class CompañiaRepository : ICompañiaRepository<Compañia>
    {
        private readonly string? CadenaAS400;
        public CompañiaRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<List<Compañia>> ObtenerCompañia()
        {
            string jsonResultados = "";


            List<Compañia> ListaCompañia = new List<Compañia>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_COMPANIA", con))
                        {
                            
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Compañia obj_BE = new Compañia();
                                    obj_BE.CodCia = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.DesCia = lector[1].ToString().Trim();
                                   
                                    ListaCompañia.Add(obj_BE);
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
            return ListaCompañia;
        }

        public async Task<List<Compañia>> ObtenerCompañia_x_Codigo(Compañia comp)
        {
           


            List<Compañia> ListaCompañia = new List<Compañia>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_COMPANIA_X_CODIGO", con))
                        {
                           
                            cmd.Parameters.AddWithValue("@codcia", comp.CodCia);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Compañia obj_BE = new Compañia();
                                    obj_BE.CodCia = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.DesCia = lector[1].ToString().Trim();
                                  
                                    ListaCompañia.Add(obj_BE);
                                }

                                lector.Close();
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                // Obtenga el primer marco de pila (donde se originó la excepción)
                StackFrame? frame = new StackTrace(ex, true).GetFrame(0);

                // Capture detalles esenciales de error para el registro (con operador condicional nulo)
                string sourceMethod = frame?.GetMethod()?.Name;
                string sourceFile = frame?.GetFileName();
                int lineNumber = frame?.GetFileLineNumber() ?? -1; // Establece un valor predeterminado para lineNumber si es null
                string errorMessage = ex.Message;
            }
            return ListaCompañia;
        }
    }
}
