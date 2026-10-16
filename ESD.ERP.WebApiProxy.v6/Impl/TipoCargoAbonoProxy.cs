using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoCargoAbonoProxy : WebApiProxyClient, ITipoCargoAbonoProxy
	{
		private const string API_CONTROLLER = "TipoCargoAbono";
		public TipoCargoAbonoDTO GetById(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<TipoCargoAbonoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoCargoAbonoDTO> Search(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoCargoAbonoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoCargoAbonoDTO SingleSearch(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<TipoCargoAbonoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoCargoAbonoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoCargoAbonoDTO ObtenerPorCodigo(TipoCargoAbonoDTO dto)
		{
			var result = RestClient.PostApi<TipoCargoAbonoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
