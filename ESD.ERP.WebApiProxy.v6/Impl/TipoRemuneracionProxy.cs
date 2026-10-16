using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoRemuneracionProxy : WebApiProxyClient, ITipoRemuneracionProxy
	{
		private const string API_CONTROLLER = "TipoRemuneracion";
		public TipoRemuneracionDTO GetById(TipoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<TipoRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoRemuneracionDTO> Search(TipoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<List<TipoRemuneracionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoRemuneracionDTO SingleSearch(TipoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<TipoRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoRemuneracionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
