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
	[Route("api/EstadoOrdenProduccionSubProceso")]
	public class EstadoOrdenProduccionSubProcesoController : AuthorizedController
	{
		private readonly IEstadoOrdenProduccionSubProcesoBusiness estadoOrdenProduccionSubProcesoBusiness;

		public EstadoOrdenProduccionSubProcesoController(
			IEstadoOrdenProduccionSubProcesoBusiness estadoOrdenProduccionSubProcesoBusiness
			)
		{
			this.estadoOrdenProduccionSubProcesoBusiness = estadoOrdenProduccionSubProcesoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = estadoOrdenProduccionSubProcesoBusiness.GetById(dto.Id);
				serviceResponse.Data = estadoOrdenProduccionSubProcesoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<EstadoOrdenProduccionSubProcesoDTO> estadoOrdenProduccionSubProcesoDTOList = estadoOrdenProduccionSubProcesoBusiness.Search(dto);
				serviceResponse.Data = estadoOrdenProduccionSubProcesoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = estadoOrdenProduccionSubProcesoBusiness.SingleSearch(dto);
				serviceResponse.Data = estadoOrdenProduccionSubProcesoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = estadoOrdenProduccionSubProcesoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = estadoOrdenProduccionSubProcesoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				estadoOrdenProduccionSubProcesoBusiness.Delete(dto);
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
		public ActionResult<ServiceResponse> ObtenerPorCodigo([FromBody] EstadoOrdenProduccionSubProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				EstadoOrdenProduccionSubProcesoDTO estadoOrdenProduccionSubProcesoDTO = estadoOrdenProduccionSubProcesoBusiness.ObtenerPorCodigo(dto.Codigo);
				serviceResponse.Data = estadoOrdenProduccionSubProcesoDTO;
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
