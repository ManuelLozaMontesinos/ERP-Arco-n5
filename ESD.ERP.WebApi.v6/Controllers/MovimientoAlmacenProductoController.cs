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
	[Route("api/MovimientoAlmacenProducto")]
	public class MovimientoAlmacenProductoController : AuthorizedController
	{
		private readonly IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness;

		public MovimientoAlmacenProductoController(
			IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness
			)
		{
			this.movimientoAlmacenProductoBusiness = movimientoAlmacenProductoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = movimientoAlmacenProductoBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoAlmacenProductoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoBusiness.Search(dto);
				serviceResponse.Data = movimientoAlmacenProductoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = movimientoAlmacenProductoBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoAlmacenProductoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoAlmacenProductoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoAlmacenProductoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenProductoBusiness.Delete(dto);
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
		[HttpPost("AgregarDesdeReserva")]
		public ActionResult<ServiceResponse> AgregarDesdeReserva([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenProductoBusiness.AgregarDesdeReserva(dto);
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
		[HttpPost("ActualizarProductoReclasificacion")]
		public ActionResult<ServiceResponse> ActualizarProductoReclasificacion([FromBody] MovimientoAlmacenProductoDTO dto)
      {
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenProductoBusiness.ActualizarProductoReclasificacion(dto);
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
		[HttpPost("AgregarOrdenCompraProducto")]
		public ActionResult<ServiceResponse> AgregarOrdenCompraProducto([FromBody] MovimientoAlmacenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenProductoBusiness.AgregarOrdenCompraProducto(dto);
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
