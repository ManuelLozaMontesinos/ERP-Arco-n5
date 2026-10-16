using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/AlmacenVolumenProducto")]
	public class AlmacenVolumenProductoController : AuthorizedController
	{
		private readonly IAlmacenVolumenProductoBusiness almacenVolumenProductoBusiness;

		public AlmacenVolumenProductoController(
			IAlmacenVolumenProductoBusiness almacenVolumenProductoBusiness
			)
		{
			this.almacenVolumenProductoBusiness = almacenVolumenProductoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AlmacenVolumenProductoDTO almacenVolumenProductoDTO = almacenVolumenProductoBusiness.GetById(dto.Id);
				serviceResponse.Data = almacenVolumenProductoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = almacenVolumenProductoBusiness.Search(dto);
				serviceResponse.Data = almacenVolumenProductoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				AlmacenVolumenProductoDTO almacenVolumenProductoDTO = almacenVolumenProductoBusiness.SingleSearch(dto);
				serviceResponse.Data = almacenVolumenProductoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = almacenVolumenProductoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = almacenVolumenProductoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				almacenVolumenProductoBusiness.Delete(dto);
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
		[HttpPost("ListarPorSedeId")]
		public ActionResult<ServiceResponse> ListarPorSedeId([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = almacenVolumenProductoBusiness.ListarPorSedeId(dto);
				serviceResponse.Data = almacenVolumenProductoDTOList;
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
		[HttpPost("ActualizarLote")]
		public ActionResult<ServiceResponse> ActualizarLote([FromBody] List<AlmacenVolumenProductoDTO> list)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				almacenVolumenProductoBusiness.ActualizarLote(list);
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
		[HttpPost("ListarStockMinimoGrid")]
		public ActionResult<ServiceResponse> ListarStockMinimoGrid([FromBody] AlmacenVolumenProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = almacenVolumenProductoBusiness.ListarStockMinimo(dto);

				StringBuilder sb = new StringBuilder();

				sb.Append("{\"total\":1,");
				sb.Append("\"page\":1,");
				sb.Append("\"records\":" + dynamicResult.Count.ToString() + ",");
				sb.Append("\"rows\":[");

				int index = 1;
				foreach (var item in dynamicResult)
				{
					sb.Append("{\"id\":" + index.ToString() + ",");
					sb.Append("\"cell\":");
					sb.Append(JsonSerializer.Serialize<dynamic>(item, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }));
					sb.Append("}");

					if (index != dynamicResult.Count)
						sb.Append(",");
					index++;
				}
				sb.Append("]}");

				serviceResponse.Data = sb.ToString();
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
