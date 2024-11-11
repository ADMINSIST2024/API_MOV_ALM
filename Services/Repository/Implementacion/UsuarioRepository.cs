using API_MOV_ALM.Models;
using Microsoft.Extensions.Configuration;
using Services.Repository.Interface;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace Services.Repository.Implementacion
{
    public class UsuarioRepository:IUsuarioRepository<Usuario>
    {
        private readonly string? CadenaAS400;
      
        public UsuarioRepository(IConfiguration configuracion)
        {
            CadenaAS400 = configuracion.GetConnectionString("CadenaAS400");
        }



        public async Task<Usuario> IniciarSesion(Usuario user)
        {
            Usuario ObjUsaurio = null;


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
                       
                        using (OleDbCommand cmd = new OleDbCommand("SP_API_LOGIN", con))
                        {
                            cmd.Parameters.AddWithValue("@nomusu", user.NOMUSU);
                            cmd.Parameters.AddWithValue("@pwdusu",Encripta(user.PAWUSU));
                            cmd.CommandType = CommandType.StoredProcedure;
                            var lector = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                            while (await lector.ReadAsync())
                            {
                                ObjUsaurio=new Usuario();
                                ObjUsaurio.NOMUSU = lector[0].ToString().Trim();
                                ObjUsaurio.PAWUSU = lector[1].ToString().Trim();
                                ObjUsaurio.CODPER = Convert.ToInt32(lector[2].ToString().Trim());
                                ObjUsaurio.FASUSU = Convert.ToInt32(lector[3].ToString().Trim());
                                ObjUsaurio.STAUSU = Convert.ToInt32(lector[4].ToString().Trim());

                            }

                            lector.Close();
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(ex, true);
                string Mensaje = ex.Message;
            }
            return ObjUsaurio;
        }

       















        public Task<List<Usuario>> Buscar(Usuario clase)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Usuario clase)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Guardar(Usuario clase)
        {
            throw new NotImplementedException();
        }

       

        public Task<List<Usuario>> Lista()
        {
            throw new NotImplementedException();
        }



        public string Encripta(string clave)
        {
            string llave = "%ü&/@#$A";
            string pass2 = "";

            for (int i = 0; i < clave.Length; i++)
            {
                char car = clave[i];
                char codigo = llave[(i % llave.Length)];

                int xorResult = car ^ codigo;
                pass2 += xorResult.ToString("X2");
            }

            return pass2;
        }

      
        public string Desencripta(string claveEncriptada)
        {
            string llave = "%ü&/@#$A";
            string passOriginal = "";

            for (int i = 0; i < claveEncriptada.Length / 2; i++)
            {
                string subcadena = claveEncriptada.Substring(i * 2, 2);
                int xorResult = Convert.ToInt32(subcadena, 16);
                char codigo = llave[(i % llave.Length)];

                char car = (char)(xorResult ^ codigo);
                passOriginal += car;
            }

            return passOriginal;
        }

      


    }
}
