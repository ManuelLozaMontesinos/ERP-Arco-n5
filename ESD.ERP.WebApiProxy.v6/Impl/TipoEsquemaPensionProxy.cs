using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoEsquemaPensionProxy : WebApiProxyClient, ITipoEsquemaPensionProxy
	{
		private const string API_CONTROLLER = "TipoEsquemaPension";
		public TipoEsquemaPensionDTO GetById(TipoEsquemaPensionDTO dto)
		{
			var result = RestClient.PostApi<TipoEsquemaPensionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoEsquemaPensionDTO> Search(TipoEsquemaPensionDTO dto)
		{
			var result = RestClient.PostApi<List<TipoEsquemaPensionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoEsquemaPensionDTO SingleSearch(TipoEsquemaPensionDTO dto)
		{
			var result = RestClient.PostApi<TipoEsquemaPensionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoEsquemaPensionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoEsquemaPensionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoEsquemaPensionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
