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
	[Route("api/OrdenProduccion")]
	public class OrdenProduccionController : AuthorizedController
	{
		private readonly IOrdenProduccionBusiness ordenProduccionBusiness;

		public OrdenProduccionController(
			IOrdenProduccionBusiness ordenProduccionBusiness
			)
		{
			this.ordenProduccionBusiness = ordenProduccionBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				OrdenProduccionDTO ordenProduccionDTO = ordenProduccionBusiness.GetById(dto.Id);
				serviceResponse.Data = ordenProduccionDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<OrdenProduccionDTO> ordenProduccionDTOList = ordenProduccionBusiness.Search(dto);
				serviceResponse.Data = ordenProduccionDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				OrdenProduccionDTO ordenProduccionDTO = ordenProduccionBusiness.SingleSearch(dto);
				serviceResponse.Data = ordenProduccionDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = ordenProduccionBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = ordenProduccionBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ordenProduccionBusiness.Delete(dto);
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
		[HttpPost("CerrarDespacho")]
		public ActionResult<ServiceResponse> CerrarDespacho([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new(); ;

			try
			{
				ordenProduccionBusiness.CerrarDespacho(dto);
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

		[HttpPost("CerrarInsumo")]
		public ActionResult<ServiceResponse> CerrarInsumo([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ordenProduccionBusiness.CerrarInsumo(dto);
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
		public ActionResult<ServiceResponse> CambiarEstado([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ordenProduccionBusiness.CambiarEstado(dto);
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

		[HttpPost("AsignarMaquina")]
		public ActionResult<ServiceResponse> AsignarMaquina([FromBody] OrdenProduccionDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ordenProduccionBusiness.AsignarMaquina(dto);
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
