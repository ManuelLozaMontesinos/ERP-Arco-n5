using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDescuentoProxy : WebApiProxyClient, ITipoDescuentoProxy
	{
		private const string API_CONTROLLER = "TipoDescuento";
		public TipoDescuentoDTO GetById(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<TipoDescuentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDescuentoDTO> Search(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDescuentoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDescuentoDTO SingleSearch(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<TipoDescuentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDescuentoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoDescuentoDTO ObtenerPorCodigo(TipoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<TipoDescuentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
