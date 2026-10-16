using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PedidoCompraRepository : IPedidoCompraRepository
	{
		private const string SP_PRE_NAME = "sp_pedido_co";
		public PedidoCompra GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PedidoCompra),
						typeof(TipoProducto),
						typeof(Proveedor),
						typeof(EstadoAprobacion),
						typeof(EstadoPedidoCompra)
					},
					x =>
					{
						PedidoCompra pc = x[0] as PedidoCompra;
						TipoProducto tp = x[1] as TipoProducto;
						Proveedor p = x[2] as Proveedor;
						EstadoAprobacion ea = x[3] as EstadoAprobacion;
						EstadoPedidoCompra epc = x[4] as EstadoPedidoCompra;

						pc.TipoProducto = tp;
						pc.TipoProductoId = tp.EntityId;
						pc.Proveedor = p;
						pc.ProveedorId = p.EntityId;
						pc.EstadoAprobacion = ea;
						pc.EstadoAprobacionId = ea?.EntityId;
						pc.EstadoPedidoCompraId = epc.EntityId;
						pc.EstadoPedidoCompra = epc;

						return pc;
					},
					"tipo_producto_id,proveedor_id,estado_aproba_id,estado_pedido_co_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PedidoCompra> Search(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("pedidoCompra", entity.Codigo);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaDesde);
			parameters.AddParameter("tipoProductoId", entity?.TipoProductoId);
			parameters.AddParameter("tipoProducto", entity?.TipoProducto?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estadoPedidoCompra", entity?.EstadoPedidoCompra?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PedidoCompra),
						typeof(TipoProducto),
						typeof(Proveedor),
						typeof(EstadoAprobacion),
						typeof(EstadoPedidoCompra)
					},
					x =>
					{
						PedidoCompra pc = x[0] as PedidoCompra;
						TipoProducto tp = x[1] as TipoProducto;
						Proveedor p = x[2] as Proveedor;
						EstadoAprobacion ea = x[3] as EstadoAprobacion;
						EstadoPedidoCompra epc = x[4] as EstadoPedidoCompra;

						pc.TipoProducto = tp;
						pc.TipoProductoId = tp.EntityId;
						pc.Proveedor = p;
						pc.ProveedorId = p.EntityId;
						pc.EstadoAprobacion = ea;
						pc.EstadoAprobacionId = ea?.EstadoAprobacionId;
						pc.EstadoPedidoCompra = epc;
						pc.EstadoPedidoCompraId = epc.EstadoPedidoCompraId;

						return pc;
					},
					"tipo_producto_id,proveedor_id,estado_aproba_id,estado_pedido_co_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PedidoCompra SingleSearch(PedidoCompra entity)
		{
			try
			{
				List<PedidoCompra> pedidoCoList = Search(entity);

				return pedidoCoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("pedidoCompra", entity.Codigo);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("esModificacion", entity.EsModificacion);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoPedidoCompraId", entity.EstadoPedidoCompraId);
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
		public int Update(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("esModificacion", entity.EsModificacion);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoPedidoCompraId", entity.EstadoPedidoCompraId);
				parameters.AddParameter("estadoPedidoCompraAnteriorId", entity.EstadoPedidoCompraAnteriorId);
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
		public void Delete(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoAprobacion(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("fechaAprobacion", entity.FechaAprobacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoAprobacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoPedido(PedidoCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("estadoPedidoCompraId", entity.EstadoPedidoCompraId);
				parameters.AddParameter("estadoPedidoCompraAnteriorId", entity.EstadoPedidoCompraAnteriorId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoPedido"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
