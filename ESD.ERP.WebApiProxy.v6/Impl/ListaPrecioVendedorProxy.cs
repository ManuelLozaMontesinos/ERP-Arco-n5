using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
    public class ListaPrecioVendedorProxy : WebApiProxyClient, IListaPrecioVendedorProxy
    {
        private const string API_CONTROLLER = "ListaPrecioVendedor";
        public ListaPrecioVendedorDTO GetById(ListaPrecioVendedorDTO dto)
        {
            var result = RestClient.PostApi<ListaPrecioVendedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
            return result;
        }
        public List<ListaPrecioVendedorDTO> Search(ListaPrecioVendedorDTO dto)
        {
            var result = RestClient.PostApi<List<ListaPrecioVendedorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
            return result;
        }
        public ListaPrecioVendedorDTO SingleSearch(ListaPrecioVendedorDTO dto)
        {
            var result = RestClient.PostApi<ListaPrecioVendedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
            return result;
        }
        public int Insert(ListaPrecioVendedorDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
            return result;
        }
        public int Update(ListaPrecioVendedorDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
            return result;
        }
        public void Delete(ListaPrecioVendedorDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
        }
        public void ActualizarListaPrecioVendedorId(ListaPrecioVendedorDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarListaPrecioVendedorId"), dto);
        }
    }
}
