using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoCondicionPagoProxy : WebApiProxyClient, ITipoCondicionPagoProxy
	{
		private const string API_CONTROLLER = "TipoCondicionPago";
		public TipoCondicionPagoDTO GetById(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<TipoCondicionPagoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoCondicionPagoDTO> Search(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoCondicionPagoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoCondicionPagoDTO SingleSearch(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<TipoCondicionPagoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoCondicionPagoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoCondicionPagoDTO ObtenerPorCodigo(TipoCondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<TipoCondicionPagoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
