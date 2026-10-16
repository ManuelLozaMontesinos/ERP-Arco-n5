using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ImpuestoTasaProxy : WebApiProxyClient, IImpuestoTasaProxy
	{
		private const string API_CONTROLLER = "ImpuestoTasa";
		public ImpuestoTasaDTO GetById(ImpuestoTasaDTO dto)
		{
			var result = RestClient.PostApi<ImpuestoTasaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ImpuestoTasaDTO> Search(ImpuestoTasaDTO dto)
		{
			var result = RestClient.PostApi<List<ImpuestoTasaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ImpuestoTasaDTO SingleSearch(ImpuestoTasaDTO dto)
		{
			var result = RestClient.PostApi<ImpuestoTasaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ImpuestoTasaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ImpuestoTasaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ImpuestoTasaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
