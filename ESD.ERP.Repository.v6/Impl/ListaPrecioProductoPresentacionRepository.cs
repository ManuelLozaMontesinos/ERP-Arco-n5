using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioProductoPresentacionRepository : IListaPrecioProductoPresentacionRepository
	{
		private const string SP_PRE_NAME = "sp_lista_p_prod_pres";
		public ListaPrecioProductoPresentacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecioProductoPresentacion)
					},
					x =>
					{
						ListaPrecioProductoPresentacion lppp = x[0] as ListaPrecioProductoPresentacion;

						return lppp;
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
		public List<ListaPrecioProductoPresentacion> Search(ListaPrecioProductoPresentacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("listaPrecioProductoId", entity.ListaPrecioProductoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecioProductoPresentacion),
						typeof(ListaPrecioProducto),
						typeof(ListaPrecio),
						typeof(ImpuestoTasa),
						typeof(ProductoPresentacionVenta),
						typeof(Producto),
						typeof(TipoIgv),
						typeof(UnidadMedida)
					},
					x =>
					{
						ListaPrecioProductoPresentacion lppp = x[0] as ListaPrecioProductoPresentacion;
						ListaPrecioProducto lpp = x[1] as ListaPrecioProducto;
						ListaPrecio lp = x[2] as ListaPrecio;
						ImpuestoTasa it = x[3] as ImpuestoTasa;
						ProductoPresentacionVenta ppv = x[4] as ProductoPresentacionVenta;
						Producto p = x[5] as Producto;
						TipoIgv ti = x[6] as TipoIgv;
						UnidadMedida um = x[7] as UnidadMedida;

						lppp.ListaPrecioProducto = lpp;
						lppp.ListaPrecioProductoId = lpp.EntityId;
						lpp.ListaPrecio = lp;
						lpp.ListaPrecioId = lp.EntityId;
						lp.ImpuestoTasa = it;
						lp.ImpuestoTasaId = it.EntityId;
						lppp.ProductoPresentacionVenta = ppv;
						lppp.ProductoPresentacionVentaId = ppv.EntityId;
						ppv.Producto = p;
						ppv.ProductoId = p.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;
						ppv.UnidadMedida = um;
						ppv.UnidadMedidaId = um.EntityId;

						return lppp;
					},
					"lista_p_prod_id,lista_precio_id,impuesto_tasa_id,produ_p_vta_id,producto_id,tipo_igv_id,unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecioProductoPresentacion SingleSearch(ListaPrecioProductoPresentacion entity)
		{
			try
			{
				List<ListaPrecioProductoPresentacion> listaPrecioProductoPresentacionList = Search(entity);

				return listaPrecioProductoPresentacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecioProductoPresentacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProductoId", entity.ListaPrecioProductoId);
				parameters.AddParameter("productoPresentacionVentaId", entity.ProductoPresentacionVentaId);
				parameters.AddParameter("porcentajePrecioBase", entity.PorcentajePrecioBase);
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
		public int Update(ListaPrecioProductoPresentacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProductoPresentacionId", entity.ListaPrecioProductoPresentacionId);
				parameters.AddParameter("porcentajePrecioBase", entity.PorcentajePrecioBase);
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
		public void Delete(ListaPrecioProductoPresentacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProductoPresentacionId", entity.ListaPrecioProductoPresentacionId);
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
