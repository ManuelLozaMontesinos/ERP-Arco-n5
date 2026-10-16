using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoContratoProxy : WebApiProxyClient, ITipoContratoProxy
	{
		private const string API_CONTROLLER = "TipoContrato";
		public TipoContratoDTO GetById(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<TipoContratoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoContratoDTO> Search(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoContratoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoContratoDTO SingleSearch(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<TipoContratoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoContratoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoContratoDTO ObtenerPorCodigo(TipoContratoDTO dto)
		{
			var result = RestClient.PostApi<TipoContratoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
