using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using System.ComponentModel;
using System.Reflection;
using ESD.ERP.Business.Impl;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/MovimientoReclasificacion")]
	public class MovimientoReclasificacionController : AuthorizedController
	{
		private readonly IMovimientoReclasificacionBusiness movimientoReclasificacionBusiness;

		public MovimientoReclasificacionController(
			IMovimientoReclasificacionBusiness movimientoReclasificacionBusiness
			)
		{
			this.movimientoReclasificacionBusiness = movimientoReclasificacionBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoReclasificacionDTO movimientoReclasificacionDTO = movimientoReclasificacionBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoReclasificacionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoReclasificacionDTO> movimientoReclasificacionDTOList = movimientoReclasificacionBusiness.Search(dto);
				serviceResponse.Data = movimientoReclasificacionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoReclasificacionDTO movimientoReclasificacionDTO = movimientoReclasificacionBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoReclasificacionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoReclasificacionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoReclasificacionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoReclasificacionBusiness.Delete(dto);
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

		[HttpPost("Solicitar")]
		
		public ActionResult<ServiceResponse> Solicitar([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();
			try
			{
				movimientoReclasificacionBusiness.Solicitar(dto);
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

		[HttpPost("Aprobar")]
		public ActionResult<ServiceResponse> Aprobar([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoReclasificacionBusiness.Aprobar(dto);
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

		[HttpPost("Desaprobar")]

		public ActionResult<ServiceResponse> Desaprobar([FromBody] MovimientoReclasificacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try 
			{
				movimientoReclasificacionBusiness.Desaprobar(dto);
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
