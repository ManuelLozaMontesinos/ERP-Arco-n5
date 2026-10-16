using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RelojMarcacionProxy : WebApiProxyClient, IRelojMarcacionProxy
	{
		private const string API_CONTROLLER = "RelojMarcacion";
		public RelojMarcacionDTO GetById(RelojMarcacionDTO dto)
		{
			var result = RestClient.PostApi<RelojMarcacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RelojMarcacionDTO> Search(RelojMarcacionDTO dto)
		{
			var result = RestClient.PostApi<List<RelojMarcacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RelojMarcacionDTO SingleSearch(RelojMarcacionDTO dto)
		{
			var result = RestClient.PostApi<RelojMarcacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RelojMarcacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RelojMarcacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RelojMarcacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
