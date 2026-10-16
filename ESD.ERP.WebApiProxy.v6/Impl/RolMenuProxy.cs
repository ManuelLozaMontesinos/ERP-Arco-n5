using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RolMenuProxy : WebApiProxyClient, IRolMenuProxy
	{
		private const string API_CONTROLLER = "RolMenu";
		public RolMenuDTO GetById(RolMenuDTO dto)
		{
			var result = RestClient.PostApi<RolMenuDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RolMenuDTO> Search(RolMenuDTO dto)
		{
			var result = RestClient.PostApi<List<RolMenuDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RolMenuDTO SingleSearch(RolMenuDTO dto)
		{
			var result = RestClient.PostApi<RolMenuDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RolMenuDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RolMenuDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RolMenuDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public List<RolMenuDTO> ListarPorRolId(RolMenuDTO dto)
      {
			var result = RestClient.PostApi<List<RolMenuDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorRolId"), dto);
			return result;
		}
   }
}
