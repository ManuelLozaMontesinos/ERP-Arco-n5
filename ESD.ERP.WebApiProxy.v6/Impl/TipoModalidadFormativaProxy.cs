using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoModalidadFormativaProxy : WebApiProxyClient, ITipoModalidadFormativaProxy
	{
		private const string API_CONTROLLER = "TipoModalidadFormativa";
		public TipoModalidadFormativaDTO GetById(TipoModalidadFormativaDTO dto)
		{
			var result = RestClient.PostApi<TipoModalidadFormativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoModalidadFormativaDTO> Search(TipoModalidadFormativaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoModalidadFormativaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoModalidadFormativaDTO SingleSearch(TipoModalidadFormativaDTO dto)
		{
			var result = RestClient.PostApi<TipoModalidadFormativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoModalidadFormativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoModalidadFormativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoModalidadFormativaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
