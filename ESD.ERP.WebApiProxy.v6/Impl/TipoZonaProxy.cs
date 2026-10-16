using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoZonaProxy : WebApiProxyClient, ITipoZonaProxy
	{
		private const string API_CONTROLLER = "TipoZona";
		public TipoZonaDTO GetById(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<TipoZonaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoZonaDTO> Search(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoZonaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoZonaDTO SingleSearch(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<TipoZonaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoZonaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoZonaDTO ObtenerPorCodigo(TipoZonaDTO dto)
		{
			var result = RestClient.PostApi<TipoZonaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
