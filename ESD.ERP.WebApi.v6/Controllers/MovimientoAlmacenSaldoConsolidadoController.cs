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
	[Route("api/MovimientoAlmacenSaldoConsolidado")]
	public class MovimientoAlmacenSaldoConsolidadoController : AuthorizedController
	{
		private readonly IMovimientoAlmacenSaldoConsolidadoBusiness movimientoAlmacenSaldoConsolidadoBusiness;

		public MovimientoAlmacenSaldoConsolidadoController(
			IMovimientoAlmacenSaldoConsolidadoBusiness movimientoAlmacenSaldoConsolidadoBusiness
			)
		{
			this.movimientoAlmacenSaldoConsolidadoBusiness = movimientoAlmacenSaldoConsolidadoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenSaldoConsolidadoDTO movimientoAlmacenSaldoConsolidadoDTO = movimientoAlmacenSaldoConsolidadoBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoAlmacenSaldoConsolidadoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoAlmacenSaldoConsolidadoDTO> movimientoAlmacenSaldoConsolidadoDTOList = movimientoAlmacenSaldoConsolidadoBusiness.Search(dto);
				serviceResponse.Data = movimientoAlmacenSaldoConsolidadoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenSaldoConsolidadoDTO movimientoAlmacenSaldoConsolidadoDTO = movimientoAlmacenSaldoConsolidadoBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoAlmacenSaldoConsolidadoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoAlmacenSaldoConsolidadoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoAlmacenSaldoConsolidadoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoConsolidadoBusiness.Delete(dto);
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
		[HttpPost("Valorizar")]
		public ActionResult<ServiceResponse> Valorizar([FromBody] MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoConsolidadoBusiness.Valorizar(dto);
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
