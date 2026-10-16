using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class NotaCreditoProductoRepository : INotaCreditoProductoRepository
	{
		private const string SP_PRE_NAME = "sp_nota_c_prod";
		public NotaCreditoProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(NotaCreditoProducto)
					},
					x =>
					{
						NotaCreditoProducto ncp = x[0] as NotaCreditoProducto;

						return ncp;
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
		public List<NotaCreditoProducto> Search(NotaCreditoProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(NotaCreditoProducto),
						typeof(ComprobanteProducto),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),
						typeof(UnidadMedida),
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv)
					},
					x =>
					{
						NotaCreditoProducto ncp = x[0] as NotaCreditoProducto;
						ComprobanteProducto cp = x[1] as ComprobanteProducto;
						Producto p = x[2] as Producto;
						FamiliaProducto fp = x[3] as FamiliaProducto;
						ClaseProducto cpr = x[4] as ClaseProducto;
						GrupoProducto gp = x[5] as GrupoProducto;
						TipoProducto tp = x[6] as TipoProducto;
						UnidadMedida um = x[7] as UnidadMedida;
						TipoIgv ti = x[8] as TipoIgv;
						UnidadMedida um2 = x[9] as UnidadMedida;
						TipoAfectacionIgv ta = x[10] as TipoAfectacionIgv;
						TipoIgv ti2 = x[11] as TipoIgv;

						ncp.ComprobanteProducto = cp;
						ncp.ComprobanteProductoId = cp?.EntityId;
						if (!Equals(cp, null))
						{
							cp.Producto = p;
							cp.ProductoId = p.EntityId;
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
							cp.UnidadMedida = um2;
							cp.UnidadMedidaId = um2?.EntityId;
						}
						ncp.TipoAfectacionIgv = ta;
						ncp.TipoAfectacionIgvId = ta.EntityId;
						ta.TipoIgv = ti2;
						ta.TipoIgvId = ti2.EntityId;

						return ncp;
					},
					"comprob_prod_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id,tipo_igv_id,unidad_medida_id,tipo_afecta_igv_id," +
					"tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public NotaCreditoProducto SingleSearch(NotaCreditoProducto entity)
		{
			try
			{
				List<NotaCreditoProducto> notaCreditoProductoList = Search(entity);

				return notaCreditoProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(NotaCreditoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoId", entity.NotaCreditoId);
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoDescuento1", entity.MontoDescuento1);
				parameters.AddParameter("montoDescuento2", entity.MontoDescuento2);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("TipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIscId", entity.TipoIscId);
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
		public int Update(NotaCreditoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoProductoId", entity.NotaCreditoProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("MontoDescuento1", entity.MontoDescuento1);
				parameters.AddParameter("MontoDescuento2", entity.MontoDescuento2);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIscId", entity.TipoIscId);
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
		public void Delete(NotaCreditoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("notaCreditoProductoId", entity.NotaCreditoProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
