using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorRedSocialProxy : WebApiProxyClient, IProveedorRedSocialProxy
	{
		private const string API_CONTROLLER = "ProveedorRedSocial";
		public ProveedorRedSocialDTO GetById(ProveedorRedSocialDTO dto)
		{
			var result = RestClient.PostApi<ProveedorRedSocialDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorRedSocialDTO> Search(ProveedorRedSocialDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorRedSocialDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorRedSocialDTO SingleSearch(ProveedorRedSocialDTO dto)
		{
			var result = RestClient.PostApi<ProveedorRedSocialDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorRedSocialDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorRedSocialDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorRedSocialDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
