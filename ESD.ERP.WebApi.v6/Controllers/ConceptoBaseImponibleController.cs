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
	[Route("api/ConceptoBaseImponible")]
	public class ConceptoBaseImponibleController : AuthorizedController
	{
		private readonly IConceptoBaseImponibleBusiness conceptoBaseImponibleBusiness;

		public ConceptoBaseImponibleController(
			IConceptoBaseImponibleBusiness conceptoBaseImponibleBusiness
			)
		{
			this.conceptoBaseImponibleBusiness = conceptoBaseImponibleBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ConceptoBaseImponibleDTO conceptoBaseImponibleDTO = conceptoBaseImponibleBusiness.GetById(dto.Id);
				serviceResponse.Data = conceptoBaseImponibleDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<ConceptoBaseImponibleDTO> conceptoBaseImponibleDTOList = conceptoBaseImponibleBusiness.Search(dto);
				serviceResponse.Data = conceptoBaseImponibleDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ConceptoBaseImponibleDTO conceptoBaseImponibleDTO = conceptoBaseImponibleBusiness.SingleSearch(dto);
				serviceResponse.Data = conceptoBaseImponibleDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = conceptoBaseImponibleBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = conceptoBaseImponibleBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				conceptoBaseImponibleBusiness.Delete(dto);
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
		[HttpPost("ListarPorConceptoRemunerativoId")]
		public ActionResult<ServiceResponse> ListarPorConceptoRemunerativoId([FromBody] ConceptoBaseImponibleDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<ConceptoBaseImponibleDTO> conceptoBaseImponibleDTOList = conceptoBaseImponibleBusiness
					.ListarPorConceptoRemunerativoId(dto);
				serviceResponse.Data = conceptoBaseImponibleDTOList;
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
