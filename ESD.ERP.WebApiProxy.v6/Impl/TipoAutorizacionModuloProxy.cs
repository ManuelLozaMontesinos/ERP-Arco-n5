using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoAutorizacionModuloProxy : WebApiProxyClient, ITipoAutorizacionModuloProxy
	{
		private const string API_CONTROLLER = "TipoAutorizacionModulo";
		public TipoAutorizacionModuloDTO GetById(TipoAutorizacionModuloDTO dto)
		{
			var result = RestClient.PostApi<TipoAutorizacionModuloDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoAutorizacionModuloDTO> Search(TipoAutorizacionModuloDTO dto)
		{
			var result = RestClient.PostApi<List<TipoAutorizacionModuloDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoAutorizacionModuloDTO SingleSearch(TipoAutorizacionModuloDTO dto)
		{
			var result = RestClient.PostApi<TipoAutorizacionModuloDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoAutorizacionModuloDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoAutorizacionModuloDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoAutorizacionModuloDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
