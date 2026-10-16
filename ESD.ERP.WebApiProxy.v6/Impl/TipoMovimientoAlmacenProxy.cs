using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoMovimientoAlmacenProxy : WebApiProxyClient, ITipoMovimientoAlmacenProxy
	{
		private const string API_CONTROLLER = "TipoMovimientoAlmacen";
		public TipoMovimientoAlmacenDTO GetById(TipoMovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<TipoMovimientoAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoMovimientoAlmacenDTO> Search(TipoMovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<List<TipoMovimientoAlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoMovimientoAlmacenDTO SingleSearch(TipoMovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<TipoMovimientoAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoMovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoMovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoMovimientoAlmacenDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public TipoMovimientoAlmacenDTO ObtenerPorCodigo(TipoMovimientoAlmacenDTO dto)
      {
			var result = RestClient.PostApi<TipoMovimientoAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
   }
}
