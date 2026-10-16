using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
    public class ContactosProxy : WebApiProxyClient, IContactosProxy
    {
        private const string API_CONTROLLER = "Contactos";
        public ContactosDTO GetById(ContactosDTO dto)
        {
            var result = RestClient.PostApi<ContactosDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
            return result;
        }
        public List<ContactosDTO> Search(ContactosDTO dto)
        {
            var result = RestClient.PostApi<List<ContactosDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
            return result;
        }
        public ContactosDTO SingleSearch(ContactosDTO dto)
        {
            var result = RestClient.PostApi<ContactosDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
            return result;
        }
        public int Insert(ContactosDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
            return result;
        }
        public int Update(ContactosDTO dto)
        {
            var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
            return result;
        }
        public void Delete(ContactosDTO dto)
        {
            var result = RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
        }

    }
}
