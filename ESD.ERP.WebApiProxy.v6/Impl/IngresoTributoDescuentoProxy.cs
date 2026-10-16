using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class IngresoTributoDescuentoProxy : WebApiProxyClient, IIngresoTributoDescuentoProxy
	{
		private const string API_CONTROLLER = "IngresoTributoDescuento";
		public IngresoTributoDescuentoDTO GetById(IngresoTributoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<IngresoTributoDescuentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<IngresoTributoDescuentoDTO> Search(IngresoTributoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<List<IngresoTributoDescuentoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public IngresoTributoDescuentoDTO SingleSearch(IngresoTributoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<IngresoTributoDescuentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(IngresoTributoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(IngresoTributoDescuentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(IngresoTributoDescuentoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
