using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenCompraRepository : IOrdenCompraRepository
	{
		private const string SP_PRE_NAME = "sp_orden_co";
		public OrdenCompra GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenCompra),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(TipoProducto),
						typeof(PedidoCompra),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(Moneda),
						typeof(ImpuestoTasa),
						typeof(EstadoAprobacion),
						typeof(EstadoOrdenCompra)
					},
					x =>
					{
						OrdenCompra oc = x[0] as OrdenCompra;
						PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
						EmpresaSede es = x[2] as EmpresaSede;
						TipoProducto tp = x[3] as TipoProducto;
						PedidoCompra pc = x[4] as PedidoCompra;
						Proveedor p = x[5] as Proveedor;
						TipoDocumentoIdentidad tdi = x[6] as TipoDocumentoIdentidad;
						Moneda m = x[7] as Moneda;
						ImpuestoTasa it = x[8] as ImpuestoTasa;
						EstadoAprobacion ea = x[9] as EstadoAprobacion;
						EstadoOrdenCompra eoc = x[10] as EstadoOrdenCompra;

						oc.PeriodoEmpresaSede = pes;
						oc.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EmpresaSedeId;
						oc.TipoProducto = tp;
						oc.TipoProductoId = tp.EntityId;
						oc.PedidoCompra = pc;
						oc.PedidoCompraId = pc?.EntityId;
						oc.Proveedor = p;
						oc.ProveedorId = p.EntityId;
						p.TipoDocumentoIdentidad = tdi;
						p.TipoDocumentoIdentidadId = tdi.EntityId;
						oc.Moneda = m;
						oc.MonedaId = m.EntityId;
						oc.ImpuestoTasa = it;
						oc.ImpuestoTasaId = it.EntityId;
						oc.EstadoAprobacion = ea;
						oc.EstadoAprobacionId = ea?.EstadoAprobacionId;
						oc.EstadoOrdenCompra = eoc;
						oc.EstadoOrdenCompraId = eoc.EntityId;

						return oc;
					},
					"periodo_emp_sede_id,empresa_sede_id,tipo_producto_id,pedido_co_id,proveedor_id," +
					"tipo_doc_iden_id,moneda_id,impuesto_tasa_id,estado_aproba_id,estado_orden_co_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public List<OrdenCompra> Search(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("ordenCompra", entity.Codigo);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("tipoProductoId", entity?.TipoProductoId);
			parameters.AddParameter("tipoProducto", entity?.TipoProducto?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estadoOrdenCompraId", entity.EstadoOrdenCompraId);
			parameters.AddParameter("estadoOrdenCompraList", entity.EstadoOrdenCompraList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenCompra),
						typeof(TipoProducto),
						typeof(Proveedor),
						typeof(Moneda),
						typeof(EstadoAprobacion),
						typeof(EstadoOrdenCompra)
					},
					x =>
					{
						OrdenCompra oc = x[0] as OrdenCompra;
						TipoProducto tp = x[1] as TipoProducto;
						Proveedor p = x[2] as Proveedor;
						Moneda m = x[3] as Moneda;
						EstadoAprobacion ea = x[4] as EstadoAprobacion;
						EstadoOrdenCompra eoc = x[5] as EstadoOrdenCompra;

						oc.TipoProducto = tp;
						oc.TipoProductoId = tp.EntityId;
						oc.Proveedor = p;
						oc.ProveedorId = p.EntityId;
						oc.Moneda = m;
						oc.MonedaId = p.EntityId;
						oc.EstadoAprobacion = ea;
						oc.EstadoAprobacionId = ea?.EstadoAprobacionId;
						oc.EstadoOrdenCompra = eoc;
						oc.EstadoOrdenCompraId = eoc.EntityId;

						return oc;
					},
					"tipo_producto_id,proveedor_id,moneda_id,estado_aproba_id,estado_orden_co_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public OrdenCompra SingleSearch(OrdenCompra entity)
		{
			try
			{
				List<OrdenCompra> ordenCompraList = Search(entity);

				return ordenCompraList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public int Insert(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("ordenCompra", entity.Codigo);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("tipoProductoId", entity.TipoProductoId);
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("fechaEntrega", entity.FechaEntrega);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("tipoCambio", entity.TipoCambio);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoImpuesto", entity.MontoImpuesto);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("plazoEntrega", entity.PlazoEntrega);
				parameters.AddParameter("lugarEntrega", entity.LugarEntrega);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("permitePagoParcial", entity.PermitePagoParcial);
				parameters.AddParameter("esModificacion", entity.EsModificacion);
				parameters.AddParameter("estadoOrdenCompraId", entity.EstadoOrdenCompraId);
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
		public int Update(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("fechaEntrega", entity.FechaEntrega);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("tipoCambio", entity.TipoCambio);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("plazoEntrega", entity.PlazoEntrega);
				parameters.AddParameter("lugarEntrega", entity.LugarEntrega);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("esModificacion", entity.EsModificacion);
				parameters.AddParameter("estadoOrdenCompraAnteriorId", entity.EstadoOrdenCompraAnteriorId);
				parameters.AddParameter("estadoOrdenCompraId", entity.EstadoOrdenCompraId);
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
		public void Delete(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("estadoOrdenCompraId", entity.EstadoOrdenCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public void ActualizarMonto(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMonto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public void ActualizarEstado(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("estadoOrdenCompraAnteriorId", entity.EstadoOrdenCompraAnteriorId);
				parameters.AddParameter("estadoOrdenCompraId", entity.EstadoOrdenCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoAprobacion(OrdenCompra entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
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
        public void ActualizarProductos(OrdenCompra entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("Id", entity.OrdenCompraId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarProductos"), parameters);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
    }
}
