using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDatoProxy : WebApiProxyClient, ITipoDatoProxy
	{
		private const string API_CONTROLLER = "TipoDato";
		public TipoDatoDTO GetById(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<TipoDatoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDatoDTO> Search(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDatoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDatoDTO SingleSearch(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<TipoDatoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDatoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoDatoDTO ObtenerPorCodigo(TipoDatoDTO dto)
		{
			var result = RestClient.PostApi<TipoDatoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
