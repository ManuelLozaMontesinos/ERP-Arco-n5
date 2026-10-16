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
	[Route("api/PeriodoEmpresaSede")]
	public class PeriodoEmpresaSedeController : AuthorizedController
	{
		private readonly IPeriodoEmpresaSedeBusiness periodoEmpresaSedeBusiness;

		public PeriodoEmpresaSedeController(
			IPeriodoEmpresaSedeBusiness periodoEmpresaSedeBusiness
			)
		{
			this.periodoEmpresaSedeBusiness = periodoEmpresaSedeBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeBusiness.GetById(dto.Id);
				serviceResponse.Data = periodoEmpresaSedeDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeBusiness.Search(dto);
				serviceResponse.Data = periodoEmpresaSedeDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeBusiness.SingleSearch(dto);
				serviceResponse.Data = periodoEmpresaSedeDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = periodoEmpresaSedeBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = periodoEmpresaSedeBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				periodoEmpresaSedeBusiness.Delete(dto);
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
		[HttpPost("ObtenerEmpresaList")]
		public ActionResult<ServiceResponse> ObtenerEmpresaList([FromBody] PeriodoEmpresaSedeDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<EmpresaDTO> empresaListDTO = periodoEmpresaSedeBusiness.ObtenerEmpresaList(dto);
				serviceResponse.Data = empresaListDTO;
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
