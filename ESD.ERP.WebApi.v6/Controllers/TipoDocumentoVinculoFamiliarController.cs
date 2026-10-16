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
	[Route("api/TipoDocumentoVinculoFamiliar")]
	public class TipoDocumentoVinculoFamiliarController : AuthorizedController
	{
		private readonly ITipoDocumentoVinculoFamiliarBusiness tipoDocumentoVinculoFamiliarBusiness;

		public TipoDocumentoVinculoFamiliarController(
			ITipoDocumentoVinculoFamiliarBusiness tipoDocumentoVinculoFamiliarBusiness
			)
		{
			this.tipoDocumentoVinculoFamiliarBusiness = tipoDocumentoVinculoFamiliarBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				TipoDocumentoVinculoFamiliarDTO tipoDocumentoVinculoFamiliarDTO = tipoDocumentoVinculoFamiliarBusiness.GetById(dto.Id);
				serviceResponse.Data = tipoDocumentoVinculoFamiliarDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<TipoDocumentoVinculoFamiliarDTO> tipoDocumentoVinculoFamiliarDTOList = tipoDocumentoVinculoFamiliarBusiness.Search(dto);
				serviceResponse.Data = tipoDocumentoVinculoFamiliarDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				TipoDocumentoVinculoFamiliarDTO tipoDocumentoVinculoFamiliarDTO = tipoDocumentoVinculoFamiliarBusiness.SingleSearch(dto);
				serviceResponse.Data = tipoDocumentoVinculoFamiliarDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = tipoDocumentoVinculoFamiliarBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = tipoDocumentoVinculoFamiliarBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] TipoDocumentoVinculoFamiliarDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				tipoDocumentoVinculoFamiliarBusiness.Delete(dto);
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
