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
	[Route("api/AnticipoCliente")]
	public class AnticipoClienteController : AuthorizedController
	{
		private readonly IAnticipoClienteBusiness anticipoClienteBusiness;

		public AnticipoClienteController(
			IAnticipoClienteBusiness anticipoClienteBusiness
			)
		{
			this.anticipoClienteBusiness = anticipoClienteBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AnticipoClienteDTO anticipoClienteDTO = anticipoClienteBusiness.GetById(dto.Id);
				serviceResponse.Data = anticipoClienteDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<AnticipoClienteDTO> anticipoClienteDTOList = anticipoClienteBusiness.Search(dto);
				serviceResponse.Data = anticipoClienteDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AnticipoClienteDTO anticipoClienteDTO = anticipoClienteBusiness.SingleSearch(dto);
				serviceResponse.Data = anticipoClienteDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = anticipoClienteBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = anticipoClienteBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.Delete(dto);
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
		[HttpPost("Extornar")]
		public ActionResult<ServiceResponse> Extornar([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.Extornar(dto);
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
		public ActionResult<ServiceResponse> Cerrar([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				anticipoClienteBusiness.Cerrar(dto);
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
		public ActionResult<ServiceResponse> Emitir([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				anticipoClienteBusiness.Emitir(dto);
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
		[HttpPost("Aplicar")]
		public ActionResult<ServiceResponse> Aplicar([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.Aplicar(dto);
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
		[HttpPost("AnularAplicacion")]
		public ActionResult<ServiceResponse> AnularAplicacion([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.AnularAplicacion(dto);
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
		[HttpPost("AnularAsignacionPedido")]
		public ActionResult<ServiceResponse> AnularAsignacionPedido([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.AnularAsignacionPedido(dto);
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

		[HttpPost("DividirMonto")]
		public ActionResult<ServiceResponse> DividirMonto([FromBody] AnticipoClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				anticipoClienteBusiness.DividirMonto(dto);
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
