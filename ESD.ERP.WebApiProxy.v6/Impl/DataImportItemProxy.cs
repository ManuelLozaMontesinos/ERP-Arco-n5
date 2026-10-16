using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class DataImportItemProxy : WebApiProxyClient, IDataImportItemProxy
	{
		private const string API_CONTROLLER = "DataImportItem";
		public DataImportItemDTO GetById(DataImportItemDTO dto)
		{
			var result = RestClient.PostApi<DataImportItemDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<DataImportItemDTO> Search(DataImportItemDTO dto)
		{
			var result = RestClient.PostApi<List<DataImportItemDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public DataImportItemDTO SingleSearch(DataImportItemDTO dto)
		{
			var result = RestClient.PostApi<DataImportItemDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(DataImportItemDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(DataImportItemDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(DataImportItemDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
