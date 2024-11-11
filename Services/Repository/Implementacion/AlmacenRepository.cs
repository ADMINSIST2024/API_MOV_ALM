using API_MOV_ALM.Models;
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
using DTOs.DtosOuputs.DtosAlmacen;
using DTOs.DtosOuputs.DtosGeneralOutputs;

namespace Services.Repository.Implementacion
{
    public class AlmacenRepository : IAlmacenRepository<Almacen>
    {
        private readonly string? CadenaAS400;
        public AlmacenRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }
        public async Task<List<Almacen>> ObtenerAlmacen()
        {
            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_ALMACEN", con))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.CodAlg = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.DesAlg = lector[1].ToString().Trim();

                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }

        public async Task<List<Almacen>> ObtenerAlmacenXCodigo(Almacen obj_Almacen)
        {

            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_ALMACEN_X_CODIGO", con))
                        {

                            cmd.Parameters.AddWithValue("@CODIGO_ALMACEN", obj_Almacen.CodAlg);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.CodAlg = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.DesAlg = lector[1].ToString().Trim();
                                    obj_BE.TdcAlg = lector[2].ToString().Trim();
                                    obj_BE.CodCos = lector[3].ToString().Trim();
                                    obj_BE.RnoAlg = lector[4].ToString().Trim();
                                    obj_BE.RcoAlg = lector[5].ToString().Trim();
                                    obj_BE.RncAlg = lector[6].ToString().Trim();


                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }

        public  List<ObtenerCorrelativoAlmacenDtoOuputs> ObtenerCorrelativoAlmacen(string tmvmag, string pcName, int codalg, int codcompania)
        {
            string jsonResultados = "";


            List<ObtenerCorrelativoAlmacenDtoOuputs> ListaAlmacen = new List<ObtenerCorrelativoAlmacenDtoOuputs>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_CORRELATIVO_ALMACEN", con))
                        {

                            cmd.Parameters.AddWithValue("@CODCIA", codcompania);
                            cmd.Parameters.AddWithValue("@CODALG", codalg);
                            cmd.Parameters.AddWithValue("@TMVMAG", tmvmag);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {
                                    ObtenerCorrelativoAlmacenDtoOuputs obj_BE = new ObtenerCorrelativoAlmacenDtoOuputs();
                                    obj_BE.nota = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                           


                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;

        }
        public async Task<List<Almacen>> ObtenerCorrelativoAlmacen2(Almacen obj_Almacen)
        {
            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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
                        con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_CORRELATIVO_ALMACEN", con))
                        {

                            cmd.Parameters.AddWithValue("@CODCIA", obj_Almacen.CodCia);
                            cmd.Parameters.AddWithValue("@CODALG", obj_Almacen.CodAlg);
                            cmd.Parameters.AddWithValue("@TMVMAG", obj_Almacen.TipoMovimiento);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.nota = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");



                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;

        }
        public async Task<List<Almacen>> ObtenerRegistro_FMOVALG2(Almacen obj_Almacen)
        {
            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_VALIDAR_REGISTRO_FMOVALG2", con))
                        {

                            cmd.Parameters.AddWithValue("@CODIGO", obj_Almacen.codigoEtiqueta);
                         
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.CodCia = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.CodAlg = Convert.ToInt32(lector[1].ToString().Trim() ?? "0");
                                    obj_BE.TipoMovimiento = lector[2].ToString().Trim(); 
                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }

        public List<Almacen> ObtenerRegistro_FMOVALG2_2(string etiqueta)
        {
            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_VALIDAR_REGISTRO_FMOVALG2", con))
                        {

                            cmd.Parameters.AddWithValue("@CODIGO", etiqueta);

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.CodCia = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.CodAlg = Convert.ToInt32(lector[1].ToString().Trim() ?? "0");
                                    obj_BE.TipoMovimiento = lector[2].ToString().Trim();
                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }

        public async Task<List<Almacen>> UtilizaRegistroCorrelativoAlmacen(Almacen obj_Almacen)
        {
            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_UTILIZA_REG_CORRELATIVO_ALM", con))
                        {

                            cmd.Parameters.AddWithValue("@CODCIA", obj_Almacen.CodCia);
                            cmd.Parameters.AddWithValue("@CODALG", obj_Almacen.CodAlg);
                            cmd.Parameters.AddWithValue("@TMVCOR", obj_Almacen.TipoMovimiento);

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    
                                    obj_BE.pcName = lector[0].ToString().Trim();
                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }

        public List<UtilizaRegistroDtoOutputs> UtilizaRegistroCorrelativoAlmacen2(string tmvmag, int codalg, int codcompania)
        {
            string jsonResultados = "";


            List<UtilizaRegistroDtoOutputs> ListaUtilizaRegistro = new List<UtilizaRegistroDtoOutputs>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_UTILIZA_REG_CORRELATIVO_ALM", con))
                        {

                            cmd.Parameters.AddWithValue("@CODCIA", codcompania);
                            cmd.Parameters.AddWithValue("@CODALG", codalg);
                            cmd.Parameters.AddWithValue("@TMVCOR", tmvmag);

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {
                                    UtilizaRegistroDtoOutputs obj_BE = new UtilizaRegistroDtoOutputs();

                                    obj_BE.pcName = lector[0].ToString().Trim();
                                    ListaUtilizaRegistro.Add(obj_BE);
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
            return ListaUtilizaRegistro;

        }

        public async Task<List<Almacen>> ValidarAlamcenXCcosto(Almacen obj_Almacen)
        {

            string jsonResultados = "";


            List<Almacen> ListaAlmacen = new List<Almacen>();

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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_VALIDAR_ALM_CCOSTO", con))
                        {

                            cmd.Parameters.AddWithValue("@CODALG", obj_Almacen.CodAlg);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Almacen obj_BE = new Almacen();
                                    obj_BE.CodAlg = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");                                   
                                    obj_BE.CodCos = lector[1].ToString().Trim();
                                    ListaAlmacen.Add(obj_BE);
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
            return ListaAlmacen;
        }
    }
}
