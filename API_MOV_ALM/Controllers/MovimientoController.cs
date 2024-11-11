using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosGeneralInputs;
using DTOs.DtosInputs.DtosMovimientoAlmacenInputs;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using DTOs.DtosOuputs.DtosMovimientoAlmacenOutputs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;

namespace API_MOV_ALM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly IMovimientoRepository<Movimientos> _MovimientoRepository;


        public MovimientoController(IConfiguration configuration, IMovimientoRepository<Movimientos> MovimientoRepository)
        {

            _MovimientoRepository = MovimientoRepository;
            _configuration = configuration;
         
        }




        [HttpPost]
        [Route("ConsultaMovimiento")]
        public async Task<IActionResult> ConsultaMovimiento(ConsultaMovimientoDtoInputs obj)
        {
            object response = null;
            Movimientos objcc = new Movimientos();
            objcc.fecmag = obj.fecmag;
            objcc.codalg = obj.codalg;
            objcc.codexi = obj.codexi;


            List<Movimientos> obj_Lista = new List<Movimientos>();
            List<ConsultaMovimientoDtoOutput> obj_ConsultaMovimientoDtoOutputs = new List<ConsultaMovimientoDtoOutput>();

            try
            {
                obj_Lista = _MovimientoRepository.ConsultaMovimientos(objcc);

                if (obj_Lista.Count() > 0)
                {


                    foreach (Movimientos obj_L in obj_Lista)
                    {
                        ConsultaMovimientoDtoOutput obj_ConsultaMovimiento = new ConsultaMovimientoDtoOutput();
                        obj_ConsultaMovimiento.mov = obj_L.mov;
                        obj_ConsultaMovimiento.cscmag = obj_L.cscmag;
                        obj_ConsultaMovimiento.codalg = obj_L.codalg;
                        obj_ConsultaMovimiento.ademag = obj_L.ademag;
                        obj_ConsultaMovimiento.orden_csc = obj_L.orden_csc;
                        obj_ConsultaMovimiento.caemag = obj_L.caemag;
                        obj_ConsultaMovimiento.canmag = obj_L.canmag;
                        obj_ConsultaMovimiento.codexi = obj_L.codexi;
                        obj_ConsultaMovimiento.codchi = obj_L.codchi;
                        obj_ConsultaMovimiento.codprv = obj_L.codprv;
                        obj_ConsultaMovimiento.nlhmag = obj_L.nlhmag;
                        obj_ConsultaMovimiento.trhmag = obj_L.trhmag;
                        obj_ConsultaMovimiento.urhmag = obj_L.urhmag;
                        obj_ConsultaMovimiento.codigo = obj_L.codigo;
                        obj_ConsultaMovimiento.codtmv = obj_L.codtmv;
                        obj_ConsultaMovimiento.cencos = obj_L.cencos;


                        obj_ConsultaMovimientoDtoOutputs.Add(obj_ConsultaMovimiento);
                    }


                    response = new
                    {
                        success = true,
                        message = "Datos encontrados",
                        result = obj_ConsultaMovimientoDtoOutputs
                    };
                }
                else
                {

                    response = new
                    {
                        success = false,
                        message = "No se encontraron datos",
                        result = obj_ConsultaMovimientoDtoOutputs
                    };

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }



    }
}
