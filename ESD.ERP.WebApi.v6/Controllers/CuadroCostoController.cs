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
	[Route("api/CuadroCosto")]
	public class CuadroCostoController : AuthorizedController
	{
		private readonly ICuadroCostoBusiness cuadroCostoBusiness;

		public CuadroCostoController(
			ICuadroCostoBusiness cuadroCostoBusiness
			)
		{
			this.cuadroCostoBusiness = cuadroCostoBusiness;
		}
		[HttpPost("Search")]
		public ActionResult<ServiceResponse> Search([FromBody] CuadroCostoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<CuadroCostoDTO> cuadroCostoDTOList = cuadroCostoBusiness.Search(dto);
				serviceResponse.Data = cuadroCostoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] CuadroCostoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				CuadroCostoDTO cuadroCostoDTO = cuadroCostoBusiness.SingleSearch(dto);
				serviceResponse.Data = cuadroCostoDTO;
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
