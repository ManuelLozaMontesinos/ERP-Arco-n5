using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenProxy : WebApiProxyClient, IMovimientoAlmacenProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacen";
		public MovimientoAlmacenDTO GetById(MovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenDTO> Search(MovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenDTO SingleSearch(MovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public int RecepcionarTransferencia(MovimientoAlmacenDTO dto)
      {
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "RecepcionarTransferencia"), dto);
			return result;
	  }
      public List<MovimientoAlmacenDTO> BuscarPorEmpresa(MovimientoAlmacenDTO dto)
      {
			var result = RestClient.PostApi<List<MovimientoAlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, "BuscarPorEmpresa"), dto);
			return result;
	  }
      public void SolicitarAutorizacion(MovimientoAlmacenDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarAutorizacion"), dto);
	  }
	  public void SolicitarModificacionEdicion(MovimientoAlmacenDTO dto)
	  {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarModificacionEdicion"), dto);
	  }
      public void Aprobar(MovimientoAlmacenDTO dto)
	  {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
	  }
	  public void Desaprobar(MovimientoAlmacenDTO dto)
	  {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
	  }
	}
}
