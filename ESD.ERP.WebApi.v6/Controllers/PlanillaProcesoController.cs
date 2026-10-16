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
	[Route("api/PlanillaProceso")]
	public class PlanillaProcesoController : AuthorizedController
	{
		private readonly IPlanillaProcesoBusiness planillaProcesoBusiness;

		public PlanillaProcesoController(
			IPlanillaProcesoBusiness planillaProcesoBusiness
			)
		{
			this.planillaProcesoBusiness = planillaProcesoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PlanillaProcesoDTO planillaProcesoDTO = planillaProcesoBusiness.GetById(dto.Id);
				serviceResponse.Data = planillaProcesoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<PlanillaProcesoDTO> planillaProcesoDTOList = planillaProcesoBusiness.Search(dto);
				serviceResponse.Data = planillaProcesoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PlanillaProcesoDTO planillaProcesoDTO = planillaProcesoBusiness.SingleSearch(dto);
				serviceResponse.Data = planillaProcesoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = planillaProcesoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = planillaProcesoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoBusiness.Delete(dto);
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
		[HttpPost("Calcular")]
		public ActionResult<ServiceResponse> Calcular([FromBody] PlanillaProcesoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoBusiness.Calcular(dto);
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
