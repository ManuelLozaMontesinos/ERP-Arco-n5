using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
    public class PuestoProxy : WebApiProxyClient, IPuestoProxy
    {
        private const string API_CONTROLLER = "Puesto";
        public PuestoDTO GetById(PuestoDTO dto)
        {
            var result = RestClient.PostApi<PuestoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
            return result;
        }
        public List<PuestoDTO> Search(PuestoDTO dto)
        {
            var result = RestClient.PostApi<List<PuestoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
            return result;
        }
        public PuestoDTO SingleSearch(PuestoDTO dto)
        {
            var result = RestClient.PostApi<PuestoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
            return result;
        }
        public int Insert(PuestoDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
            return result;
        }
        public int Update(PuestoDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
            return result;
        }

    }
}
