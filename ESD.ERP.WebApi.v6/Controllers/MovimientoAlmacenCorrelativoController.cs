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
	[Route("api/MovimientoAlmacenCorrelativo")]
	public class MovimientoAlmacenCorrelativoController : AuthorizedController
	{
		private readonly IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness;

		public MovimientoAlmacenCorrelativoController(
			IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness
			)
		{
			this.movimientoAlmacenCorrelativoBusiness = movimientoAlmacenCorrelativoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = movimientoAlmacenCorrelativoBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoAlmacenCorrelativoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoAlmacenCorrelativoDTO> movimientoAlmacenCorrelativoDTOList = movimientoAlmacenCorrelativoBusiness.Search(dto);
				serviceResponse.Data = movimientoAlmacenCorrelativoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = movimientoAlmacenCorrelativoBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoAlmacenCorrelativoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoAlmacenCorrelativoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoAlmacenCorrelativoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoAlmacenCorrelativoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenCorrelativoBusiness.Delete(dto);
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
