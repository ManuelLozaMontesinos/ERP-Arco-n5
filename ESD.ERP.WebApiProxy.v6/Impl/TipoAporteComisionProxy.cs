using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoAporteComisionProxy : WebApiProxyClient, ITipoAporteComisionProxy
	{
		private const string API_CONTROLLER = "TipoAporteComision";
		public TipoAporteComisionDTO GetById(TipoAporteComisionDTO dto)
		{
			var result = RestClient.PostApi<TipoAporteComisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoAporteComisionDTO> Search(TipoAporteComisionDTO dto)
		{
			var result = RestClient.PostApi<List<TipoAporteComisionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoAporteComisionDTO SingleSearch(TipoAporteComisionDTO dto)
		{
			var result = RestClient.PostApi<TipoAporteComisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoAporteComisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoAporteComisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoAporteComisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
