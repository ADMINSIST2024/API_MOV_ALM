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
using System.Xml.Linq;

namespace Services.Repository.Implementacion
{
    public class AltaRepository : IAltaRepository<Alta>
    {
        private readonly string? CadenaAS400;

        public AltaRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }

        public async Task<int> Alta(Alta obj)
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

                        using (OleDbCommand cmd = new OleDbCommand("SP_API_ALTA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Agregar parámetros

                            cmd.Parameters.AddWithValue("@codigo", obj.codigo) ;
                            cmd.Parameters.AddWithValue("@codcia", obj.codcia) ;
                            cmd.Parameters.AddWithValue("@codalg", obj.codalg) ;
                            cmd.Parameters.AddWithValue("@tmvmag", obj.tmvmag) ;
                            cmd.Parameters.AddWithValue("@nmvmag", obj.nmvmag) ;
                            cmd.Parameters.AddWithValue("@cscmag", obj.cscmag) ;
                            cmd.Parameters.AddWithValue("@secma2", obj.secma2) ;
                            cmd.Parameters.AddWithValue("@codtmv", obj.codtmv) ;
                            cmd.Parameters.AddWithValue("@cremag", obj.cremag) ;
                            cmd.Parameters.AddWithValue("@cencos", obj.cencos) ;
                            cmd.Parameters.AddWithValue("@ademag", obj.ademag) ;
                            cmd.Parameters.AddWithValue("@ucrmag", obj.ucrmag) ;
                            cmd.Parameters.AddWithValue("@caemag", obj.caemag) ;
                            cmd.Parameters.AddWithValue("@refere", obj.refere) ;
                            cmd.Parameters.AddWithValue("@ctdor1", obj.ctdor1) ;
                            cmd.Parameters.AddWithValue("@anoor1", obj.anoor1) ;
                            cmd.Parameters.AddWithValue("@nroor1", obj.nroor1) ;
                            cmd.Parameters.AddWithValue("@cscor2", obj.cscor2) ;
                            cmd.Parameters.AddWithValue("@fecmag", obj.fecmag) ;
                            cmd.Parameters.AddWithValue("@pcname", obj.pcname) ;
                            cmd.Parameters.AddWithValue("@ncrma2", obj.ncrma2);


                            rowsAffected= await cmd.ExecuteNonQueryAsync();

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

        /*
         public async Task<int> InsertaODRPCON1(Alta obj)
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

                         using (OleDbCommand cmd = new OleDbCommand("SP_API_GRABAR_ODRPCON1", con))
                         {
                             cmd.CommandType = CommandType.StoredProcedure;

                             // Agregar parámetros

                             cmd.Parameters.AddWithValue("@CODCIA", obj.codcia);
                             cmd.Parameters.AddWithValue("@CODTDC", obj.codtdc);
                             cmd.Parameters.AddWithValue("@ANOPC1", obj.anopc1);
                             cmd.Parameters.AddWithValue("@NROPC1", obj.nropc1);
                             cmd.Parameters.AddWithValue("@CODALG", obj.codalg);
                             cmd.Parameters.AddWithValue("@FEMPC1", obj.fempc1);
                             cmd.Parameters.AddWithValue("@CIMPC1", obj.cimpc1);
                             cmd.Parameters.AddWithValue("@OBSPC1", obj.obspc1);
                             cmd.Parameters.AddWithValue("@CODTMV", obj.codtmv);
                             cmd.Parameters.AddWithValue("@CENCOS", obj.cencos);
                             cmd.Parameters.AddWithValue("@USCPC1", obj.uscpc1);
                             cmd.Parameters.AddWithValue("@FHCPC1", obj.fhcpc1);
                             cmd.Parameters.AddWithValue("@STAPC1", obj.stapc1);
                             cmd.Parameters.AddWithValue("@NPRPC1", obj.nprpc1);
                             cmd.Parameters.AddWithValue("@FGCPC1", obj.fgcpc1);



                             rowsAffected = await cmd.ExecuteNonQueryAsync();

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
        */

        public async Task<(int rowsAffected, string mensaje)> InsertaODRPCON1(Alta obj)
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
                            using (OleDbCommand cmd = new OleDbCommand("SP_API_GRABAR_ODRPCON1", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Uso de parámetros explícitos para mejorar rendimiento
                                cmd.Parameters.AddWithValue("@CODCIA", obj.codcia);
                                cmd.Parameters.AddWithValue("@CODTDC", obj.codtdc);
                                cmd.Parameters.AddWithValue("@ANOPC1", obj.anopc1);
                                cmd.Parameters.AddWithValue("@NROPC1", obj.nropc1);
                                cmd.Parameters.AddWithValue("@CODALG", obj.codalg);
                                cmd.Parameters.AddWithValue("@FEMPC1", DateTime.Parse(obj.fempc1).ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@CIMPC1", obj.cimpc1);
                                cmd.Parameters.AddWithValue("@OBSPC1", obj.obspc1);
                                cmd.Parameters.AddWithValue("@CODTMV", obj.codtmv);
                                cmd.Parameters.AddWithValue("@CENCOS", obj.cencos);
                                cmd.Parameters.AddWithValue("@USCPC1", obj.uscpc1);
                                cmd.Parameters.AddWithValue("@FHCPC1", obj.fhcpc1);
                                cmd.Parameters.AddWithValue("@STAPC1", obj.stapc1);
                                cmd.Parameters.AddWithValue("@NPRPC1", obj.nprpc1);
                                cmd.Parameters.AddWithValue("@FGCPC1", obj.fgcpc1);
                                var filasAfectadasParam = new OleDbParameter("@filasAfectadas", SqlDbType.Int) { Direction = ParameterDirection.Output };
                                cmd.Parameters.Add(filasAfectadasParam);
                               
                                await cmd.ExecuteNonQueryAsync();

                                 rowsAffected = (int)filasAfectadasParam.Value;
                                mensaje="Registro guardado exitosamente, filas afectadas: " + rowsAffected;
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
                mensaje="Error en la base de datos: "+ ex.Message;
                Console.WriteLine($"Código de error: {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                // Captura general de excepciones
                mensaje = "Error general: " +  ex.Message ;
            }
            return (rowsAffected, mensaje);
        }

     
    }
}
