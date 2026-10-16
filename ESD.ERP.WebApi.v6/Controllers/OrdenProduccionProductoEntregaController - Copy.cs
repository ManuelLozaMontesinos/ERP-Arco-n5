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
	[Route("api/OrdenProduccionProductoEntrega")]
	public class OrdenProduccionProductoEntregaController : AuthorizedController
	{
		private readonly IOrdenProduccionProductoEntregaBusiness ordenProduccionProductoEntregaBusiness;

		public OrdenProduccionProductoEntregaController(
			IOrdenProduccionProductoEntregaBusiness ordenProduccionProductoEntregaBusiness
			)
		{
			this.ordenProduccionProductoEntregaBusiness = ordenProduccionProductoEntregaBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				OrdenProduccionProductoEntregaDTO ordenProduccionProductoEntregaDTO = ordenProduccionProductoEntregaBusiness.GetById(dto.Id);
				serviceResponse.Data = ordenProduccionProductoEntregaDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<OrdenProduccionProductoEntregaDTO> ordenProduccionProductoEntregaDTOList = ordenProduccionProductoEntregaBusiness.Search(dto);
				serviceResponse.Data = ordenProduccionProductoEntregaDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				OrdenProduccionProductoEntregaDTO ordenProduccionProductoEntregaDTO = ordenProduccionProductoEntregaBusiness.SingleSearch(dto);
				serviceResponse.Data = ordenProduccionProductoEntregaDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = ordenProduccionProductoEntregaBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = ordenProduccionProductoEntregaBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] OrdenProduccionProductoEntregaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ordenProduccionProductoEntregaBusiness.Delete(dto);
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
