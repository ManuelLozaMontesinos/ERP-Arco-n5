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
	[Route("api/MovimientoAlmacenCierre")]
	public class MovimientoAlmacenCierreController : AuthorizedController
	{
		private readonly IMovimientoAlmacenCierreBusiness movimientoAlmacenCierreBusiness;

		public MovimientoAlmacenCierreController(
			IMovimientoAlmacenCierreBusiness movimientoAlmacenCierreBusiness
			)
		{
			this.movimientoAlmacenCierreBusiness = movimientoAlmacenCierreBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenCierreDTO movimientoAlmacenCierreDTO = movimientoAlmacenCierreBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoAlmacenCierreDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoAlmacenCierreDTO> movimientoAlmacenCierreDTOList = movimientoAlmacenCierreBusiness.Search(dto);
				serviceResponse.Data = movimientoAlmacenCierreDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenCierreDTO movimientoAlmacenCierreDTO = movimientoAlmacenCierreBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoAlmacenCierreDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoAlmacenCierreBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoAlmacenCierreBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenCierreBusiness.Delete(dto);
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
		[HttpPost("Aperturar")]
		public ActionResult<ServiceResponse> Aperturar([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenCierreBusiness.Aperturar(dto);
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
		[HttpPost("Cerrar")]
		public ActionResult<ServiceResponse> Cerrar([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenCierreBusiness.Cerrar(dto);
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
		[HttpPost("Reprocesar")]
		public ActionResult<ServiceResponse> Reprocesar([FromBody] MovimientoAlmacenCierreDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenCierreBusiness.Reprocesar(dto);
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

        [HttpPost("ReprocesoAnual")]
        public ActionResult<ServiceResponse> ReprocesoAnual([FromBody] MovimientoAlmacenCierreDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                movimientoAlmacenCierreBusiness.ReprocesarSaldoAnual(dto);
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
