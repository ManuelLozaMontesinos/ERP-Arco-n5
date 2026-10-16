using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ConvenioDobleTributacionProxy : WebApiProxyClient, IConvenioDobleTributacionProxy
	{
		private const string API_CONTROLLER = "ConvenioDobleTributacion";
		public ConvenioDobleTributacionDTO GetById(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<ConvenioDobleTributacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ConvenioDobleTributacionDTO> Search(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<List<ConvenioDobleTributacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ConvenioDobleTributacionDTO SingleSearch(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<ConvenioDobleTributacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ConvenioDobleTributacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public ConvenioDobleTributacionDTO ObtenerPorCodigo(ConvenioDobleTributacionDTO dto)
		{
			var result = RestClient.PostApi<ConvenioDobleTributacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
