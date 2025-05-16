using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosAlta;
using DTOs.DtosOuputs.DtosEtiqueta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Interface;
using ServiceStack;
using ServiceStack.Text.Json;
using System.Text.Json;

namespace API_MOV_ALM.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class EtiquetaController : ControllerBase
    {
        public readonly IAltaRepository<Alta> _AltaRepository;
        public readonly IAlmacenRepository<Almacen> _AlmacenRepository;
        public readonly IGeneralRepository<General> _GeneralRepository;

        public EtiquetaController(IAltaRepository<Alta> AltaRepository, IAlmacenRepository<Almacen> AlmacenRepository, IGeneralRepository<General> GeneralRepository)
        {

            _AltaRepository = AltaRepository;
            _AlmacenRepository = AlmacenRepository;
            _GeneralRepository = GeneralRepository;
        }


        [HttpPost("bloquear")]
        public IActionResult BloquearEtiqueta(ProcesoLecturaEtiquetaDtoInputs obj)
        {
            object response = null;
            List<EtiquetaDtoOutput> obj_EtiquetaDtoOutputs = new List<EtiquetaDtoOutput>();

            try
            {
                int resultadoBloquearRegistro = _GeneralRepository.BloquearRegistro2(obj.etiqueta, obj.pcName);
                switch (resultadoBloquearRegistro)
                {
                    case 1:
                    case 8888:
                        List<Almacen> lstAlmacen = _AlmacenRepository.ObtenerRegistro_FMOVALG2_2(obj.etiqueta);
                        int resultvalidarRegistroFMOVALG2 = lstAlmacen.Count();

                        if (resultvalidarRegistroFMOVALG2 == 0)
                        {
                            response = new
                            {
                                success = false,
                                message = "Etiqueta no tiene ingreso en detalle de movimiento",
                                result = obj_EtiquetaDtoOutputs

                            };
                        }
                        else
                        {

                            List<Etiqueta> obj_ListaEtiqueta = new List<Etiqueta>();
                            obj_ListaEtiqueta = _GeneralRepository.ObtenerDatosEtiqueta2(obj.etiqueta);

                            if (obj_ListaEtiqueta.Count == 0)
                            {
                                response = new
                                {
                                    success = false,
                                    message = "La etiqueta no existe",
                                    result = obj_EtiquetaDtoOutputs

                                };
                            }
                            else
                            {
                                if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                && obj_ListaEtiqueta[0].tmvma1 == "I"
                                && (obj_ListaEtiqueta[0].tmvmag == "I"
                                    || obj_ListaEtiqueta[0].tmvmag == "S")
                                && (int)obj_ListaEtiqueta[0].codalg == Convert.ToInt32(obj.almacen)
                                )
                                {
                                    if (obj.listCodExis.Contains(obj_ListaEtiqueta[0].codexi.ToString()))
                                    {
                                        string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);
                                        if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                        {
                                            foreach (Etiqueta obj_L in obj_ListaEtiqueta)
                                            {
                                                EtiquetaDtoOutput obj_EtiquetaDtoOutput = new EtiquetaDtoOutput();
                                                obj_EtiquetaDtoOutput.codexi = obj_L.codexi;
                                                obj_EtiquetaDtoOutput.codchi = obj_L.codchi;
                                                obj_EtiquetaDtoOutput.nlhmag = obj_L.nlhmag;
                                                obj_EtiquetaDtoOutput.cremang = obj_L.cremang;
                                                obj_EtiquetaDtoOutput.unimed = obj_L.unimed;
                                                obj_EtiquetaDtoOutput.caemag = obj_L.caemag;
                                                obj_EtiquetaDtoOutput.umemag = obj_L.umemag;
                                                obj_EtiquetaDtoOutput.codigo = obj_L.codigo;
                                                obj_EtiquetaDtoOutput.codalg = obj_L.codalg;
                                                obj_EtiquetaDtoOutput.codcia = obj_L.codcia;
                                                obj_EtiquetaDtoOutput.tmvma1 = obj_L.tmvma1;
                                                obj_EtiquetaDtoOutput.tmvmag = obj_L.tmvmag;
                                                obj_EtiquetaDtoOutput.ademag = obj_L.ademag;
                                                obj_EtiquetaDtoOutput.codtex = obj_L.codtex;
                                                obj_EtiquetaDtoOutput.codprv = obj_L.codprv;
                                                obj_EtiquetaDtoOutput.trhmag = obj_L.trhmag;
                                                obj_EtiquetaDtoOutput.urhmag = obj_L.urhmag;
                                                obj_EtiquetaDtoOutput.fecmag = obj_L.fecmag.ToString("dd-MM-yyyy");
                                                obj_EtiquetaDtoOutput.ltomag = obj_L.ltomag;
                                                obj_EtiquetaDtoOutput.peso_unitario = obj_L.cremang / obj_L.caemag;
                                                obj_EtiquetaDtoOutput.desexi = obj_L.desexi;
                                                obj_EtiquetaDtoOutput.destipexi = obj_L.destipexi;

                                                obj_EtiquetaDtoOutputs.Add(obj_EtiquetaDtoOutput);
                                            }
                                            response = new
                                            {
                                                success = true,
                                                message = "Datos de Etiqueta Obtenidos",
                                                result = obj_EtiquetaDtoOutputs
                                            };

                                        }
                                        else
                                        {
                                            response = new
                                            {
                                                success = false,
                                                message = "El registro está utilizado por: " + PcNameObtenido,
                                                result = obj_EtiquetaDtoOutputs

                                            };
                                        }
                                    }
                                    else
                                    {
                                        _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                        response = new
                                        {
                                            success = false,
                                            message = "Etiqueta pertenece a una existencia distinta a la orden ingresada.",
                                            result = obj_EtiquetaDtoOutputs

                                        };
                                    }
                                }
                                else if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                     && obj_ListaEtiqueta[0].tmvma1 == "I"
                                    && obj_ListaEtiqueta[0].tmvmag == "T"
                                    && (int)obj_ListaEtiqueta[0].ademag == Convert.ToInt32(obj.almacen)
                                    )
                                {
                                    if (obj.listCodExis.Contains(obj_ListaEtiqueta[0].codexi.ToString()))
                                    {
                                        string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);
                                        if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                        {
                                            foreach (Etiqueta obj_L in obj_ListaEtiqueta)
                                            {
                                                EtiquetaDtoOutput obj_EtiquetaDtoOutput = new EtiquetaDtoOutput();
                                                obj_EtiquetaDtoOutput.codexi = obj_L.codexi;
                                                obj_EtiquetaDtoOutput.codchi = obj_L.codchi;
                                                obj_EtiquetaDtoOutput.nlhmag = obj_L.nlhmag;
                                                obj_EtiquetaDtoOutput.cremang = obj_L.cremang;
                                                obj_EtiquetaDtoOutput.unimed = obj_L.unimed;
                                                obj_EtiquetaDtoOutput.caemag = obj_L.caemag;
                                                obj_EtiquetaDtoOutput.umemag = obj_L.umemag;
                                                obj_EtiquetaDtoOutput.codigo = obj_L.codigo;
                                                obj_EtiquetaDtoOutput.codalg = obj_L.codalg;
                                                obj_EtiquetaDtoOutput.codcia = obj_L.codcia;
                                                obj_EtiquetaDtoOutput.tmvma1 = obj_L.tmvma1;
                                                obj_EtiquetaDtoOutput.tmvmag = obj_L.tmvmag;
                                                obj_EtiquetaDtoOutput.ademag = obj_L.ademag;
                                                obj_EtiquetaDtoOutput.codtex = obj_L.codtex;
                                                obj_EtiquetaDtoOutput.codprv = obj_L.codprv;
                                                obj_EtiquetaDtoOutput.trhmag = obj_L.trhmag;
                                                obj_EtiquetaDtoOutput.urhmag = obj_L.urhmag;
                                                obj_EtiquetaDtoOutput.fecmag = obj_L.fecmag.ToString("dd-MM-yyyy");
                                                obj_EtiquetaDtoOutput.ltomag = obj_L.ltomag;
                                                obj_EtiquetaDtoOutput.peso_unitario = obj_L.cremang / obj_L.caemag;
                                                obj_EtiquetaDtoOutput.desexi = obj_L.desexi;
                                                obj_EtiquetaDtoOutput.destipexi = obj_L.destipexi;

                                                obj_EtiquetaDtoOutputs.Add(obj_EtiquetaDtoOutput);
                                            }
                                            response = new
                                            {
                                                success = true,
                                                message = "Datos de Etiqueta Obtenidos",
                                                result = obj_EtiquetaDtoOutputs
                                            };
                                        }
                                        else
                                        {
                                            response = new
                                            {
                                                success = false,
                                                message = "El registro está utilizado por: " + PcNameObtenido,
                                                result = obj_EtiquetaDtoOutputs

                                            };
                                        }
                                    }
                                    else
                                    {
                                        _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                        response = new
                                        {
                                            success = false,
                                            message = "Etiqueta pertenece a una existencia distinta a la orden ingresada.",
                                            result = obj_EtiquetaDtoOutputs

                                        };
                                    }
                                }
                                else
                                {
                                    _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                    response = new
                                    {
                                        success = false,
                                        message = "Etiqueta no pertenece.",
                                        result = obj_EtiquetaDtoOutputs

                                    };

                                }
                            }
                        }
                        break;
                    case 9999:
                        response = new
                        {
                            success = false,
                            message = "No existe el registro.",
                            result = obj_EtiquetaDtoOutputs

                        };
                        break;
                    case 0:

                        response = new
                        {
                            success = false,
                            message = "El registro se encuentra bloqueado por otro equipo.",
                            result = obj_EtiquetaDtoOutputs

                        };
                        break;
                }
            }

            catch (Exception ex)
            {
                Tools.Log.Write(2,
                    "Etiqueta: " + obj.etiqueta + "\n" +
                    "Mensaje de Error: " + ex.Message.ToString() + "\n" +
                    "Input: " + JsonSerializer.Serialize(obj).ToString());

                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
    }
}
