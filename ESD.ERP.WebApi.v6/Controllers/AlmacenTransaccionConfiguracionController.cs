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
	[Route("api/AlmacenTransaccionConfiguracion")]
	public class AlmacenTransaccionConfiguracionController : AuthorizedController
	{
		private readonly IAlmacenTransaccionConfiguracionBusiness almacenTransaccionConfiguracionBusiness;

		public AlmacenTransaccionConfiguracionController(
			IAlmacenTransaccionConfiguracionBusiness almacenTransaccionConfiguracionBusiness
			)
		{
			this.almacenTransaccionConfiguracionBusiness = almacenTransaccionConfiguracionBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AlmacenTransaccionConfiguracionDTO almacenTransaccionConfiguracionDTO = almacenTransaccionConfiguracionBusiness.GetById(dto.Id);
				serviceResponse.Data = almacenTransaccionConfiguracionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfiguracionDTOList = almacenTransaccionConfiguracionBusiness.Search(dto);
				serviceResponse.Data = almacenTransaccionConfiguracionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AlmacenTransaccionConfiguracionDTO almacenTransaccionConfiguracionDTO = almacenTransaccionConfiguracionBusiness.SingleSearch(dto);
				serviceResponse.Data = almacenTransaccionConfiguracionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = almacenTransaccionConfiguracionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = almacenTransaccionConfiguracionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				almacenTransaccionConfiguracionBusiness.Delete(dto);
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
		[HttpPost("ListarPorPorAlmacenTransaccionId")]
		public ActionResult<ServiceResponse> ListarPorPorAlmacenTransaccionId([FromBody] AlmacenTransaccionConfiguracionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfiguracionDTOList = almacenTransaccionConfiguracionBusiness.ListarPorPorAlmacenTransaccionId(dto);
				serviceResponse.Data = almacenTransaccionConfiguracionDTOList;
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
