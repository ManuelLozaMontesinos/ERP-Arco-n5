using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoImpuestoProxy : WebApiProxyClient, ITipoImpuestoProxy
	{
		private const string API_CONTROLLER = "TipoImpuesto";
		public TipoImpuestoDTO GetById(TipoImpuestoDTO dto)
		{
			var result = RestClient.PostApi<TipoImpuestoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoImpuestoDTO> Search(TipoImpuestoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoImpuestoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoImpuestoDTO SingleSearch(TipoImpuestoDTO dto)
		{
			var result = RestClient.PostApi<TipoImpuestoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoImpuestoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoImpuestoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoImpuestoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
