using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoBaseImponibleProxy : WebApiProxyClient, ITipoBaseImponibleProxy
	{
		private const string API_CONTROLLER = "TipoBaseImponible";
		public TipoBaseImponibleDTO GetById(TipoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<TipoBaseImponibleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoBaseImponibleDTO> Search(TipoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<List<TipoBaseImponibleDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoBaseImponibleDTO SingleSearch(TipoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<TipoBaseImponibleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoBaseImponibleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoBaseImponibleDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
