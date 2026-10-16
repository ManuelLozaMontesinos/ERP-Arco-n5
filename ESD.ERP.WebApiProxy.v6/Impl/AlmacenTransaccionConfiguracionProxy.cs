using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AlmacenTransaccionConfiguracionProxy : WebApiProxyClient, IAlmacenTransaccionConfiguracionProxy
	{
		private const string API_CONTROLLER = "AlmacenTransaccionConfiguracion";
		public AlmacenTransaccionConfiguracionDTO GetById(AlmacenTransaccionConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<AlmacenTransaccionConfiguracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AlmacenTransaccionConfiguracionDTO> Search(AlmacenTransaccionConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<List<AlmacenTransaccionConfiguracionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AlmacenTransaccionConfiguracionDTO SingleSearch(AlmacenTransaccionConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<AlmacenTransaccionConfiguracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AlmacenTransaccionConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AlmacenTransaccionConfiguracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AlmacenTransaccionConfiguracionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public List<AlmacenTransaccionConfiguracionDTO> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracionDTO dto)
      {
			var result = RestClient.PostApi<List<AlmacenTransaccionConfiguracionDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorPorAlmacenTransaccionId"), dto);
			return result;
		}
   }
}
