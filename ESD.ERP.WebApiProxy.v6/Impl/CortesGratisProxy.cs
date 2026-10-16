using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
    public class CortesGratisProxy : WebApiProxyClient, ICortesGratisProxy
    {
        private const string API_CONTROLLER = "CortesGratis";
        public CortesGratisDTO GetById(CortesGratisDTO dto)
        {
            var result = RestClient.PostApi<CortesGratisDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
            return result;
        }
        public List<CortesGratisDTO> Search(CortesGratisDTO dto)
        {
            var result = RestClient.PostApi<List<CortesGratisDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
            return result;
        }
        public CortesGratisDTO SingleSearch(CortesGratisDTO dto)
        {
            var result = RestClient.PostApi<CortesGratisDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
            return result;
        }
        public int Insert(CortesGratisDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
            return result;
        }
        public int Update(CortesGratisDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
            return result;
        }
      
    }
}
