using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SituacionEducativaProxy : WebApiProxyClient, ISituacionEducativaProxy
	{
		private const string API_CONTROLLER = "SituacionEducativa";
		public SituacionEducativaDTO GetById(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<SituacionEducativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SituacionEducativaDTO> Search(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<List<SituacionEducativaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SituacionEducativaDTO SingleSearch(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<SituacionEducativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SituacionEducativaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public SituacionEducativaDTO ObtenerPorCodigo(SituacionEducativaDTO dto)
		{
			var result = RestClient.PostApi<SituacionEducativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
