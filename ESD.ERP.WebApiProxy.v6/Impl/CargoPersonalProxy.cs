using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CargoPersonalProxy : WebApiProxyClient, ICargoPersonalProxy
	{
		private const string API_CONTROLLER = "CargoPersonal";
		public CargoPersonalDTO GetById(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<CargoPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CargoPersonalDTO> Search(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<List<CargoPersonalDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CargoPersonalDTO SingleSearch(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<CargoPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CargoPersonalDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public CargoPersonalDTO ObtenerPorCodigo(CargoPersonalDTO dto)
		{
			var result = RestClient.PostApi<CargoPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
