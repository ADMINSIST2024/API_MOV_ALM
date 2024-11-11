using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosCompañiaInputs;
using DTOs.DtosOuputs.DtosCompañiaOutputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Repository.Interface;
using System.Net.Sockets;

namespace API_MOV_ALM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {

        public IConfiguration _configuration;
        public readonly IUsuarioRepository<Usuario> _UsuarioRepository;
        public readonly ICompañiaRepository<Compañia> _CompañiaRepository;
        public readonly IJwtRepository<Jwt> _JwtRepository;
        public CompaniaController( IConfiguration configuration,ICompañiaRepository<Compañia> CompañiaRepository)
        {
           
            _CompañiaRepository = CompañiaRepository;
            _configuration = configuration;
         
        }




        [HttpGet]
        [Route("ObtenerCompania")]
        //[Authorize]
        public async Task<IActionResult> ObtenerCompania()
        {
            List<Compañia> obj_compañia = new List<Compañia>();
            List<CompañiaDtoOutputs> obj_compañiaDtoOutputs = new List<CompañiaDtoOutputs>();
            try
            {
                obj_compañia = await _CompañiaRepository.ObtenerCompañia();

                if (obj_compañia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Compañias",
                        result = ""
                    });
                }
                else
                {
                   
                    foreach (Compañia compañia in obj_compañia)
                    {
                        CompañiaDtoOutputs compañiaDtoOutputs = new CompañiaDtoOutputs();
                        compañiaDtoOutputs.CodCia = compañia.CodCia;
                        compañiaDtoOutputs.DesCia = compañia.DesCia;
                        obj_compañiaDtoOutputs.Add(compañiaDtoOutputs);
                    }

                } 
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Compañias Obtenidas",
                result = obj_compañiaDtoOutputs
            });
        }


        [HttpPost]
        [Route("ObtenerCompañia_x_Codigo")]
        //[Authorize]
        public async Task<IActionResult> ObtenerCompañia_x_Codigo(CompañiaDtoInputs comp)
        {
            Compañia objCompañia=new Compañia();
            objCompañia.CodCia=comp.CodCia;

            List<CompañiaDtoOutputs> obj_compañiaDtoOutputs = new List<CompañiaDtoOutputs>();
            List<Compañia> obj_compañia = new List<Compañia>();
            try
            {
              

                obj_compañia = await _CompañiaRepository.ObtenerCompañia_x_Codigo(objCompañia);

                if (obj_compañia == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Compañias",
                        result = ""
                    });
                }
                else {
                    
                    foreach (Compañia compañia in obj_compañia)
                    {
                        CompañiaDtoOutputs compañiaDtoOutputs = new CompañiaDtoOutputs();
                        compañiaDtoOutputs.CodCia = compañia.CodCia;
                        compañiaDtoOutputs.DesCia = compañia.DesCia;
                        obj_compañiaDtoOutputs.Add(compañiaDtoOutputs);
                    }


                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(new
            {
                success = true,
                message = "Compañias Obtenidas",
                result = obj_compañiaDtoOutputs
            });
        }

    }
}
