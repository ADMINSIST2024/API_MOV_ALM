using API_MOV_ALM.Models;
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
using System.Configuration;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using DTOs.DtosInputs.DtosGeneralInputs;
using DTOs.DtosInputs.DtosAlta;

namespace Services.Repository.Implementacion
{
    public class GeneralRepository : IGeneralRepository<General>
    {
        private readonly string? CadenaAS400;

        public GeneralRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<int> BloquearRegistro(General obj)
        {
            int rowsAffected = 0;
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
                        
                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros
                            cmd.Parameters.AddWithValue("@PCNAME", obj.pcName);  // Asumiendo que obj tiene una propiedad PCName
                            cmd.Parameters.AddWithValue("@CODIGO", obj.codigoEtiqueta);  // Asumiendo que obj tiene una propiedad Codigo


                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                   
                                   rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public int BloquearRegistro2(string etiqueta, string pcName)
        {
            int rowsAffected = 0;
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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros
                            cmd.Parameters.AddWithValue("@PCNAME", pcName);  // Asumiendo que obj tiene una propiedad PCName
                            cmd.Parameters.AddWithValue("@CODIGO", etiqueta);  // Asumiendo que obj tiene una propiedad Codigo


                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public async Task<int> BloquearRegistroCorrelativo(General obj)
        {
            int rowsAffected = 0;
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

                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO_CORRELATIVO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@PCNAME", obj.pcName);
                            cmd.Parameters.AddWithValue("@CODCIA", obj.codCia);
                            cmd.Parameters.AddWithValue("@CODALG", obj.codAlg);
                            cmd.Parameters.AddWithValue("@TMVCOR", obj.tmvcor);


                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public int BloquearRegistroCorrelativo2(string tmvmag, string pcName, int codalg, int codcompania)
        {
            int rowsAffected = 0;
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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO_CORRELATIVO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@PCNAME", pcName);
                            cmd.Parameters.AddWithValue("@CODCIA", codcompania);
                            cmd.Parameters.AddWithValue("@CODALG", codalg);
                            cmd.Parameters.AddWithValue("@TMVCOR", tmvmag);


                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public int BloquearRegistroCorrelativoNull(string tmvmag, string pcName, int codalg, int codcompania)
        {
            int rowsAffected = 0;
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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO_CORRELATIVO_NULL", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@PCNAME", pcName);
                            cmd.Parameters.AddWithValue("@CODCIA", codcompania);
                            cmd.Parameters.AddWithValue("@CODALG", codalg);
                            cmd.Parameters.AddWithValue("@TMVCOR", tmvmag);


                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public  async  Task<int> BloquearRegistroNull(General obj)
        {
            int rowsAffected=0;
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
                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_BLOQUEAR_REGISTRO_NULL", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                          
                            cmd.Parameters.AddWithValue("@PCNAME", obj.pcName);  // Asumiendo que obj tiene una propiedad PCName
                            cmd.Parameters.AddWithValue("@CODIGO", obj.codigoEtiqueta);  // Asumiendo que obj tiene una propiedad Codigo

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public async Task<int> CriterioTipoExistencia(General obj)
        {
            int res = 0;
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
                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_CRITERIO_TIP_EXISTENCIA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.AddWithValue("@CODTEX", obj.codTex);  
                          

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    res = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return res;
        }

        public async Task<int> DesbloquearFABCORRE(General obj)
        {
            int rowsAffected = 0;
            string mensaje = "";

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
                        await con.OpenAsync();

                        if (con.State == ConnectionState.Open)
                        {
                            using (OleDbCommand cmd = new OleDbCommand("SP_API_DESBLOQUEAR_FABCORRE", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Uso de parámetros explícitos para mejorar rendimiento
                                cmd.Parameters.AddWithValue("@CODCIA", obj.codCia);
                                cmd.Parameters.AddWithValue("@CODALG", obj.codAlg);
                                cmd.Parameters.AddWithValue("@TMVCOR", obj.tmvcor);
                            
                                var filasAfectadasParam = new OleDbParameter("@filasAfectadas", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                cmd.Parameters.Add(filasAfectadasParam);

                                await cmd.ExecuteNonQueryAsync();

                                rowsAffected = (int)filasAfectadasParam.Value;
                                mensaje = "Registro actualizado exitosamente, filas afectadas: " + rowsAffected;
                            }
                        }
                        else
                        {
                            mensaje = "No se pudo abrir la conexión a la base de datos";
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                // Capturar información detallada del error de base de datos
                mensaje = "Error en la base de datos: " + ex.Message;
                Console.WriteLine($"Código de error: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                // Captura general de excepciones
                mensaje = "Error general: " + ex.Message;
            }
            return (rowsAffected);
        }

        public int DesbloquearFABCORRE2(int codcompania, int codalg, string tmvmag)
        {
            int rowsAffected = 0;
            string mensaje = "";

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

                        if (con.State == ConnectionState.Open)
                        {
                            using (OleDbCommand cmd = new OleDbCommand("SP_API_DESBLOQUEAR_FABCORRE", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Uso de parámetros explícitos para mejorar rendimiento
                                cmd.Parameters.AddWithValue("@CODCIA", codcompania);
                                cmd.Parameters.AddWithValue("@CODALG", codalg);
                                cmd.Parameters.AddWithValue("@TMVCOR", tmvmag);

                                var filasAfectadasParam = new OleDbParameter("@filasAfectadas", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                cmd.Parameters.Add(filasAfectadasParam);

                                 cmd.ExecuteNonQuery();

                                rowsAffected = (int)filasAfectadasParam.Value;
                                mensaje = "Registro actualizado exitosamente, filas afectadas: " + rowsAffected;
                            }
                        }
                        else
                        {
                            mensaje = "No se pudo abrir la conexión a la base de datos";
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                // Capturar información detallada del error de base de datos
                mensaje = "Error en la base de datos: " + ex.Message;
                Console.WriteLine($"Código de error: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                // Captura general de excepciones
                mensaje = "Error general: " + ex.Message;
            }
            return (rowsAffected);
        }

        public async Task<int> DesbloquearRegistro(General obj)
        {
            int rowsAffected = 0;
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

                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_DESBLOQUEAR_REGISTRO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros
                          
                            cmd.Parameters.AddWithValue("@CODIGO", obj.codigoEtiqueta);  // Asumiendo que obj tiene una propiedad Codigo


                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public int DesbloquearRegistro2(string codigo)
        {
            int rowsAffected = 0;
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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_DESBLOQUEAR_REGISTRO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@CODIGO", codigo);  // Asumiendo que obj tiene una propiedad Codigo


                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public async Task<int> DesbloquearTABCORRE(General obj)
        {
            int rowsAffected = 0;
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

                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_DESBLOQUEA_TABCORRE", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@CODCIA", obj.codCia);  // Asumiendo que obj tiene una propiedad Codigo


                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    rowsAffected = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return rowsAffected;
        }

        public async Task<List<Etiqueta>> ObtenerDatosEtiqueta(Etiqueta obj)
        {

            string jsonResultados = "";


            List<Etiqueta> ListaDatosEtiqueta = new List<Etiqueta>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_DATOS_ETIQUETA", con))
                        {
                            cmd.Parameters.AddWithValue("@CODIGO", obj.codEtiqueta);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    Etiqueta obj_BE = new Etiqueta();
                                    obj_BE.codexi=Convert.ToDecimal(lector[0].ToString().Trim()??"0");
                                    obj_BE.codchi=lector[1].ToString().Trim();
                                    obj_BE.nlhmag=lector[2].ToString().Trim();
                                    obj_BE.cremang=Convert.ToDecimal(lector[3].ToString().Trim()??"0");
                                    obj_BE.unimed=lector[4].ToString().Trim();
                                    obj_BE.caemag=Convert.ToDecimal(lector[5].ToString().Trim()??"0");
                                    obj_BE.umemag=lector[6].ToString().Trim();
                                    obj_BE.codigo=lector[7].ToString().Trim();
                                    obj_BE.codalg=Convert.ToDecimal(lector[8].ToString().Trim()??"0");
                                    obj_BE.codcia=Convert.ToDecimal(lector[9].ToString().Trim()??"0");
                                    obj_BE.tmvma1=lector[10].ToString().Trim();
                                    obj_BE.tmvmag=lector[11].ToString().Trim();
                                    obj_BE.ademag=Convert.ToDecimal(lector[12].ToString().Trim()??"0");
                                    obj_BE.codtex=Convert.ToDecimal(lector[13].ToString().Trim()??"0");
                                    obj_BE.codprv=lector[14].ToString().Trim();
                                    obj_BE.trhmag=Convert.ToDecimal(lector[15].ToString().Trim()??"0");
                                    obj_BE.urhmag=lector[16].ToString().Trim();
                                    obj_BE.fecmag=Convert.ToDateTime(lector[17].ToString().Trim()??DateTime.MinValue.ToString());
                                    obj_BE.ltomag = lector[18].ToString().Trim();

                                    ListaDatosEtiqueta.Add(obj_BE);
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
            return ListaDatosEtiqueta;
        }

        public List<Etiqueta> ObtenerDatosEtiqueta2(string etiqueta)
        {
            string jsonResultados = "";


            List<Etiqueta> ListaDatosEtiqueta = new List<Etiqueta>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_DATOS_ETIQUETA", con))
                        {
                            cmd.Parameters.AddWithValue("@CODIGO", etiqueta);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {
                                    Etiqueta obj_BE = new Etiqueta();
                                    obj_BE.codexi = Convert.ToDecimal(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.codchi = lector[1].ToString().Trim();
                                    obj_BE.nlhmag = lector[2].ToString().Trim();
                                    obj_BE.cremang = Convert.ToDecimal(lector[3].ToString().Trim() ?? "0");
                                    obj_BE.unimed = lector[4].ToString().Trim();
                                    obj_BE.caemag = Convert.ToDecimal(lector[5].ToString().Trim() ?? "0");
                                    obj_BE.umemag = lector[6].ToString().Trim();
                                    obj_BE.codigo = lector[7].ToString().Trim();
                                    obj_BE.codalg = Convert.ToDecimal(lector[8].ToString().Trim() ?? "0");
                                    obj_BE.codcia = Convert.ToDecimal(lector[9].ToString().Trim() ?? "0");
                                    obj_BE.tmvma1 = lector[10].ToString().Trim();
                                    obj_BE.tmvmag = lector[11].ToString().Trim();
                                    obj_BE.ademag = Convert.ToDecimal(lector[12].ToString().Trim() ?? "0");
                                    obj_BE.codtex = Convert.ToDecimal(lector[13].ToString().Trim() ?? "0");
                                    obj_BE.codprv = lector[14].ToString().Trim();
                                    obj_BE.trhmag = Convert.ToDecimal(lector[15].ToString().Trim() ?? "0");
                                    obj_BE.urhmag = lector[16].ToString().Trim();
                                    obj_BE.fecmag = Convert.ToDateTime(lector[17].ToString().Trim() ?? DateTime.MinValue.ToString());
                                    obj_BE.ltomag = lector[18].ToString().Trim();

                                    ListaDatosEtiqueta.Add(obj_BE);
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
            return ListaDatosEtiqueta;
        }

        public  List<ObtenerDatosStockEmpaqueDtoOutput> ObtenerDatosStockEmpaque(string ObtenerTodasEtiquetas)
        {

            string jsonResultados = "";


            List<ObtenerDatosStockEmpaqueDtoOutput> ListaObtenerDatosStockEmpaque = new List<ObtenerDatosStockEmpaqueDtoOutput>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_DATOS_STOCK_EMPAQUE", con))
                        {
                            cmd.Parameters.AddWithValue("@ETIQUETAS", ObtenerTodasEtiquetas);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while ( lector.Read())
                                {
                                    ObtenerDatosStockEmpaqueDtoOutput obj_BE = new ObtenerDatosStockEmpaqueDtoOutput();
                                    obj_BE.canmag = Convert.ToDouble(lector[0].ToString().Trim() ?? "0");
                                    obj_BE.cremag = Convert.ToDouble(lector[1].ToString().Trim());
                                    obj_BE.codigoEtiqueta = lector[2].ToString().Trim();
                                    obj_BE.scoma1 = Convert.ToDouble(lector[3].ToString().Trim() ?? "0");
                                    obj_BE.stockreal = Convert.ToDouble(lector[4].ToString().Trim() ?? "0");


                                    ListaObtenerDatosStockEmpaque.Add(obj_BE);
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
            return ListaObtenerDatosStockEmpaque;
        }

        public ObtenerDiasPermitidosDtoOutputs ObtenerDiasPermitidos()
        {
            ObtenerDiasPermitidosDtoOutputs obj_ObtenerDiasPermitidos = null;

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
                         con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_DIAS_PERMITIDOS", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if (lector.Read())
                                {
                                    obj_ObtenerDiasPermitidos = new ObtenerDiasPermitidosDtoOutputs
                                    {
                                        fecMen = lector[0].ToString().Trim(),
                                        fecAct = lector[1].ToString().Trim() 
                                       
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_ObtenerDiasPermitidos;
        }

        public ObtenerDiasPermitidosAyerDtoOutputs ObtenerDiasPermitidosAyer()
        {
            ObtenerDiasPermitidosAyerDtoOutputs obj_ObtenerDiasPermitidosAyer = null;

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
                         con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_DIAS_PERMITIDOS_AYER", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if ( lector.Read())
                                {
                                    obj_ObtenerDiasPermitidosAyer = new ObtenerDiasPermitidosAyerDtoOutputs
                                    {
                                        fecMen = lector[0].ToString().Trim(),
                                        fecAct = lector[1].ToString().Trim()

                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_ObtenerDiasPermitidosAyer;
        }

        public ObtenerFechaSistemaDtoOutputs ObtenerFechaSistema()
        {
            ObtenerFechaSistemaDtoOutputs obj_ObtenerFechaSistema = null;

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
                         con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_FECHA_SISTEMA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if ( lector.Read())
                                {
                                    obj_ObtenerFechaSistema = new ObtenerFechaSistemaDtoOutputs
                                    {
                                        fechaSistema = lector[0].ToString().Trim()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_ObtenerFechaSistema;
        }

        public ObtenerFechaSistemaAyerDtoOutputs ObtenerFechaSistemaAyer()
        {
            ObtenerFechaSistemaAyerDtoOutputs obj_ObtenerFechaSistemaAyer = null;

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
                         con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_FECHA_SISTEMA_AYER", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if ( lector.Read())
                                {
                                    obj_ObtenerFechaSistemaAyer = new ObtenerFechaSistemaAyerDtoOutputs
                                    {
                                        fechaSistemaAyer = lector[0].ToString().Trim()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_ObtenerFechaSistemaAyer;
        }

        public async Task<List<General>> ObtenerFMOVALG2(General obj)
        {

            string jsonResultados = "";


            List<General> ListaDatosGeneral = new List<General>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_FMOVALG2", con))
                        {
                            cmd.Parameters.AddWithValue("@CODIGO ", obj.codigoEtiquetas);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    General obj_BE = new General();

                                    obj_BE.tmamag = lector[0].ToString().Trim();
                                    obj_BE.nmamag = Convert.ToInt64(lector[1].ToString().Trim() ?? "0");
                                    obj_BE.csamag = Convert.ToInt64(lector[2].ToString().Trim() ?? "0");
                                    obj_BE.seama2 = Convert.ToInt64(lector[3].ToString().Trim() ?? "0");
                                    obj_BE.alamag = Convert.ToInt64(lector[4].ToString().Trim() ?? "0");
                                    obj_BE.codtex = Convert.ToInt32(lector[5].ToString().Trim() ?? "0");
                                    obj_BE.codexi = Convert.ToInt64(lector[6].ToString().Trim() ?? "0");
                                    obj_BE.codprv = lector[7].ToString().Trim();
                                    obj_BE.nlhmag = lector[8].ToString().Trim();
                                    obj_BE.trhmag = Convert.ToDouble(lector[9].ToString().Trim() ?? "0");
                                    obj_BE.urhmag = lector[10].ToString().Trim();
                                    obj_BE.canmag = Convert.ToDouble(lector[11].ToString().Trim() ?? "0");
                                    obj_BE.canmag = Convert.ToDouble(lector[12].ToString().Trim() ?? "0");

                                    ListaDatosGeneral.Add(obj_BE);
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
            return ListaDatosGeneral;
        }

        public ObtenerHoraSistemaDtoOutputs ObtenerHoraSistema()
        {
            ObtenerHoraSistemaDtoOutputs obj_ObtenerHoraSistema = null;

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
                         con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_OBTENER_HORA_SISTEMA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if (lector.Read())
                                {
                                    obj_ObtenerHoraSistema = new ObtenerHoraSistemaDtoOutputs
                                    {
                                        horaSistema = Convert.ToInt32(lector[0].ToString().Trim() ?? "0"),
                                        minutoSistema = Convert.ToInt32(lector[1].ToString().Trim() ?? "0"),
                                        segundoSistema = Convert.ToInt32(lector[2].ToString().Trim() ?? "0")
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_ObtenerHoraSistema;
        }

        public async Task<General> UtilizaRegistro(General obj)
        {
            General obj_General = null;

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
                        await con.OpenAsync();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_UTILIZA_REGISTRO", con))
                        {
                            
                            cmd.Parameters.AddWithValue("@CODIGO", obj.codigoEtiqueta);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                if (await lector.ReadAsync())
                                {
                                    obj_General = new General
                                    {
                                        pcName = lector[0].ToString().Trim()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_General;
        }

      

        public async Task<int> ValidarTipoListado(General obj)
        {
            int res = 0;
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
                        await con.OpenAsync();

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_VALIDAR_TIPO_LISTADO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.AddWithValue("@NUMOPT", obj.nroOrden); 
                          

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {

                                    res = Convert.ToInt32(lector[0].ToString().Trim() ?? "0");

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
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }
            return res;
        }

        public async Task<List<General>> ValidaUsoCorrelativo()
        {
            string jsonResultados = "";


            List<General> ListaDatosGeneral = new List<General>();

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
                        await con.OpenAsync();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_VALIDAR_USO_CORRELATIVO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                while (await lector.ReadAsync())
                                {
                                    General obj_BE = new General();

                                    obj_BE.pcName = lector[0].ToString().Trim();
                                    obj_BE.nCorre = lector[1].ToString().Trim();
                                    ListaDatosGeneral.Add(obj_BE);


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return ListaDatosGeneral;
        }

        public string UtilizaRegistro2(string etiqueta)
        {
            General obj_General = null;

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
                         con.OpenAsync();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_UTILIZA_REGISTRO", con))
                        {

                            cmd.Parameters.AddWithValue("@CODIGO", etiqueta);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector =  cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if (lector.Read())
                                {
                                    obj_General = new General
                                    {
                                        pcName = lector[0].ToString().Trim()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_General.pcName;
        }

        General IGeneralRepository<General>.ValidarLogin(General obj)
        {
            string jsonResultados = "";



            General obj_BE = new General();
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
                        con.Open();  // Usa OpenAsync en lugar de Open

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_LOGIN", con))
                        {
                            cmd.Parameters.AddWithValue("@NOMUSU", obj.nomUsu);
                            cmd.Parameters.AddWithValue("@PAWUSU", obj.pawUsu);
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                if (lector.Read())
                                {


                                    obj_BE.nomUsu = lector[0].ToString().Trim();
                                    obj_BE.pawUsu = lector[1].ToString().Trim();
                                    obj_BE.codPer = Convert.ToInt64(lector[2].ToString().Trim());
                                    obj_BE.faUsu = Convert.ToInt32(lector[3].ToString().Trim());
                                    obj_BE.staUsu = Convert.ToInt32(lector[4].ToString().Trim());



                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            return obj_BE;
        }

        public List<General> ObtenerDatosOrden(General obj)
        {
            string jsonResultados = "";


            List<General> ListaDatosGeneral = new List<General>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_DATOS_ORDEN", con))
                        {
                            cmd.Parameters.AddWithValue("@ORDEN", obj.nroOrden);
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {
                                    General obj_BE = new General();
                                    obj_BE.tipoMovimiento = lector[0].ToString().Trim();
                                    obj_BE.anio = lector[1].ToString().Trim();
                                    obj_BE.codigoProceso = lector[2].ToString().Trim();
                                    obj_BE.descripcionProceso = lector[3].ToString().Trim();
                                    obj_BE.nroCargaMaxima = lector[4].ToString().Trim();
                                    obj_BE.estadoOrden = lector[5].ToString().Trim();

                                    ListaDatosGeneral.Add(obj_BE);
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
            return ListaDatosGeneral;
        }

        public List<General> ObtenerEstadoCarga(General obj)
        {
            string jsonResultados = "";


            List<General> ListaDatosGeneral = new List<General>();

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

                        using (OleDbCommand cmd = new("SP_API_OBTENER_ESTADO_CARGA", con))
                        {
                            cmd.Parameters.AddWithValue("@ORDEN", obj.nroOrden);
                            cmd.Parameters.AddWithValue("@CARGA", obj.nroCarga);

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {
                                    General obj_BE = new General();
                                    obj_BE.estadoCarga =Convert.ToInt32(lector[0].ToString().Trim());

                                    ListaDatosGeneral.Add(obj_BE);
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
            return ListaDatosGeneral;
        }

        public List<General> ExisteConsecutivo(General obj)
        {
            string jsonResultados = "";


            List<General> ListaDatosGeneral = new List<General>();

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

                        using (OleDbCommand cmd = new("SP_API_EXISTE_CONSECUTIVO", con))
                        {
                            cmd.Parameters.AddWithValue("@ORDEN", obj.nroOrden);
                            cmd.Parameters.AddWithValue("@TIPO", obj.tipoMovimiento);
                            cmd.Parameters.AddWithValue("@COSECUTIVO", obj.consecutivo);

                            cmd.CommandType = CommandType.StoredProcedure;
                            using (var lector = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                            {
                                while (lector.Read())
                                {
                                    General obj_BE = new General();
                                    obj_BE.estadoConsecutivo = lector[0].ToString().Trim();

                                    ListaDatosGeneral.Add(obj_BE);
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
            return ListaDatosGeneral;
        }
    }
}
