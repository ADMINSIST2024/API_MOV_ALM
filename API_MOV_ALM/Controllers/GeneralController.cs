using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosCentroCostoInputs;
using DTOs.DtosInputs.DtosEtiqueta;
using DTOs.DtosInputs.DtosGeneralInputs;
using DTOs.DtosOuputs.DtosCentroCostoOutputs;
using DTOs.DtosOuputs.DtosEtiqueta;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using DTOs.DtosOuputs.DtoUsuarioOutputs;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_MOV_ALM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GeneralController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly IGeneralRepository<General> _GeneralRepository;
        public readonly IUsuarioRepository<Usuario> _UsuarioRepository;

        public GeneralController(IConfiguration configuration, IGeneralRepository<General> GeneralRepository, IUsuarioRepository<Usuario> UsuarioRepository)
        {

            _GeneralRepository = GeneralRepository;
            _configuration = configuration;
            _UsuarioRepository = UsuarioRepository;
        }

        /*
        [HttpGet]
        [Route("ObtenerFechaSistema")]
        public async Task<IActionResult> ObtenerFechaSistema()
        {
            object response; string token = "";
           
            General obj_General = new General();

           
            try
            {
                obj_General = await _GeneralRepository.ObtenerFechaSistema();

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo la Fecha del Sistema",
                        result = ""
                    });
                }

                ObtenerFechaSistemaDtoOutputs Obj = new ObtenerFechaSistemaDtoOutputs();
                Obj.fechaSistema = obj_General.fechaSistema;
               

            
                response = new
                {
                    success = true,
                    message = "Fecha Obtenida Satisfactoriamente",
                    result = Obj
                   
                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
             */   


        [HttpPost]
        [Route("ObtenerDatosEtiqueta")]
        public async Task<IActionResult> ObtenerDatosEtiqueta(EtiquetaDtoInputs obj)
        {
            Etiqueta objcc = new Etiqueta();
            objcc.codEtiqueta = obj.codEtiqueta;


            List<Etiqueta> obj_Lista = new List<Etiqueta>();
            List<EtiquetaDtoOutput> obj_EtiquetaDtoOutputs = new List<EtiquetaDtoOutput>();
            try
            {
                obj_Lista = await _GeneralRepository.ObtenerDatosEtiqueta(objcc);

                if (obj_Lista == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro datos de la Etiqueta",
                        result = ""
                    });
                }
                else
                {

                    foreach (Etiqueta obj_L in obj_Lista)
                    {
                        EtiquetaDtoOutput obj_EtiquetaDtoOutput = new EtiquetaDtoOutput();
                        obj_EtiquetaDtoOutput.codexi= obj_L.codexi;
                        obj_EtiquetaDtoOutput.codchi= obj_L.codchi;
                        obj_EtiquetaDtoOutput.nlhmag= obj_L.nlhmag;
                        obj_EtiquetaDtoOutput.cremang = obj_L.cremang;
                        obj_EtiquetaDtoOutput.unimed= obj_L.unimed;
                        obj_EtiquetaDtoOutput.caemag= obj_L.caemag;
                        obj_EtiquetaDtoOutput.umemag= obj_L.umemag;
                        obj_EtiquetaDtoOutput.codigo= obj_L.codigo;
                        obj_EtiquetaDtoOutput.codalg= obj_L.codalg;
                        obj_EtiquetaDtoOutput.codcia= obj_L.codcia;
                        obj_EtiquetaDtoOutput.tmvma1= obj_L.tmvma1;
                        obj_EtiquetaDtoOutput.tmvmag= obj_L.tmvmag;
                        obj_EtiquetaDtoOutput.ademag= obj_L.ademag;
                        obj_EtiquetaDtoOutput.codtex= obj_L.codtex;
                        obj_EtiquetaDtoOutput.codprv= obj_L.codprv;
                        obj_EtiquetaDtoOutput.trhmag= obj_L.trhmag;
                        obj_EtiquetaDtoOutput.urhmag= obj_L.urhmag;
                        obj_EtiquetaDtoOutput.fecmag=obj_L.fecmag.ToString("dd-MM-yyyy");
                        obj_EtiquetaDtoOutput.ltomag = obj_L.ltomag;


                        obj_EtiquetaDtoOutputs.Add(obj_EtiquetaDtoOutput);
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
                message = "Datos de Etiqueta Obtenidos",
                result = obj_EtiquetaDtoOutputs
            });
        }

        [HttpPost]
        [Route("BloquearRegistroNull")]
        public async Task<IActionResult> BloquearRegistroNull(BloquearRegistroNullDtoInputs obj)
        {
            object response=null; 

            General obj_General = new General();

            obj_General.codigoEtiqueta = obj.codigoEtiqueta;    
            obj_General.pcName= obj.pcName;

            try
            {
               int res =  await _GeneralRepository.BloquearRegistroNull(obj_General);
                if (res == 1)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro bloqueado satisfactoriamente",
                        result = res

                    };
                }
                else if (res == 9999)
                {
                    response = new
                    {
                        success = false,
                        message = "Registro no existe",
                        result = res

                    };
                }
                else if (res == 0)
                {
                    response = new
                    {
                        success = false,
                        message = "Registro se encuentra bloqueado",
                        result = res

                    };
                }
               

            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("BloquearRegistro")]
        public async Task<IActionResult> BloquearRegistro(BloquearRegistroNullDtoInputs obj)
        {
            object response=null;

            General obj_General = new General();

            obj_General.codigoEtiqueta = obj.codigoEtiqueta;
            obj_General.pcName = obj.pcName;

            try
            {
                int res = await _GeneralRepository.BloquearRegistro(obj_General);
                if (res == 8888)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro bloqueado por el mismo dispositivo",
                        result = res

                    };
                }
                else if(res == 9999)
                {
                    response = new
                    {
                        success = false,
                        message = "Registro no encontrado",
                        result = res

                    };
                }
                else if (res == 0)
                {
                    response = new
                    {
                        success = false,
                        message = "Registro Bloqueado por otro dispositivo",
                        result = res

                    };
                }
                else if (res == 1)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Bloqueado satisfactoriamente",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }


        [HttpPost]
        [Route("UtilizaRegistro")]
        public async Task<IActionResult> UtilizaRegistro(UtilizaRegistroDtoInputs obj)
        {
            object response; string token = "";

            General obj_General = new General();
            obj_General.codigoEtiqueta = obj.codigoEtiqueta;

            try
            {
                obj_General = await _GeneralRepository.UtilizaRegistro(obj_General);

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontró datos",
                        result = ""
                    });
                }

                UtilizaRegistroDtoOutputs Obj = new UtilizaRegistroDtoOutputs();
                Obj.pcName = obj_General.pcName;



                response = new
                {
                    success = true,
                    message = "Nombre de PC Obtenida Satisfactoriamente",
                    result = Obj

                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("ValidarTipoListado")]
        public async Task<IActionResult> ValidarTipoListado(ValidarTipoListadoDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.nroOrden = obj.nroOrden;
           

            try
            {
                int res = await _GeneralRepository.ValidarTipoListado(obj_General);
                if (res >0)
                {
                    response = new
                    {
                        success = true,
                        message = "Se encontró Registro",
                        result = res

                    };
                }
                else 
                {
                    response = new
                    {
                        success = false,
                        message = "No se encontró Registro",
                        result = res

                    };
                }
               
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DesbloquearRegistro")]
        public async Task<IActionResult> DesbloquearRegistro(DesbloquearRegistroDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.codigoEtiqueta = obj.codigoEtiqueta;
           

            try
            {
                int res = await _GeneralRepository.DesbloquearRegistro(obj_General);
                if (res == 1)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro desbloqueado",
                        result = res

                    };
                }
                else if (res == 0)
                {
                    response = new
                    {
                        success = false,
                        message = "Advertencia: No se encontró ningún registro para actualizar",
                        result = res

                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = "Error: La operación falló",
                        result = res

                    };
                }
               
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        /*
        [HttpGet]
        [Route("ObtenerHoraSistema")]
        public async Task<IActionResult> ObtenerHoraSistema()
        {
            object response; string token = "";

            General obj_General = new General();


            try
            {
                obj_General = await _GeneralRepository.ObtenerHoraSistema();

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo la Hora del Sistema",
                        result = ""
                    });
                }

                ObtenerHoraSistemaDtoOutputs Obj = new ObtenerHoraSistemaDtoOutputs();
                Obj.horaSistema = obj_General.horaSistema;
                Obj.minutoSistema = obj_General.minutoSistema;
                Obj.segundoSistema = obj_General.segundoSistema;


                response = new
                {
                    success = true,
                    message = "Hora Obtenida Satisfactoriamente",
                    result = Obj

                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        */
        /*
        [HttpGet]
        [Route("ObtenerFechaSistemaAyer")]
        public async Task<IActionResult> ObtenerFechaSistemaAyer()
        {
            object response; string token = "";

            General obj_General = new General();


            try
            {
                obj_General = await _GeneralRepository.ObtenerFechaSistemaAyer();

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo la Fecha del Sistema",
                        result = ""
                    });
                }

                ObtenerFechaSistemaDtoOutputs Obj = new ObtenerFechaSistemaDtoOutputs();
                Obj.fechaSistema = obj_General.fechaSistema;



                response = new
                {
                    success = true,
                    message = "Fecha Obtenida Satisfactoriamente",
                    result = Obj

                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        */
        /*
        [HttpGet]
        [Route("ObtenerDiasPermitidos")]
        public async Task<IActionResult> ObtenerDiasPermitidos()
        {
            object response; string token = "";

            General obj_General = new General();


            try
            {
                obj_General = await _GeneralRepository.ObtenerDiasPermitidos();

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo dias permitidos",
                        result = ""
                    });
                }

                ObtenerDiasPermitidosDtoOutputs Obj = new ObtenerDiasPermitidosDtoOutputs();
                Obj.fecMen = obj_General.fecMen;
                Obj.fecAct = obj_General.fecAct;
        


                response = new
                {
                    success = true,
                    message = "Dias permitidos obtenidos Satisfactoriamente",
                    result = Obj

                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        */
        /*
        [HttpGet]
        [Route("ObtenerDiasPermitidosAyer")]
        public async Task<IActionResult> ObtenerDiasPermitidosAyer()
        {
            object response; string token = "";

            General obj_General = new General();


            try
            {
                obj_General = await _GeneralRepository.ObtenerDiasPermitidosAyer();

                if (obj_General == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo dias permitidos",
                        result = ""
                    });
                }

                ObtenerDiasPermitidosDtoOutputs Obj = new ObtenerDiasPermitidosDtoOutputs();
                Obj.fecMen = obj_General.fecMen;
                Obj.fecAct = obj_General.fecAct;



                response = new
                {
                    success = true,
                    message = "Dias permitidos obtenidos Satisfactoriamente",
                    result = Obj

                };
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        */
        /*
        [HttpPost]
        [Route("ObtenerDatosStockEmpaque")]
        public async Task<IActionResult> ObtenerDatosStockEmpaque(ObtenerDatosStockEmpaqueDtoInputs obj)
        {
            object response = null; 

            General obj_General = new General();

            obj_General.codigoEtiquetas = obj.codigoEtiquetas;
            List<General> obj_Lista = new List<General>();
            List<ObtenerDatosStockEmpaqueDtoOutput> obj_ObtenerDatosStockEmpaqueDtoOutputs = new List<ObtenerDatosStockEmpaqueDtoOutput>();
            try
            {
                obj_Lista = await _GeneralRepository.ObtenerDatosStockEmpaque(obj_General);
                if (obj_Lista == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro datos del Stock",
                        result = ""
                    });
                }
                else
                {

                    foreach (General obj_L in obj_Lista)
                    {
                        ObtenerDatosStockEmpaqueDtoOutput obj_DtoOutput = new ObtenerDatosStockEmpaqueDtoOutput();
                        obj_DtoOutput.canmag = obj_L.canmag;
                        obj_DtoOutput.cremag = obj_L.cremag;
                        obj_DtoOutput.codigoEtiqueta = obj_L.codigoEtiqueta;
                        obj_DtoOutput.scoma1 = obj_L.scoma1;
                        obj_DtoOutput.stockreal = obj_L.stockreal;
                       



                        obj_ObtenerDatosStockEmpaqueDtoOutputs.Add(obj_DtoOutput);
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
                message = "Datos de stock Obtenidos",
                result = obj_ObtenerDatosStockEmpaqueDtoOutputs
            });
        }
        */
        /*
        [HttpPost]
        [Route("BloquearRegistroCorrelativoNull")]
        public async Task<IActionResult> BloquearRegistroCorrelativoNull(BloquearRegistroCorrelativoNullDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.pcName = obj.pcName;
            obj_General.codCia = obj.codCia;
            obj_General.codAlg = obj.codAlg;
            obj_General.tmvcor = obj.tmvcor;

            try
            {
                int res = await _GeneralRepository.BloquearRegistroCorrelativoNull(obj_General);
                if (res > 0)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Bloqueado",
                        result = res

                    };
                }              
                
                else 
                {
                    response = new
                    {
                        success = true,
                        message = "Registro no Bloqueado",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        */

        [HttpPost]
        [Route("BloquearRegistroCorrelativo")]
        public async Task<IActionResult> BloquearRegistroCorrelativo(BloquearRegistroCorrelativoNullDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.pcName = obj.pcName;
            obj_General.codCia = obj.codCia;
            obj_General.codAlg = obj.codAlg;
            obj_General.tmvcor = obj.tmvcor;

            try
            {
                int res = await _GeneralRepository.BloquearRegistroCorrelativo(obj_General);
                if (res > 0)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Bloqueado",
                        result = res

                    };
                }

                else
                {
                    response = new
                    {
                        success = true,
                        message = "Registro no Bloqueado",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }


        [HttpPost]
        [Route("CriterioTipoExistencia")]
        public async Task<IActionResult> CriterioTipoExistencia(CriterioTipoExistenciaDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.codTex = obj.codTex;
          

            try
            {
                int res = await _GeneralRepository.CriterioTipoExistencia(obj_General);
                if (res > 0)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Obtenido Correctamente",
                        result = res

                    };
                }

                else
                {
                    response = new
                    {
                        success = true,
                        message = "Registro no Obtenido",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        
        [HttpGet]
        [Route("ValidaUsoCorrelativo")]
        public async Task<IActionResult> ValidaUsoCorrelativo()
        {
            object response = null;


            List<General> obj_Lista = new List<General>();
            List<ValidaUsoCorrelativoDtoOutput> obj_ValidaUsoCorrelativo = new List<ValidaUsoCorrelativoDtoOutput>();

            try
            {
                obj_Lista = await _GeneralRepository.ValidaUsoCorrelativo();

                if (obj_Lista == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se Obtuvo datos",
                        result = ""
                    });
                }
                else {

                    foreach (General obj_L in obj_Lista)
                    {
                        ValidaUsoCorrelativoDtoOutput obj_DtoOutput = new ValidaUsoCorrelativoDtoOutput();
                        obj_DtoOutput.pcName = obj_L.pcName;
                        obj_DtoOutput.nCorre = obj_L.nCorre;




                        obj_ValidaUsoCorrelativo.Add(obj_DtoOutput);
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
                message = "Datos Obtenidos",
                result = obj_ValidaUsoCorrelativo
            });
        }

        [HttpPost]
        [Route("ObtenerFMOVALG2")]
        public async Task<IActionResult> ObtenerFMOVALG2(ObtenerFMOVALG2DtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.codigoEtiquetas = obj.codigoEtiqueta;
            List<General> obj_Lista = new List<General>();
            List<ObtenerFMOVALG2DtoOutput> obj_ObtenerFMOVALG2DtoOutput = new List<ObtenerFMOVALG2DtoOutput>();
            try
            {
                obj_Lista = await _GeneralRepository.ObtenerFMOVALG2(obj_General);
                if (obj_Lista == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "No se encontro datos ",
                        result = ""
                    });
                }
                else
                {

                    foreach (General obj_L in obj_Lista)
                    {
                        ObtenerFMOVALG2DtoOutput obj_DtoOutput = new ObtenerFMOVALG2DtoOutput();
                        

                        obj_DtoOutput.tmamag = obj_L.tmamag;
                        obj_DtoOutput.nmamag = obj_L.nmamag;
                        obj_DtoOutput.csamag = obj_L.csamag;
                        obj_DtoOutput.seama2 = obj_L.seama2;
                        obj_DtoOutput.alamag = obj_L.alamag;
                        obj_DtoOutput.codtex = obj_L.codtex;
                        obj_DtoOutput.codexi = obj_L.codexi;
                        obj_DtoOutput.codprv = obj_L.codprv;
                        obj_DtoOutput.nlhmag = obj_L.nlhmag;
                        obj_DtoOutput.trhmag = obj_L.trhmag;
                        obj_DtoOutput.urhmag = obj_L.urhmag;
                        obj_DtoOutput.canmag = obj_L.canmag;
                        obj_DtoOutput.canmag = obj_L.canmag;


                        obj_ObtenerFMOVALG2DtoOutput.Add(obj_DtoOutput);
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
                message = "Datos Obtenidos",
                result = obj_ObtenerFMOVALG2DtoOutput
            });
        }

        [HttpPost]
        [Route("DesbloquearTABCORRE")]
        public async Task<IActionResult> DesbloquearTABCORRE(DesbloquearTABCORREDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

          
            obj_General.codCia = obj.codCia;
         

            try
            {
                int res = await _GeneralRepository.DesbloquearTABCORRE(obj_General);
                if (res > 0)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Desbloqueado",
                        result = res

                    };
                }

                else
                {
                    response = new
                    {
                        success = true,
                        message = "Registro no Desbloqueado",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("DesbloquearFABCORRE")]
        public async Task<IActionResult> DesbloquearFABCORRE(DesbloquearFABCORREDtoInputs obj)
        {
            object response = null;

            General obj_General = new General();

            obj_General.codCia = obj.codcia;
            obj_General.codAlg = obj.codalg;
                obj_General.tmvcor = obj.tmvcor;

            try
            {
                int res = await _GeneralRepository.DesbloquearFABCORRE(obj_General);
                if (res > 0 )
                {
                    response = new
                    {
                        success = true,
                        message = "Registro actualizado correctamente",
                        result = res

                    };
                }
                else 
                {
                    response = new
                    {
                        success = false,
                        message = "Registro no actualizado",
                        result = res

                    };
                }
              
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }


        [HttpPost]
        [Route("ValidarLogin")]
        public async Task<IActionResult> ValidarLogin(ValidarLoginDtoInputs obj)
        {
            General obj_general = new General();
            obj_general.nomUsu = obj.usuario;
            obj_general.pawUsu = Encripta(obj.clave);
            object response = null;

            General obj_General = new General();
            ValidarLoginDtoOutputs obj_ValidarLogin = new ValidarLoginDtoOutputs();
            try
            {
                obj_General =  _GeneralRepository.ValidarLogin(obj_general);

                if (obj_General == null || obj_General.nomUsu == "" || obj_General.nomUsu == null)
                {
                    response = new
                    {
                        success = false,
                        message = "Error de usuario y clave.",
                        result = obj_ValidarLogin

                    };
                }
                else
                {
                    obj_ValidarLogin.nomUsu = obj_General.nomUsu;
                    obj_ValidarLogin.pawUsu = obj_General.pawUsu;
                    obj_ValidarLogin.codPer = obj_General.codPer;
                    obj_ValidarLogin.faUsu = obj_General.faUsu;
                    obj_ValidarLogin.staUsu = obj_General.staUsu;

                    if (obj_ValidarLogin.staUsu == 9)
                    {
                        response = new
                        {
                            success = false,
                            message = "Usuario esta de baja.",
                            result = obj_ValidarLogin

                        };
                    }
                    else if (obj_ValidarLogin.faUsu == 0)
                    {
                        response = new
                        {
                            success = false,
                            message = "Usuario no autorizado.",
                            result = obj_ValidarLogin

                        };

                    }
                    else {
                       

                        response = new
                        {
                            success = true,
                            message = "Login Exitoso",
                            result = obj_ValidarLogin

                        };
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("validarNroOrden")]
        public async Task<IActionResult> validarNroOrden(NroOrdenDtoInputs obj)
        {
            object response = null;
            General objcc = new General();
            objcc.nroOrden = obj.nroOrden;


            List<General> obj_Lista = new List<General>();
            List<NroOrdenDtoOutput> obj_NroOrdenDtoOutputs = new List<NroOrdenDtoOutput>();

            try
            {
                obj_Lista =   _GeneralRepository.ObtenerDatosOrden(objcc);

                if (obj_Lista.Count() > 0)
                {


                    foreach (General obj_L in obj_Lista)
                    {
                        NroOrdenDtoOutput obj_NroOrden = new NroOrdenDtoOutput();
                        obj_NroOrden.tipoMovimiento = obj_L.tipoMovimiento;
                        obj_NroOrden.anio = obj_L.anio;
                        obj_NroOrden.codigoProceso = obj_L.codigoProceso;
                        obj_NroOrden.descripcionProceso = obj_L.descripcionProceso;
                        obj_NroOrden.nroCarga =obj_L.nroCargaMaxima;
                        obj_NroOrden.estadoOrden = obj_L.estadoOrden;


                        obj_NroOrdenDtoOutputs.Add(obj_NroOrden);
                    }


                    response = new
                    {
                        success = true,
                        message = "Datos encontrados",
                        result = obj_NroOrdenDtoOutputs
                    };
                }
                else
                {

                    response = new
                    {
                        success = false,
                        message = "Nº orden no existe.",
                        result = obj_NroOrdenDtoOutputs
                    };

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("validarEstadoCarga")]
        public async Task<IActionResult> validarEstadoCarga(NroCargaDtoInputs obj)
        {
            object response = null;
            General objcc = new General();
            objcc.nroOrden = obj.nroOrden;
            objcc.nroCarga = obj.nroCarga;

            List<General> obj_Lista = new List<General>();
            List<NroCargaDtoOutput> obj_NroCargaDtoOutput = new List<NroCargaDtoOutput>();

            try
            {
                obj_Lista = _GeneralRepository.ObtenerEstadoCarga(objcc);

                if (obj_Lista.Count() > 0)
                {

                   int estado= obj_Lista[0].estadoCarga;


                    if (estado < 2)
                    {

                        response = new
                        {
                            success = true,
                            message = "Carga Abierta.",
                            result = obj_NroCargaDtoOutput
                        };
                    }
                    else
                    {
                        response = new
                        {
                            success = false,
                            message = "No se puede realizar el movimiento ,ya que el Nº Carga esta cerrada.",
                            result = obj_NroCargaDtoOutput
                        };
                    }

                  



                }
                else
                {

                    response = new
                    {
                        success = true,
                        message = "Carga Abierta.",
                        result = obj_NroCargaDtoOutput
                    };

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }


        [HttpPost]
        [Route("validarEstadoConsecutivo")]
        public async Task<IActionResult> validarEstadoConsecutivo(ConsecutivoDtoInputs obj)
        {
            object response = null;
            General objcc = new General();
            objcc.nroOrden = obj.nroOrden;
            objcc.tipoMovimiento = obj.tipoMovimiento;
            objcc.consecutivo = obj.consecutivo;

            List<General> obj_Lista = new List<General>();
            List<NroCargaDtoOutput> obj_NroCargaDtoOutput = new List<NroCargaDtoOutput>();

            try
            {
                obj_Lista = _GeneralRepository.ExisteConsecutivo(objcc);

                if (obj_Lista.Count() > 0)
                {

                    String estado = obj_Lista[0].estadoConsecutivo;


                    if (estado =="N")
                    {

                        response = new
                        {
                            success = false,
                            message = "El consecutivo no existe.",
                            result ="N"
                        };
                    }
                    else
                    {
                        response = new
                        {
                            success = true,
                            message = "El consecutivo existe.",
                            result = "S"
                        };
                    }





                }
               
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }











        private string Encripta(string clave)
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


        private string Desencripta(string claveEncriptada)
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
