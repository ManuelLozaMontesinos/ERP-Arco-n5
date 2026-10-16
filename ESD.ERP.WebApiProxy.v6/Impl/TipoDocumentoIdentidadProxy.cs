using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoDocumentoIdentidadProxy : WebApiProxyClient, ITipoDocumentoIdentidadProxy
	{
		private const string API_CONTROLLER = "TipoDocumentoIdentidad";
		public TipoDocumentoIdentidadDTO GetById(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoIdentidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoDocumentoIdentidadDTO> Search(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<List<TipoDocumentoIdentidadDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoDocumentoIdentidadDTO SingleSearch(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoIdentidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoDocumentoIdentidadDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoDocumentoIdentidadDTO ObtenerPorCodigo(TipoDocumentoIdentidadDTO dto)
		{
			var result = RestClient.PostApi<TipoDocumentoIdentidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
