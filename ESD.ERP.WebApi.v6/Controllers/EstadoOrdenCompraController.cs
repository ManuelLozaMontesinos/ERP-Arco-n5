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
	[Route("api/EstadoOrdenCompra")]
	public class EstadoOrdenCompraController : AuthorizedController
	{
		private readonly IEstadoOrdenCompraBusiness estadoOrdenCompraBusiness;

		public EstadoOrdenCompraController(
			IEstadoOrdenCompraBusiness estadoOrdenCompraBusiness
			)
		{
			this.estadoOrdenCompraBusiness = estadoOrdenCompraBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenCompraDTO estadoOrdenCompraDTO = estadoOrdenCompraBusiness.GetById(dto.Id);
				serviceResponse.Data = estadoOrdenCompraDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<EstadoOrdenCompraDTO> estadoOrdenCompraDTOList = estadoOrdenCompraBusiness.Search(dto);
				serviceResponse.Data = estadoOrdenCompraDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenCompraDTO estadoOrdenCompraDTO = estadoOrdenCompraBusiness.SingleSearch(dto);
				serviceResponse.Data = estadoOrdenCompraDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = estadoOrdenCompraBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = estadoOrdenCompraBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				estadoOrdenCompraBusiness.Delete(dto);
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
		[HttpPost("ObtenerPorCodigo")]
		public ActionResult<ServiceResponse> ObtenerPorCodigo([FromBody] EstadoOrdenCompraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenCompraDTO estadoOrdenCompraDTO = estadoOrdenCompraBusiness.ObtenerPorCodigo(dto.Codigo);
				serviceResponse.Data = estadoOrdenCompraDTO;
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
