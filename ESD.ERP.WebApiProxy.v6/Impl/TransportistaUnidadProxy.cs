using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TransportistaUnidadProxy : WebApiProxyClient, ITransportistaUnidadProxy
	{
		private const string API_CONTROLLER = "TransportistaUnidad";
		public TransportistaUnidadDTO GetById(TransportistaUnidadDTO dto)
		{
			var result = RestClient.PostApi<TransportistaUnidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TransportistaUnidadDTO> Search(TransportistaUnidadDTO dto)
		{
			var result = RestClient.PostApi<List<TransportistaUnidadDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TransportistaUnidadDTO SingleSearch(TransportistaUnidadDTO dto)
		{
			var result = RestClient.PostApi<TransportistaUnidadDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TransportistaUnidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TransportistaUnidadDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TransportistaUnidadDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
