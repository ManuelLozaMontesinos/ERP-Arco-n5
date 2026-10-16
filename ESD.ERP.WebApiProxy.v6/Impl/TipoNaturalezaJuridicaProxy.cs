using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoNaturalezaJuridicaProxy : WebApiProxyClient, ITipoNaturalezaJuridicaProxy
	{
		private const string API_CONTROLLER = "TipoNaturalezaJuridica";
		public TipoNaturalezaJuridicaDTO GetById(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<TipoNaturalezaJuridicaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoNaturalezaJuridicaDTO> Search(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoNaturalezaJuridicaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoNaturalezaJuridicaDTO SingleSearch(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<TipoNaturalezaJuridicaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoNaturalezaJuridicaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoNaturalezaJuridicaDTO ObtenerPorCodigo(TipoNaturalezaJuridicaDTO dto)
		{
			var result = RestClient.PostApi<TipoNaturalezaJuridicaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
