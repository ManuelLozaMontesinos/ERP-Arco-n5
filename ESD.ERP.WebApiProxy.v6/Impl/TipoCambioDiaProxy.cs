using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoCambioDiaProxy : WebApiProxyClient, ITipoCambioDiaProxy
	{
		private const string API_CONTROLLER = "TipoCambioDia";
		public TipoCambioDiaDTO GetById(TipoCambioDiaDTO dto)
		{
			var result = RestClient.PostApi<TipoCambioDiaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoCambioDiaDTO> Search(TipoCambioDiaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoCambioDiaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoCambioDiaDTO SingleSearch(TipoCambioDiaDTO dto)
		{
			var result = RestClient.PostApi<TipoCambioDiaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoCambioDiaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoCambioDiaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoCambioDiaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
