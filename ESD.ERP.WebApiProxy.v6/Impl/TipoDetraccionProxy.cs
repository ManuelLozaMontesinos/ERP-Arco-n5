using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDetraccionProxy : WebApiProxyClient, ITipoDetraccionProxy
	{
		private const string API_CONTROLLER = "TipoDetraccion";
		public TipoDetraccionDTO GetById(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<TipoDetraccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDetraccionDTO> Search(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDetraccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDetraccionDTO SingleSearch(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<TipoDetraccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDetraccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoDetraccionDTO ObtenerPorCodigo(TipoDetraccionDTO dto)
		{
			var result = RestClient.PostApi<TipoDetraccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
