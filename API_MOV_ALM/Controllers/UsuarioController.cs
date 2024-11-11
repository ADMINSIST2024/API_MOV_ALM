using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosUsuarioInputs;
using DTOs.DtosOuputs.DtoUsuarioOutputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Repository.Implementacion;
using Services.Repository.Interface;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace API_MOV_ALM.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly IUsuarioRepository<Usuario> _UsuarioRepository;
        public readonly ICompañiaRepository<Compañia> _CompañiaRepository;
        public readonly IJwtRepository<Jwt> _JwtRepository;
        public UsuarioController(IUsuarioRepository<Usuario> UsuarioRepository, IConfiguration configuration,
            ICompañiaRepository<Compañia> CompañiaRepository, IJwtRepository<Jwt> JwtRepository)
        {
            _UsuarioRepository = UsuarioRepository;
            _CompañiaRepository =CompañiaRepository;
            _configuration = configuration;
            _JwtRepository = JwtRepository;
        }
          

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> IniciarSesion(LoginDtoInputs user)
        {
            object response; string token = ""; 
            Usuario obj_usuario = new Usuario();
            obj_usuario.NOMUSU = user.NOMUSU;
            obj_usuario.PAWUSU = user.PAWUSU;

            Jwt obj_jwt = new Jwt();
            try
            {
                 obj_usuario = await _UsuarioRepository.IniciarSesion(obj_usuario);

                if (obj_usuario == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Credenciales incorrectas",
                        result = ""
                    });
                }

                LoginDtoOutputs Obj_LoginDtoOutputs =new LoginDtoOutputs();
                Obj_LoginDtoOutputs.CODPER = obj_usuario.CODPER;             
                Obj_LoginDtoOutputs.NOMUSU = obj_usuario.NOMUSU;
                obj_jwt.CodigoUsuario =obj_usuario.CODPER;
                obj_jwt.NombreUsuario = obj_usuario.NOMUSU;

              //  token = _JwtRepository.GenerarToken(obj_jwt);
               
                response = new
                {
                    success = true,
                    message = "Login validado Correctamente",
                    result = new
                    {
                        //token = token,
                        datos = Obj_LoginDtoOutputs
                    }
                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new{success = false,message = "Error Catch: "+ ex.Message, StackTrace = ex.StackTrace, result = ""});
            }

            return new JsonResult(response);
        }

      

        [HttpPost]
        [Route("Encripta")]
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

        [HttpPost]
        [Route("Desencripta")]
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
