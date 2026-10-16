using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class VendedorSerieProxy : WebApiProxyClient, IVendedorSerieProxy
	{
		private const string API_CONTROLLER = "VendedorSerie";
		public VendedorSerieDTO GetById(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<VendedorSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<VendedorSerieDTO> Search(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<List<VendedorSerieDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public VendedorSerieDTO SingleSearch(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<VendedorSerieDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(VendedorSerieDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void ActualizarListaDocumentoComercialSerieId(VendedorSerieDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarListaDocumentoComercialSerieId"), dto);
		}
		public List<TipoDocumentoComercialDTO> ListarTipoDocumentoComercialPorVendedor(VendedorSerieDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDocumentoComercialDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarTipoDocumentoComercialPorVendedor"), dto);
			return result;
		}
      public List<AlmacenDTO> ListarAlmacenPorVendedor(VendedorSerieDTO dto)
      {
			var result = RestClient.PostApi<List<AlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarAlmacenPorVendedor"), dto);
			return result;
		}
   }
}
