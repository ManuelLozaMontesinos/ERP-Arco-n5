using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenCompraProductoRepository : IOrdenCompraProductoRepository
	{
		private const string SP_PRE_NAME = "sp_orden_c_prod";
		public OrdenCompraProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenCompraProducto),
						typeof(OrdenCompra),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
					},
					x =>
					{
						OrdenCompraProducto ocp = x[0] as OrdenCompraProducto;
						OrdenCompra oc = x[1] as OrdenCompra;
						Producto p = x[2] as Producto;
						FamiliaProducto fp = x[3] as FamiliaProducto;
						ClaseProducto cpr = x[4] as ClaseProducto;
						GrupoProducto gp = x[5] as GrupoProducto;

						ocp.OrdenCompra = oc;
						ocp.OrdenCompraId = oc.EntityId;
						ocp.Producto = p;
						ocp.ProductoId = p.EntityId;
						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cpr;
						fp.ClaseProductoId = cpr.EntityId;
						cpr.GrupoProducto = gp;
						cpr.GrupoProductoId = gp.EntityId;
						return ocp;
					},
						"orden_co_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public List<OrdenCompraProducto> Search(OrdenCompraProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
			parameters.AddParameter("ordenCompraProductoIdList", entity.OrdenCompraProductoIdList);
			parameters.AddParameter("conCantidadPendiente", entity.ConCantidadPendiente);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenCompraProducto),
						typeof(OrdenCompra),
						typeof(ImpuestoTasa),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),

					},
					x =>
					{
						OrdenCompraProducto ocp = x[0] as OrdenCompraProducto;
						OrdenCompra oc = x[1] as OrdenCompra;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Producto p = x[3] as Producto;
						FamiliaProducto fp = x[4] as FamiliaProducto;
						ClaseProducto cpr = x[5] as ClaseProducto;
						GrupoProducto gp = x[6] as GrupoProducto;
						TipoProducto tp = x[7] as TipoProducto;
						UnidadMedida um = x[8] as UnidadMedida;
						TipoIgv ti = x[9] as TipoIgv;

						ocp.Producto = p;
						ocp.ProductoId = p.EntityId;
						ocp.OrdenCompra = oc;
						ocp.OrdenCompraId = oc.EntityId;
						oc.ImpuestoTasa = it;
						oc.ImpuestoTasaId = it.EntityId;
						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cpr;
						fp.ClaseProductoId = cpr.EntityId;
						cpr.GrupoProducto = gp;
						cpr.GrupoProductoId = gp.EntityId;
						gp.TipoProducto = tp;
						gp.TipoProductoId = tp.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return ocp;
					},
					"orden_co_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id,tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public OrdenCompraProducto SingleSearch(OrdenCompraProducto entity)
		{
			try
			{
				List<OrdenCompraProducto> ordenCompraProductoList = Search(entity);

				return ordenCompraProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public int Insert(OrdenCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("OrdenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("cantidadIngresada", entity.CantidadIngresada);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoImpuesto", entity.MontoImpuesto);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("esAdicional", entity.EsAdicional);
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
		public int Update(OrdenCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraProductoId", entity.OrdenCompraProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoImpuesto", entity.MontoImpuesto);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
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
		public void Delete(OrdenCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraProductoId", entity.OrdenCompraProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
      public void EliminarPorOrdenCompraId(OrdenCompraProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorOrdenCompraId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
            throw;
         }
		}
		public void ActualizarCantidadIngresada(OrdenCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("ordenCompraProductoId", entity.OrdenCompraProductoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCantidadIngresada"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
