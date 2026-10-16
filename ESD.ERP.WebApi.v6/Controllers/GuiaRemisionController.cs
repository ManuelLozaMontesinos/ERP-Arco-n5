using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/GuiaRemision")]
	public class GuiaRemisionController : AuthorizedController
	{
		private readonly IGuiaRemisionBusiness guiaRemisionBusiness;

		public GuiaRemisionController(
			IGuiaRemisionBusiness guiaRemisionBusiness
			)
		{
			this.guiaRemisionBusiness = guiaRemisionBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				GuiaRemisionDTO guiaRemisionDTO = guiaRemisionBusiness.GetById(dto.Id);
				serviceResponse.Data = guiaRemisionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<GuiaRemisionDTO> guiaRemisionDTOList = guiaRemisionBusiness.Search(dto);
				serviceResponse.Data = guiaRemisionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				GuiaRemisionDTO guiaRemisionDTO = guiaRemisionBusiness.SingleSearch(dto);
				serviceResponse.Data = guiaRemisionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = guiaRemisionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = guiaRemisionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.Delete(dto);
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
		[HttpPost("GenerarDesdeComprobante")]
		public ActionResult<ServiceResponse> GenerarDesdeComprobante([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.GenerarDesdeComprobante(dto);
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
		public ActionResult<ServiceResponse> Emitir([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.Emitir(dto);
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
		[HttpPost("EmitirSunat")]
		public ActionResult<ServiceResponse> EmitirSunat([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.EmitirSunat(dto);
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
		[HttpPost("SolicitarModificacion")]
		public ActionResult<ServiceResponse> SolicitarModificacion([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.SolicitarModificacion(dto);
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
		[HttpPost("Aprobar")]
		public ActionResult<ServiceResponse> Aprobar([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.Aprobar(dto);
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
		[HttpPost("Desaprobar")]
		public ActionResult<ServiceResponse> Desaprobar([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.Desaprobar(dto);
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
		[HttpPost("ActualizarEstado")]
		public ActionResult<ServiceResponse> ActualizarEstado([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.ActualizarEstado(dto);
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
		public ActionResult<ServiceResponse> ActualizarEmisionElectronica([FromBody] GuiaRemisionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				guiaRemisionBusiness.ActualizarEmisionElectronica(dto);
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
        [HttpPost("ActualizarPeso")]
        public ActionResult<ServiceResponse> ActualizarPeso([FromBody] GuiaRemisionDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                guiaRemisionBusiness.ActualizarPeso(dto);
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
