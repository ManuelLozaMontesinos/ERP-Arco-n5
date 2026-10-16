using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CotizacionProductoProxy : WebApiProxyClient, ICotizacionProductoProxy
	{
		private const string API_CONTROLLER = "CotizacionProducto";
		public CotizacionProductoDTO GetById(CotizacionProductoDTO dto)
		{
			var result = RestClient.PostApi<CotizacionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CotizacionProductoDTO> Search(CotizacionProductoDTO dto)
		{
			var result = RestClient.PostApi<List<CotizacionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CotizacionProductoDTO SingleSearch(CotizacionProductoDTO dto)
		{
			var result = RestClient.PostApi<CotizacionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CotizacionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CotizacionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CotizacionProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarSinSaldo(CotizacionProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarSinSaldo"), dto);
		}
        public List<CotizacionProductoDTO> CortesGratis(CotizacionProductoDTO dto)
        {
            var result = RestClient.PostApi<List<CotizacionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "CortesGratis"), dto);
            return result;
        }
        public List<CotizacionProductoDTO> CortesGratisIlo(CotizacionProductoDTO dto)
        {
            var result = RestClient.PostApi<List<CotizacionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "CortesGratisIlo"), dto);
            return result;
        }
        public List<CotizacionProductoDTO> ProductosParcial(CotizacionProductoDTO dto)
        {
            var result = RestClient.PostApi<List<CotizacionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "ProductosParcial"), dto);
            return result;
        }
    }
}
