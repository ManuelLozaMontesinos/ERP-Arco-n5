using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoCuentaBancoProxy : WebApiProxyClient, ITipoCuentaBancoProxy
	{
		private const string API_CONTROLLER = "TipoCuentaBanco";
		public TipoCuentaBancoDTO GetById(TipoCuentaBancoDTO dto)
		{
			var result = RestClient.PostApi<TipoCuentaBancoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoCuentaBancoDTO> Search(TipoCuentaBancoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoCuentaBancoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoCuentaBancoDTO SingleSearch(TipoCuentaBancoDTO dto)
		{
			var result = RestClient.PostApi<TipoCuentaBancoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoCuentaBancoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoCuentaBancoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoCuentaBancoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
