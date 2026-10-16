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
	[Route("api/MovimientoAlmacenSaldo")]
	public class MovimientoAlmacenSaldoController : AuthorizedController
	{
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;

		public MovimientoAlmacenSaldoController(
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness
			)
		{
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
		}
		[HttpPost("GetById")]
		public ActionResult<ServiceResponse> GetById([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = movimientoAlmacenSaldoBusiness.GetById(dto.Id);
				serviceResponse.Data = movimientoAlmacenSaldoDTO;
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
		public ActionResult<ServiceResponse> Search([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = movimientoAlmacenSaldoBusiness.Search(dto);
				serviceResponse.Data = movimientoAlmacenSaldoDTOList;
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
		public ActionResult<ServiceResponse> SingleSearch([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = movimientoAlmacenSaldoBusiness.SingleSearch(dto);
				serviceResponse.Data = movimientoAlmacenSaldoDTO;
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
		public ActionResult<ServiceResponse> Insert([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int newId = movimientoAlmacenSaldoBusiness.Insert(dto);
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
		public ActionResult<ServiceResponse> Update([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				int id = movimientoAlmacenSaldoBusiness.Update(dto);
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
		public ActionResult<ServiceResponse> Delete([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoBusiness.Delete(dto);
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
		[HttpPost("ObtenerSaldoVenta")]
		public ActionResult<ServiceResponse> ObtenerSaldoVenta([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoVenta(dto);
				serviceResponse.Data = saldo;
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
		[HttpPost("ValidarSaldoVenta")]
		public ActionResult<ServiceResponse> ValidarSaldoVenta([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(dto);
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
		[HttpPost("ValidarSaldoVentaCombo")]
		public ActionResult<ServiceResponse> ValidarSaldoVentaCombo([FromBody] List<MovimientoAlmacenSaldoDTO> list)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoBusiness.ValidarSaldoVentaCombo(list);
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
		[HttpPost("ActualizarCantidadReserva")]
		public ActionResult<ServiceResponse> ActualizarCantidadReserva([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{
				movimientoAlmacenSaldoBusiness.ActualizarCantidadReserva(dto);
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
		[HttpPost("ListarSaldoMultiAlmacenGrid")]
		public ActionResult<ServiceResponse> ListarSaldoMultiAlmacenGrid([FromBody] MovimientoAlmacenSaldoDTO dto)
		{
			Logger.InfoStart();
			ServiceResponse serviceResponse = new();

			try
			{

				List<dynamic> dynamicResult = movimientoAlmacenSaldoBusiness.ListarSaldoMultiAlmacen(dto);

				StringBuilder sb = new();

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
					sb.Append('}');

					if (index != dynamicResult.Count)
						sb.Append(',');
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
