using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RegimenSaludProxy : WebApiProxyClient, IRegimenSaludProxy
	{
		private const string API_CONTROLLER = "RegimenSalud";
		public RegimenSaludDTO GetById(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<RegimenSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RegimenSaludDTO> Search(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<List<RegimenSaludDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RegimenSaludDTO SingleSearch(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<RegimenSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RegimenSaludDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public RegimenSaludDTO ObtenerPorCodigo(RegimenSaludDTO dto)
		{
			var result = RestClient.PostApi<RegimenSaludDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
