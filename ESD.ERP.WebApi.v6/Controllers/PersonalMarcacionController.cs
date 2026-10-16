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
using ESD.Core.Service.Converter;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/PersonalMarcacion")]
	public class PersonalMarcacionController : AuthorizedController
	{
		private readonly IPersonalMarcacionBusiness personalMarcacionBusiness;

		public PersonalMarcacionController(
			IPersonalMarcacionBusiness personalMarcacionBusiness
			)
		{
			this.personalMarcacionBusiness = personalMarcacionBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PersonalMarcacionDTO personalMarcacionDTO = personalMarcacionBusiness.GetById(dto.Id);
				serviceResponse.Data = personalMarcacionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<PersonalMarcacionDTO> personalMarcacionDTOList = personalMarcacionBusiness.Search(dto);
				serviceResponse.Data = personalMarcacionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PersonalMarcacionDTO personalMarcacionDTO = personalMarcacionBusiness.SingleSearch(dto);
				serviceResponse.Data = personalMarcacionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = personalMarcacionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = personalMarcacionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				personalMarcacionBusiness.Delete(dto);
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
		[HttpPost("ListarMarcacionHoraGrid")]
		public ActionResult<ServiceResponse> ListarMarcacionHoraGrid([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = personalMarcacionBusiness.ListarMarcacionHoraPivot(dto);

				JsonSerializerOptions jsonSerializerOptions = new();
				jsonSerializerOptions.Converters.Add(new ShortDateTimeConverter());
				jsonSerializerOptions.WriteIndented = true;
				jsonSerializerOptions.PropertyNameCaseInsensitive = true;

				StringBuilder sb = new();

				sb.Append("{\"total\":1,");
				sb.Append("\"page\":1,");
				sb.Append("\"records\":" + dynamicResult.Count.ToString() + ",");
				sb.Append("\"rows\":[");

				int index = 1;
				foreach (var item in dynamicResult)
				{
					sb.Append("{\"id\":" + index.ToString() + ",");
					sb.Append("\"cell\":");
					sb.Append(JsonSerializer.Serialize<dynamic>(item, jsonSerializerOptions));
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
		[HttpPost("SincronizarMarcacion")]
		public ActionResult<ServiceResponse> SincronizarMarcacion([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				personalMarcacionBusiness.SincronizarMarcacion(dto);
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
		[HttpPost("ListarPersonalConHoras")]
		public ActionResult<ServiceResponse> ListarPersonalConHoras([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<PersonalDTO> personalDTOList = personalMarcacionBusiness.ListarPersonalConHoras(dto);
				serviceResponse.Data = personalDTOList;
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
		[HttpPost("ActualizarHoraLote")]
		public ActionResult<ServiceResponse> ActualizarHoraLote([FromBody] PersonalMarcacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				personalMarcacionBusiness.ActualizarHoraLote(dto);
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
