using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OcupacionProxy : WebApiProxyClient, IOcupacionProxy
	{
		private const string API_CONTROLLER = "Ocupacion";
		public OcupacionDTO GetById(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<OcupacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OcupacionDTO> Search(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<List<OcupacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OcupacionDTO SingleSearch(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<OcupacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OcupacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public OcupacionDTO ObtenerPorCodigo(OcupacionDTO dto)
		{
			var result = RestClient.PostApi<OcupacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
