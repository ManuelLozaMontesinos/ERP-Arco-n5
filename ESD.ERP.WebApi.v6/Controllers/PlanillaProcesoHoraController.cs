using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/PlanillaProcesoHora")]
	public class PlanillaProcesoHoraController : AuthorizedController
	{
		private readonly IPlanillaProcesoHoraBusiness planillaProcesoHoraBusiness;
		public PlanillaProcesoHoraController(
			IPlanillaProcesoHoraBusiness planillaProcesoHoraBusiness
			)
		{
			this.planillaProcesoHoraBusiness = planillaProcesoHoraBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PlanillaProcesoHoraDTO planillaProcesoHoraDTO = planillaProcesoHoraBusiness.GetById(dto.Id);
				serviceResponse.Data = planillaProcesoHoraDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<PlanillaProcesoHoraDTO> planillaProcesoHoraDTOList = planillaProcesoHoraBusiness.Search(dto);
				serviceResponse.Data = planillaProcesoHoraDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PlanillaProcesoHoraDTO planillaProcesoHoraDTO = planillaProcesoHoraBusiness.SingleSearch(dto);
				serviceResponse.Data = planillaProcesoHoraDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = planillaProcesoHoraBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = planillaProcesoHoraBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoHoraBusiness.Delete(dto);
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
		[HttpPost("ListarHoraGrid")]
		public ActionResult<ServiceResponse> ListarHoraGrid([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = planillaProcesoHoraBusiness.ListarHoraPivot(dto);

				StringBuilder sb = new StringBuilder();

				sb.Append("{\"total\":1,");
				sb.Append("\"page\":1,");
				sb.Append("\"records\":" + dynamicResult.Count.ToString() + ",");
				sb.Append("\"rows\":[");

				int index = 1;
				foreach (var item in dynamicResult)
				{
					sb.Append("{\"id\":" + index.ToString() + ",");
					sb.Append("\"cell\":");
					sb.Append(JsonSerializer.Serialize<dynamic>(item, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }));
					sb.Append("}");

					if (index != dynamicResult.Count)
						sb.Append(",");
					index++;
				}
				sb.Append("]}");

				serviceResponse.Data = sb.ToString();
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
		[HttpPost("ActualizarLote")]
		public ActionResult<ServiceResponse> ActualizarLote([FromBody] List<PlanillaProcesoHoraDTO> list) {
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoHoraBusiness.ActualizarLote(list);
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
		[HttpPost("ActualizarLotePorPersonal")]
		public ActionResult<ServiceResponse> ActualizarLotePorPersonal([FromBody] List<PlanillaProcesoHoraDTO> list)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoHoraBusiness.ActualizarLotePorPersonal(list);
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
		[HttpPost("ActualizarListaPersonalId")]
		public ActionResult<ServiceResponse> ActualizarListaPersonalId([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoHoraBusiness.ActualizarListaPersonalId(dto);
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
		[HttpPost("ActualizarDesdeMarcacion")]
		public ActionResult<ServiceResponse> ActualizarDesdeMarcacion([FromBody] PlanillaProcesoHoraDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				planillaProcesoHoraBusiness.ActualizarDesdeMarcacion(dto);
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
