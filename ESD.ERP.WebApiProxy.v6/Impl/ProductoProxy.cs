using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
    public class ProductoProxy : WebApiProxyClient, IProductoProxy
    {
        private const string API_CONTROLLER = "Producto";
        public ProductoDTO GetById(ProductoDTO dto)
        {
            var result = RestClient.PostApi<ProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
            return result;
        }
        public List<ProductoDTO> Search(ProductoDTO dto)
        {
            var result = RestClient.PostApi<List<ProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
            return result;
        }
        public ProductoDTO SingleSearch(ProductoDTO dto)
        {
            var result = RestClient.PostApi<ProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
            return result;
        }
        public int Insert(ProductoDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
            return result;
        }
        public int Update(ProductoDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
            return result;
        }
        public void Delete(ProductoDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
        }
        public void ActualizarPesoMasterPack(ProductoDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarPesoMasterPack"), dto);
        }
    }
}
