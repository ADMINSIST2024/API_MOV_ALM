using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosAlmacen;
using DTOs.DtosInputs.DtosMovimientoAlmacenInputs;
using DTOs.DtosOuputs.DtosAlmacen;
using DTOs.DtosOuputs.DtosCompañiaOutputs;
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
    public class AlmacenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly IAlmacenRepository<Almacen> _AlmacenRepository;

        public AlmacenController(IConfiguration configuration, IAlmacenRepository<Almacen> AlmacenRepository)
        {

            _AlmacenRepository = AlmacenRepository;
            _configuration = configuration;

        }



        [HttpGet]
        [Route("ObtenerAlmacen")]
        //[Authorize]
        public async Task<IActionResult> ObtenerAlmacen()
        {
            List<Almacen> obj_almacen = new List<Almacen>();
            List<AlmacenDtoOutputs> obj_almacenDtoOutputs = new List<AlmacenDtoOutputs>();
            try
            {
                obj_almacen = await _AlmacenRepository.ObtenerAlmacen();

                if (obj_almacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Almacen",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen almacen in obj_almacen)
                    {
                        AlmacenDtoOutputs almacenDtoOutputs = new AlmacenDtoOutputs();
                        almacenDtoOutputs.CODALG = almacen.CodAlg;
                        almacenDtoOutputs.DESALG = almacen.DesAlg;
                        obj_almacenDtoOutputs.Add(almacenDtoOutputs);
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
                message = "Almacenes Obtenidos",
                result = obj_almacenDtoOutputs
            });
        }

        [HttpPost]
        [Route("ObtenerAlmacenXCodigo")]
        //[Authorize]
        public async Task<IActionResult> ObtenerAlmacenXCodigo(ObtenerAlmacenXCodigoDtoInputs almacen)
        {
            Almacen objAlmacen = new Almacen();
            objAlmacen.CodAlg = almacen.codigoAlmacen;


            List<Almacen> obj_Listalmacen = new List<Almacen>();
            List<ObtenerAlmacenXCodigoDtoOuputs> obj_ObtenerAlmacenXCodigoDtoOuputs = new List<ObtenerAlmacenXCodigoDtoOuputs>();
            try
            {
                obj_Listalmacen = await _AlmacenRepository.ObtenerAlmacenXCodigo(objAlmacen);

                if (obj_Listalmacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Almacen",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen obj_L in obj_Listalmacen)
                    {
                        ObtenerAlmacenXCodigoDtoOuputs obj = new ObtenerAlmacenXCodigoDtoOuputs();
                        obj.codigoAlmacen = obj_L.CodAlg;
                        obj.descripcionAlmacen = obj_L.DesAlg;
                        obj.requiereNroOrden = obj_L.RnoAlg;
                        obj.requiereCscOrden = obj_L.RcoAlg;
                        obj.tipoDocumento = obj_L.TdcAlg;
                        obj.requiereNroCarga = obj_L.RncAlg;
                        obj.centroCosto = obj_L.CodCos;
                        obj_ObtenerAlmacenXCodigoDtoOuputs.Add(obj);
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
                message = "Almacenes Obtenidos",
                result = obj_ObtenerAlmacenXCodigoDtoOuputs
            });
        }

        
        [HttpPost]
        [Route("ObtenerCorrelativoAlmacen")]
        //[Authorize]
        public async Task<IActionResult> ObtenerCorrelativoAlmacen(ObtenerCorrelativoAlmacenDtoInputs almacen)
        {
            Almacen objAlmacen = new Almacen();
            objAlmacen.CodCia = almacen.codigoCia;
            objAlmacen.CodAlg = almacen.codigoAlmacen;
            objAlmacen.TipoMovimiento = almacen.tipoMovimiento;


            List<Almacen> obj_Listalmacen = new List<Almacen>();
            List<ObtenerCorrelativoAlmacenDtoOuputs> obj_List = new List<ObtenerCorrelativoAlmacenDtoOuputs>();
            try
            {
                obj_Listalmacen = await _AlmacenRepository.ObtenerCorrelativoAlmacen2(objAlmacen);

                if (obj_Listalmacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Correlativo",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen obj_L in obj_Listalmacen)
                    {
                        ObtenerCorrelativoAlmacenDtoOuputs obj = new ObtenerCorrelativoAlmacenDtoOuputs();
                        obj.nota = obj_L.nota;

                        obj_List.Add(obj);
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
                message = "Correlativo Obtenidos",
                result = obj_List
            });
        }
        
        
           [HttpPost]
        [Route("ObtenerRegistroFMVALG2")]
        //[Authorize]
        public async Task<IActionResult> ObtenerRegistro_FMOVALG2(ObtenerRegistroFMOVALG2DtoInputs movimiento)
        {
            Almacen objAlmacen = new Almacen();
            objAlmacen.codigoEtiqueta = movimiento.codEtiqueta;


            List<Almacen> obj_Listalmacen = new List<Almacen>();
            List<ObtenerRegistroFMOVALG2DtoOutputs> obj_ObtenerRegistroFMOVALG2DtoOutputs = new List<ObtenerRegistroFMOVALG2DtoOutputs>();
            try
            {
                obj_Listalmacen = await _AlmacenRepository.ObtenerRegistro_FMOVALG2(objAlmacen);

                if (obj_Listalmacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Almacen",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen obj_L in obj_Listalmacen)
                    {
                        ObtenerRegistroFMOVALG2DtoOutputs obj = new ObtenerRegistroFMOVALG2DtoOutputs();
                        obj.codcia = obj_L.CodCia;
                        obj.codalg = obj_L.CodAlg;
                        obj.tmvmag = obj_L.TipoMovimiento;

                        obj_ObtenerRegistroFMOVALG2DtoOutputs.Add(obj);
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
                message = "Registros de Movimientos de Almacen Obtenidos",
                result = obj_ObtenerRegistroFMOVALG2DtoOutputs
            });
        }

        

        [HttpPost]
        [Route("UtilizaRegistroCorrelativoAlmacen")]
        //[Authorize]
        public async Task<IActionResult> UtilizaRegistroCorrelativoAlmacen(UtilizaRegCorrelativoAlmDtoInputs almacen)
        {
            Almacen objAlmacen = new Almacen();
            objAlmacen.CodCia = almacen.codigoCia;
            objAlmacen.CodAlg = almacen.codigoAlmacen;
            objAlmacen.TipoMovimiento = almacen.tipoMovimiento;


            List<Almacen> obj_Listalmacen = new List<Almacen>();
            List<UtilizaRegCorrelativoAlmDtoOutputs> obj_UtilizaRegCorrelativoAlmacen = new List<UtilizaRegCorrelativoAlmDtoOutputs>();
            try
            {
                obj_Listalmacen = await _AlmacenRepository.UtilizaRegistroCorrelativoAlmacen(objAlmacen);

                if (obj_Listalmacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se obtuvo registros",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen obj_L in obj_Listalmacen)
                    {
                        UtilizaRegCorrelativoAlmDtoOutputs obj = new UtilizaRegCorrelativoAlmDtoOutputs();
                        obj.pcName = obj_L.pcName;


                        obj_UtilizaRegCorrelativoAlmacen.Add(obj);
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
                message = "Registros encontrados satisfactoriamente",
                result = obj_UtilizaRegCorrelativoAlmacen
            });
        }
        
        
        
        [HttpPost]
        [Route("ValidarAlamcenXCcosto")]
        public async Task<IActionResult> ValidarAlamcenXCcosto(ValidarAlamcenXCcostoDtoInputs almacen)
        {
            Almacen objAlmacen = new Almacen();
            objAlmacen.CodAlg = almacen.codAlg;


            List<Almacen> obj_Listalmacen = new List<Almacen>();
            List<ValidarAlamcenXCcostoDtoOuputs> obj_ValidarAlamcenXCcostoDtoOuputs = new List<ValidarAlamcenXCcostoDtoOuputs>();
            try
            {
                obj_Listalmacen = await _AlmacenRepository.ValidarAlamcenXCcosto(objAlmacen);

                if (obj_Listalmacen == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro Almacen",
                        result = ""
                    });
                }
                else
                {

                    foreach (Almacen obj_L in obj_Listalmacen)
                    {
                        ValidarAlamcenXCcostoDtoOuputs obj = new ValidarAlamcenXCcostoDtoOuputs();
                        obj.codAlg = obj_L.CodAlg;
                        obj.codCos = obj_L.CodCos;

                        obj_ValidarAlamcenXCcostoDtoOuputs.Add(obj);
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
                message = "Almacenes Obtenidos",
                result = obj_ValidarAlamcenXCcostoDtoOuputs
            });
        }



    }
}
