using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using System.Text;
using System.Text.Json;
using ESD.Core.Service.Converter;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/Comprobante")]
	public class ComprobanteController : AuthorizedController
	{
		private readonly IComprobanteBusiness comprobanteBusiness;

		public ComprobanteController(
			IComprobanteBusiness comprobanteBusiness
			)
		{
			this.comprobanteBusiness = comprobanteBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				ComprobanteDTO comprobanteDTO = comprobanteBusiness.GetById(dto.Id);
				serviceResponse.Data = comprobanteDTO;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Search")]
		public ActionResult<ServiceResponse> Search([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.Search(dto);
				serviceResponse.Data = comprobanteDTOList;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("SingleSearch")]
		public ActionResult<ServiceResponse> SingleSearch([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				ComprobanteDTO comprobanteDTO = comprobanteBusiness.SingleSearch(dto);
				serviceResponse.Data = comprobanteDTO;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Insert")]
		public ActionResult<ServiceResponse> Insert([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				int newId = comprobanteBusiness.Insert(dto);
				serviceResponse.Data = newId;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Update")]
		public ActionResult<ServiceResponse> Update([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				int id = comprobanteBusiness.Update(dto);
				serviceResponse.Data = id;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Delete")]
		public ActionResult<ServiceResponse> Delete([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.Delete(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("AplicarTasaPorMedioPago")]
		public ActionResult<ServiceResponse> AplicarTasaPorMedioPago([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.AplicarTasaPorMedioPago(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Emitir")]
		public ActionResult<ServiceResponse> Emitir([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.Emitir(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("Cerrar")]
		public ActionResult<ServiceResponse> Cerrar([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.Cerrar(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarEmisionElectronica")]
		public ActionResult<ServiceResponse> ActualizarEmisionElectronica([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.ActualizarEmisionElectronica(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ListarDocumentoEmitido")]
		public ActionResult<ServiceResponse> ListarDocumentoEmitido([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.ListarDocumentoEmitido(dto);
				serviceResponse.Data = comprobanteDTOList;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarEstadoSunat")]
		public ActionResult<ServiceResponse> ActualizarEstadoSunat([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.ActualizarEstadoSunat(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ListarResumenEstadoSunat")]
		public ActionResult<ServiceResponse> ListarResumenEstadoSunat([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.ListarResumenEstadoSunat(dto);
				serviceResponse.Data = comprobanteDTOList;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarPorComprobanteElectronicoIdList")]
		public ActionResult<ServiceResponse> ActualizarPorComprobanteElectronicoIdList([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.ActualizarPorComprobanteElectronicoIdList(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("EmitirCanje")]
		public ActionResult<ServiceResponse> EmitirCanje([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.EmitirCanje(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("EliminarCanje")]
		public ActionResult<ServiceResponse> EliminarCanje([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.EliminarCanje(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarDetraccion")]
		public ActionResult<ServiceResponse> ActualizarDetraccion([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.ActualizarDetraccion(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarTipoComprobante")]
		public ActionResult<ServiceResponse> ActualizarTipoComprobante([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.ActualizarTipoComprobante(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("AgregarComprobanteAnticipo")]
		public ActionResult<ServiceResponse> AgregarComprobanteAnticipo([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = comprobanteBusiness.AgregarComprobanteAnticipo(dto);
				serviceResponse.Data = newId;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ActualizarComprobanteAnticipo")]
		public ActionResult<ServiceResponse> ActualizarComprobanteAnticipo([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				int id = comprobanteBusiness.ActualizarComprobanteAnticipo(dto);
				serviceResponse.Data = id;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("AnularComprobanteAnticipo")]
		public ActionResult<ServiceResponse> AnularComprobanteAnticipo([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();;

			try
			{
				comprobanteBusiness.AnularComprobanteAnticipo(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("ListarCobranzaGrid")]
		public ActionResult<ServiceResponse> ListarCobranzaGrid([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = comprobanteBusiness.ListarCobranza(dto);

				JsonSerializerOptions jsonSerializerOptions = new();
				jsonSerializerOptions.Converters.Add(new ShortDateTimeConverter());
				jsonSerializerOptions.WriteIndented = true;
				jsonSerializerOptions.PropertyNameCaseInsensitive = true;

				StringBuilder sb = new();

				sb.Append("{\"total\":1,");
				sb.Append("\"page\":1,");
				sb.Append("\"records\":" + dynamicResult.Count.ToString() + ",");
				sb.Append("\"rows\":[");

				int index = 1;
				foreach (var item in dynamicResult)
				{
					sb.Append("{\"id\":" + index.ToString() + ",");
					sb.Append("\"cell\":");
					sb.Append(JsonSerializer.Serialize<dynamic>(item, jsonSerializerOptions));
					sb.Append('}');

					if (index != dynamicResult.Count)
						sb.Append(',');
					index++;
				}
				sb.Append("]}");

				serviceResponse.Data = sb.ToString();
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("BuscarDespacho")]
		public ActionResult<ServiceResponse> BuscarDespacho([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.BuscarDespacho(dto);
				serviceResponse.Data = comprobanteDTOList;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("CerrarDespacho")]
		public ActionResult<ServiceResponse> CerrarDespacho([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				comprobanteBusiness.CerrarDespacho(dto);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}
		[HttpPost("BuscarPorEmpresaSede")]
		public ActionResult<ServiceResponse> BuscarPorEmpresaSede([FromBody] ComprobanteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				List<ComprobanteDTO> comprobanteDTOList = comprobanteBusiness.BuscarPorEmpresaSede(dto);
				serviceResponse.Data = comprobanteDTOList;
			}
			catch (Exception e)
			{
				Logger.Error(e);
				serviceResponse.Success = false;
				serviceResponse.Message = e.Message;
			}

			Logger.InfoFinish();
			return serviceResponse;
		}

	}
}
