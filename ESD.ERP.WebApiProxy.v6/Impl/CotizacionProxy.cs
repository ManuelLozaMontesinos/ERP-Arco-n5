using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CotizacionProxy : WebApiProxyClient, ICotizacionProxy
	{
		private const string API_CONTROLLER = "Cotizacion";
		public CotizacionDTO GetById(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<CotizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CotizacionDTO> Search(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<List<CotizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CotizacionDTO SingleSearch(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<CotizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CotizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public int GenerarComprobante(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "GenerarComprobante"), dto);
			return result;
		}
      public void GenerarPedido(CotizacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "GenerarPedido"), dto);
		}
      public void CambiarEstado(CotizacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CambiarEstado"), dto);
	  }
		public int GenerarOrdenProduccion(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "GenerarOrdenProduccion"), dto);
			return result;
		}

        public int GenerarOrdenProduccionParcial(CotizacionDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "GenerarOrdenProduccionParcial"), dto);
            return result;
        }

		public decimal CortesGratisParcial(CotizacionDTO dto)
		{
			var result = RestClient.PostApi<decimal>(WebApiMethod.Create(API_CONTROLLER, "CortesGratisParcial"), dto);
			return result;
		}
    }
}