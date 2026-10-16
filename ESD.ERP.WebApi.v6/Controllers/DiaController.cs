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
	[Route("api/Dia")]
	public class DiaController : AuthorizedController
	{
		private readonly IDiaBusiness diaBusiness;

		public DiaController(
			IDiaBusiness diaBusiness
			)
		{
			this.diaBusiness = diaBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				DiaDTO diaDTO = diaBusiness.GetById(dto.Id);
				serviceResponse.Data = diaDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<DiaDTO> diaDTOList = diaBusiness.Search(dto);
				serviceResponse.Data = diaDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				DiaDTO diaDTO = diaBusiness.SingleSearch(dto);
				serviceResponse.Data = diaDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = diaBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = diaBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] DiaDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				diaBusiness.Delete(dto);
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
