using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CuadroCostoProxy : WebApiProxyClient, ICuadroCostoProxy
	{
		private const string API_CONTROLLER = "CuadroCosto";
		public List<CuadroCostoDTO> Search(CuadroCostoDTO dto)
		{
			var result = RestClient.PostApi<List<CuadroCostoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CuadroCostoDTO SingleSearch(CuadroCostoDTO dto)
		{
			var result = RestClient.PostApi<CuadroCostoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
	}
}
