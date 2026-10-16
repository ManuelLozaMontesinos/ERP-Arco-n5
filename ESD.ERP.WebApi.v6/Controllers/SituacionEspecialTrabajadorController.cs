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
	[Route("api/SituacionEspecialTrabajador")]
	public class SituacionEspecialTrabajadorController : AuthorizedController
	{
		private readonly ISituacionEspecialTrabajadorBusiness situacionEspecialTrabajadorBusiness;

		public SituacionEspecialTrabajadorController(
			ISituacionEspecialTrabajadorBusiness situacionEspecialTrabajadorBusiness
			)
		{
			this.situacionEspecialTrabajadorBusiness = situacionEspecialTrabajadorBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = situacionEspecialTrabajadorBusiness.GetById(dto.Id);
				serviceResponse.Data = situacionEspecialTrabajadorDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<SituacionEspecialTrabajadorDTO> situacionEspecialTrabajadorDTOList = situacionEspecialTrabajadorBusiness.Search(dto);
				serviceResponse.Data = situacionEspecialTrabajadorDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = situacionEspecialTrabajadorBusiness.SingleSearch(dto);
				serviceResponse.Data = situacionEspecialTrabajadorDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = situacionEspecialTrabajadorBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = situacionEspecialTrabajadorBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				situacionEspecialTrabajadorBusiness.Delete(dto);
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
		public ActionResult<ServiceResponse> ObtenerPorCodigo([FromBody] SituacionEspecialTrabajadorDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				SituacionEspecialTrabajadorDTO situacionEspecialTrabajadorDTO = situacionEspecialTrabajadorBusiness.ObtenerPorCodigo(dto.Codigo);
				serviceResponse.Data = situacionEspecialTrabajadorDTO;
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
