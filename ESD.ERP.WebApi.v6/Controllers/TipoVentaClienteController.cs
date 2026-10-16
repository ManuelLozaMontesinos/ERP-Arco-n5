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
	[Route("api/TipoVentaCliente")]
	public class TipoVentaClienteController : AuthorizedController
	{
		private readonly ITipoVentaClienteBusiness tipoVentaClienteBusiness;

		public TipoVentaClienteController(
			ITipoVentaClienteBusiness tipoVentaClienteBusiness
			)
		{
			this.tipoVentaClienteBusiness = tipoVentaClienteBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				TipoVentaClienteDTO tipoVentaClienteDTO = tipoVentaClienteBusiness.GetById(dto.Id);
				serviceResponse.Data = tipoVentaClienteDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<TipoVentaClienteDTO> tipoVentaClienteDTOList = tipoVentaClienteBusiness.Search(dto);
				serviceResponse.Data = tipoVentaClienteDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				TipoVentaClienteDTO tipoVentaClienteDTO = tipoVentaClienteBusiness.SingleSearch(dto);
				serviceResponse.Data = tipoVentaClienteDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = tipoVentaClienteBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = tipoVentaClienteBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] TipoVentaClienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				tipoVentaClienteBusiness.Delete(dto);
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
