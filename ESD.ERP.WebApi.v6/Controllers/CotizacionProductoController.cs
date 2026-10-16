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
	[Route("api/CotizacionProducto")]
	public class CotizacionProductoController : AuthorizedController
	{
		private readonly ICotizacionProductoBusiness cotizacionProductoBusiness;

		public CotizacionProductoController(
			ICotizacionProductoBusiness cotizacionProductoBusiness
			)
		{
			this.cotizacionProductoBusiness = cotizacionProductoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				CotizacionProductoDTO cotizacionProductoDTO = cotizacionProductoBusiness.GetById(dto.Id);
				serviceResponse.Data = cotizacionProductoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoBusiness.Search(dto);
				serviceResponse.Data = cotizacionProductoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				CotizacionProductoDTO cotizacionProductoDTO = cotizacionProductoBusiness.SingleSearch(dto);
				serviceResponse.Data = cotizacionProductoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = cotizacionProductoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = cotizacionProductoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				 cotizacionProductoBusiness.Delete(dto);
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
		[HttpPost("ActualizarSinSaldo")]
		public ActionResult<ServiceResponse> ActualizarSinSaldo([FromBody] CotizacionProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				cotizacionProductoBusiness.ActualizarSinSaldo(dto);
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
        [HttpPost("CortesGratis")]
        public ActionResult<ServiceResponse> CortesGratis([FromBody] CotizacionProductoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoBusiness.CortesGratis(dto);
                serviceResponse.Data = cotizacionProductoDTOList;
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

        [HttpPost("CortesGratisIlo")]
        public ActionResult<ServiceResponse> CortesGratisIlo([FromBody] CotizacionProductoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoBusiness.CortesGratisIlo(dto);
                serviceResponse.Data = cotizacionProductoDTOList;
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

        [HttpPost("ProductosParcial")]
        public ActionResult<ServiceResponse> ProductosParcial([FromBody] CotizacionProductoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoBusiness.ProductosParcial(dto);
                serviceResponse.Data = cotizacionProductoDTOList;
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
