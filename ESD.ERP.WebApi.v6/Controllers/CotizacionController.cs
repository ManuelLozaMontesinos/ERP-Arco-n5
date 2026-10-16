using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using ESD.ERP.Business.Impl;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/Cotizacion")]
	public class CotizacionController : AuthorizedController
	{
		private readonly ICotizacionBusiness cotizacionBusiness;

		public CotizacionController(
			ICotizacionBusiness cotizacionBusiness
			)
		{
			this.cotizacionBusiness = cotizacionBusiness;
		}

		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				CotizacionDTO cotizacionDTO = cotizacionBusiness.GetById(dto.Id);
				serviceResponse.Data = cotizacionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<CotizacionDTO> cotizacionDTOList = cotizacionBusiness.Search(dto);
				serviceResponse.Data = cotizacionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				CotizacionDTO cotizacionDTO = cotizacionBusiness.SingleSearch(dto);
				serviceResponse.Data = cotizacionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = cotizacionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = cotizacionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				cotizacionBusiness.Delete(dto);
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

		[HttpPost("GenerarComprobante")]
		public ActionResult<ServiceResponse> GenerarComprobante([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = cotizacionBusiness.GenerarComprobante(dto);
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

		[HttpPost("GenerarPedido")]
		public ActionResult<ServiceResponse> GenerarPedido([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				cotizacionBusiness.GenerarPedido(dto);

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

		[HttpPost("CambiarEstado")]
		public ActionResult<ServiceResponse> CambiarEstado([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				cotizacionBusiness.CambiarEstado(dto);
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

		[HttpPost("GenerarOrdenProduccion")]
		public ActionResult<ServiceResponse> GenerarOrdenProduccion([FromBody] CotizacionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				cotizacionBusiness.GenerarOrdenProduccion(dto);
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

        [HttpPost("GenerarOrdenProduccionParcial")]
        public ActionResult<ServiceResponse> GenerarOrdenProduccionParcial([FromBody] CotizacionDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                cotizacionBusiness.GenerarOrdenProduccionParcial(dto);
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

		[HttpPost("CortesGratisParcial")]
        public ActionResult<ServiceResponse> CortesGratisParcial([FromBody] CotizacionDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                decimal cortesGratis = cotizacionBusiness.CortesGratisParcial(dto);
                serviceResponse.Data = cortesGratis;
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
