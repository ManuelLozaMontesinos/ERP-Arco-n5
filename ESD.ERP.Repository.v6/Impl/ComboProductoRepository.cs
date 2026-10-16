using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComboProductoRepository : IComboProductoRepository
	{
		private const string SP_PRE_NAME = "sp_combo_producto";
		public ComboProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComboProducto),
						typeof(Producto),
						typeof(UnidadMedida)
					},
					x =>
					{
						ComboProducto cp = x[0] as ComboProducto;
						Producto p = x[1] as Producto;
						UnidadMedida um = x[2] as UnidadMedida;

						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						cp.ProductoIncluido = p;
						cp.ProductoIncluidoId = p.EntityId;

						return cp;
					},
					"producto_id,unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ComboProducto> Search(ComboProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComboProducto),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv)
					},
					x =>
					{
						ComboProducto cp = x[0] as ComboProducto;
						Producto p = x[1] as Producto;
						FamiliaProducto fp = x[2] as FamiliaProducto;
						ClaseProducto cprd = x[3] as ClaseProducto;
						GrupoProducto gp = x[4] as GrupoProducto;
						TipoProducto tp = x[5] as TipoProducto;
						UnidadMedida um = x[6] as UnidadMedida;
						TipoIgv ti = x[7] as TipoIgv;

						cp.ProductoIncluido = p;
						cp.ProductoIncluidoId = p.EntityId;
						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cprd;
						fp.ClaseProductoId = cprd.EntityId;
						cprd.GrupoProducto = gp;
						cprd.GrupoProductoId = gp.EntityId;
						gp.TipoProducto = tp;
						gp.TipoProductoId = tp.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return cp;
					},
					"prod_inclu_id,familia_producto_id,clase_producto_id,grupo_producto_id,tipo_producto_id,unidad_medida_id,tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ComboProducto SingleSearch(ComboProducto entity)
		{
			try
			{
				List<ComboProducto> comboProductoList = Search(entity);

				return comboProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComboProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("productoIncluidoId", entity.ProductoIncluidoId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("porcentajePrecio", entity.PorcentajePrecio);
				parameters.AddParameter("numeroDecimalesRedondeo", entity.NumeroDecimalesRedondeo);
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
		public int Update(ComboProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comboProductoId", entity.ComboProductoId);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("porcentajePrecio", entity.PorcentajePrecio);
				parameters.AddParameter("numeroDecimalesRedondeo", entity.NumeroDecimalesRedondeo);
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
		public void Delete(ComboProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comboProductoId", entity.ComboProductoId);
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
