using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PeriodicidadRemunerativaProxy : WebApiProxyClient, IPeriodicidadRemunerativaProxy
	{
		private const string API_CONTROLLER = "PeriodicidadRemunerativa";
		public PeriodicidadRemunerativaDTO GetById(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<PeriodicidadRemunerativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PeriodicidadRemunerativaDTO> Search(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<List<PeriodicidadRemunerativaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PeriodicidadRemunerativaDTO SingleSearch(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<PeriodicidadRemunerativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PeriodicidadRemunerativaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public PeriodicidadRemunerativaDTO ObtenerPorCodigo(PeriodicidadRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<PeriodicidadRemunerativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
