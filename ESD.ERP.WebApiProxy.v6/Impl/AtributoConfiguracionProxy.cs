using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AtributoConfiguracionProxy : WebApiProxyClient, IAtributoConfiguracionProxy
	{
		private const string API_CONTROLLER = "AtributoConfiguracion";
		public AtributoConfiguracionDTO GetById(AtributoConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<AtributoConfiguracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AtributoConfiguracionDTO> Search(AtributoConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<List<AtributoConfiguracionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AtributoConfiguracionDTO SingleSearch(AtributoConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<AtributoConfiguracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AtributoConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AtributoConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AtributoConfiguracionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
