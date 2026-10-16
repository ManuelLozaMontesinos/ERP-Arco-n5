using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoPagoRemuneracionProxy : WebApiProxyClient, ITipoPagoRemuneracionProxy
	{
		private const string API_CONTROLLER = "TipoPagoRemuneracion";
		public TipoPagoRemuneracionDTO GetById(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<TipoPagoRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoPagoRemuneracionDTO> Search(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<List<TipoPagoRemuneracionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoPagoRemuneracionDTO SingleSearch(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<TipoPagoRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoPagoRemuneracionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoPagoRemuneracionDTO ObtenerPorCodigo(TipoPagoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<TipoPagoRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
