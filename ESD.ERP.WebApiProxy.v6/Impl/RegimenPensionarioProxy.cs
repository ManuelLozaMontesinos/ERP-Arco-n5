using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RegimenPensionarioProxy : WebApiProxyClient, IRegimenPensionarioProxy
	{
		private const string API_CONTROLLER = "RegimenPensionario";
		public RegimenPensionarioDTO GetById(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<RegimenPensionarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RegimenPensionarioDTO> Search(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<List<RegimenPensionarioDTO>>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RegimenPensionarioDTO SingleSearch(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<RegimenPensionarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RegimenPensionarioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.DELETE), dto);
		}
		public RegimenPensionarioDTO ObtenerPorCodigo(RegimenPensionarioDTO dto)
		{
			var result = RestClient.PostApi<RegimenPensionarioDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
