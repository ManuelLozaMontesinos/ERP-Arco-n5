using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenCompraEntregaRepository : IOrdenCompraEntregaRepository
	{
		private const string SP_PRE_NAME = "sp_orden_c_ent";
		public OrdenCompraEntrega GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenCompraEntrega)
					},
					x =>
					{
						OrdenCompraEntrega oce = x[0] as OrdenCompraEntrega;

						return oce;
					},
					string.Empty
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<OrdenCompraEntrega> Search(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity?.OrdenCompraProducto?.OrdenCompra?.PeriodoEmpresaSede?.EmpresaSede?.EmpresaId);
			parameters.AddParameter("empresaSedeId", entity?.OrdenCompraProducto?.OrdenCompra?.PeriodoEmpresaSede?.EmpresaSedeId);
			parameters.AddParameter("ordenCompraProductoId", entity.OrdenCompraProductoId);
			parameters.AddParameter("containsDescripcion", entity?.OrdenCompraProducto?.Producto?.Descripcion);
			parameters.AddParameter("containsRazonSocial", entity?.OrdenCompraProducto?.OrdenCompra?.Proveedor?.RazonSocial);
			parameters.AddParameter("fecha", entity.Fecha);
			parameters.AddParameter("conCantidadPendiente", entity?.OrdenCompraProducto?.ConCantidadPendiente);
			parameters.AddParameter("estadoAprobacion", entity?.OrdenCompraProducto?.OrdenCompra?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estadoOrdenCompraList", entity?.OrdenCompraProducto?.OrdenCompra?.EstadoOrdenCompraList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenCompraEntrega),
						typeof(OrdenCompraProducto),
						typeof(Producto),
						typeof(UnidadMedida),
						typeof(OrdenCompra),
						typeof(Proveedor)
					},
					x =>
					{
						OrdenCompraEntrega oce = x[0] as OrdenCompraEntrega;
						OrdenCompraProducto ocp = x[1] as OrdenCompraProducto;
						Producto p = x[2] as Producto;
						UnidadMedida um = x[3] as UnidadMedida;
						OrdenCompra oc = x[4] as OrdenCompra;
						Proveedor prv = x[5] as Proveedor;

						oce.OrdenCompraProducto = ocp;
						oce.OrdenCompraProductoId = ocp.EntityId;
						ocp.Producto = p;
						ocp.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						ocp.OrdenCompra = oc;
						ocp.OrdenCompraId = oc.EntityId;
						oc.Proveedor = prv;
						oc.ProveedorId = prv.EntityId;

						return oce;
					},
					"orden_c_prod_id,producto_id,unidad_medida_id,orden_co_id,proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public OrdenCompraEntrega SingleSearch(OrdenCompraEntrega entity)
		{
			try
			{
				List<OrdenCompraEntrega> ordenCompraEntregaList = Search(entity);

				return ordenCompraEntregaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraProductoId", entity.OrdenCompraProductoId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);
				parameters.AddNewIdParameter();

				int id = DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.INSERT), parameters);

				return id;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Update(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraEntregaId", entity.OrdenCompraEntregaId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraEntregaId", entity.OrdenCompraEntregaId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void AgregarDesdeOrdenCompraProducto(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraProducto.OrdenCompraId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_agregarDesdeOrdenCompraProducto"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarPorOrdenCompraId(OrdenCompraEntrega entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraProducto.OrdenCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorOrdenCompraId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		
	}
}
