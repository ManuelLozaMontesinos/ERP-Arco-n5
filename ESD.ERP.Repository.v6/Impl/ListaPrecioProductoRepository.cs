using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioProductoRepository : IListaPrecioProductoRepository
	{
		private const string SP_PRE_NAME = "sp_lista_p_prod";
		public ListaPrecioProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecioProducto),
						typeof(Producto),
						typeof(TipoIgv)
					},
					x =>
					{
						ListaPrecioProducto lpp = x[0] as ListaPrecioProducto;
						Producto p = x[1] as Producto;
						TipoIgv ti = x[2] as TipoIgv;

						lpp.Producto = p;
						lpp.ProductoId = p.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return lpp;
					},
					"producto_id,tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ListaPrecioProducto> Search(ListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("containsDescripcion", entity.Producto?.Descripcion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecioProducto),
						typeof(ListaPrecio),
						typeof(ImpuestoTasa),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoIgv)
					},
					x =>
					{
						ListaPrecioProducto lpp = x[0] as ListaPrecioProducto;
						ListaPrecio lp = x[1] as ListaPrecio;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Producto p = x[3] as Producto;
						FamiliaProducto fp = x[4] as FamiliaProducto;
						ClaseProducto cp = x[5] as ClaseProducto;
						GrupoProducto gp = x[6] as GrupoProducto;
						TipoIgv ti = x[7] as TipoIgv;

						lpp.ListaPrecio = lp;
						lpp.ListaPrecioId = lp.EntityId;
						lp.ImpuestoTasa = it;
						lp.ImpuestoTasaId = it.EntityId;
						lpp.Producto = p;
						lpp.ProductoId = p.EntityId;
						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cp;
						fp.ClaseProductoId = cp.EntityId;
						cp.GrupoProducto = gp;
						cp.GrupoProductoId = gp.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return lpp;
					},
					"lista_precio_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id,tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecioProducto SingleSearch(ListaPrecioProducto entity)
		{
			try
			{
				List<ListaPrecioProducto> listaPrecioProductoList = Search(entity);

				return listaPrecioProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("permiteDescuento", entity.PermiteDescuento);
				parameters.AddParameter("estado", entity.Estado);
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
		public int Update(ListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProductoId", entity.ListaPrecioProductoId);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("permiteDescuento", entity.PermiteDescuento);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(ListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProductoId", entity.ListaPrecioProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<ListaPrecioProducto> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<ListaPrecioProductoType> listaPrecioProductoTypeList = new();
				listaPrecioProductoTypeList = (from s in list
														 select new ListaPrecioProductoType
														 {
															 ListaPrecioProductoId = s.ListaPrecioProductoId,
															 ListaPrecioId = s.ListaPrecioId,
															 ProductoId = s.ProductoId,
															 ValorUnitario = s.ValorUnitario,
															 PrecioUnitario = s.PrecioUnitario,
															 Estado = s.Estado,
															 UsuarioCreacionId = s.UsuarioCreacionId,
														 }).ToList();

				parameters.AddTableParameter("listaPrecioProductoList", "dbo.ListaPrecioProductoType", listaPrecioProductoTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarSaldoAlmacen(ListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
			parameters.AddParameter("mesContableId", entity.MesContableId);
			parameters.AddParameter("periodoId", entity.PeriodoId);
			parameters.AddParameter("producto", entity.Producto.Codigo);
			parameters.AddParameter("descripcion", entity.Producto.Descripcion);
			parameters.AddParameter("codigoReferencial", entity.Producto.CodigoReferencial);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarSaldoAlmacen"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
