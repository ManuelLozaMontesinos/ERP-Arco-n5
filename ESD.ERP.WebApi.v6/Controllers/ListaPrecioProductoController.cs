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
	[Route("api/ListaPrecioProducto")]
	public class ListaPrecioProductoController : AuthorizedController
	{
		private readonly IListaPrecioProductoBusiness listaPrecioProductoBusiness;

		public ListaPrecioProductoController(
			IListaPrecioProductoBusiness listaPrecioProductoBusiness
			)
		{
			this.listaPrecioProductoBusiness = listaPrecioProductoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ListaPrecioProductoDTO listaPrecioProductoDTO = listaPrecioProductoBusiness.GetById(dto.Id);
				serviceResponse.Data = listaPrecioProductoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<ListaPrecioProductoDTO> listaPrecioProductoDTOList = listaPrecioProductoBusiness.Search(dto);
				serviceResponse.Data = listaPrecioProductoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				ListaPrecioProductoDTO listaPrecioProductoDTO = listaPrecioProductoBusiness.SingleSearch(dto);
				serviceResponse.Data = listaPrecioProductoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = listaPrecioProductoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = listaPrecioProductoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				listaPrecioProductoBusiness.Delete(dto);
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
		public ActionResult<ServiceResponse> ActualizarLote([FromBody] List<ListaPrecioProductoDTO> list)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				listaPrecioProductoBusiness.ActualizarLote(list);
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
		[HttpPost("ListarSaldoAlmacenGrid")]
		public ActionResult<ServiceResponse> ListarSaldoAlmacenGrid([FromBody] ListaPrecioProductoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = listaPrecioProductoBusiness.ListarSaldoAlmacen(dto);

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
