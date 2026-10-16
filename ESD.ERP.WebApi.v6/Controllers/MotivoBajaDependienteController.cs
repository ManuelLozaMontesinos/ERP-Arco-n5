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
	[Route("api/MotivoBajaDependiente")]
	public class MotivoBajaDependienteController : AuthorizedController
	{
		private readonly IMotivoBajaDependienteBusiness motivoBajaDependienteBusiness;

		public MotivoBajaDependienteController(
			IMotivoBajaDependienteBusiness motivoBajaDependienteBusiness
			)
		{
			this.motivoBajaDependienteBusiness = motivoBajaDependienteBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MotivoBajaDependienteDTO motivoBajaDependienteDTO = motivoBajaDependienteBusiness.GetById(dto.Id);
				serviceResponse.Data = motivoBajaDependienteDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MotivoBajaDependienteDTO> motivoBajaDependienteDTOList = motivoBajaDependienteBusiness.Search(dto);
				serviceResponse.Data = motivoBajaDependienteDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MotivoBajaDependienteDTO motivoBajaDependienteDTO = motivoBajaDependienteBusiness.SingleSearch(dto);
				serviceResponse.Data = motivoBajaDependienteDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = motivoBajaDependienteBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = motivoBajaDependienteBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MotivoBajaDependienteDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				motivoBajaDependienteBusiness.Delete(dto);
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
