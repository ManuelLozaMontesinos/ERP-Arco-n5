using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoOperacionAnticipoProxy : WebApiProxyClient, ITipoOperacionAnticipoProxy
	{
		private const string API_CONTROLLER = "TipoOperacionAnticipo";
		public TipoOperacionAnticipoDTO GetById(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<TipoOperacionAnticipoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoOperacionAnticipoDTO> Search(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoOperacionAnticipoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoOperacionAnticipoDTO SingleSearch(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<TipoOperacionAnticipoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoOperacionAnticipoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoOperacionAnticipoDTO ObtenerPorCodigo(TipoOperacionAnticipoDTO dto)
		{
			var result = RestClient.PostApi<TipoOperacionAnticipoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
