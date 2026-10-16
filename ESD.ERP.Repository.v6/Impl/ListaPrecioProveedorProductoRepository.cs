using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioProveedorProductoRepository : IListaPrecioProveedorProductoRepository
	{
		private const string SP_PRE_NAME = "sp_lista_p_p_prod";
		public ListaPrecioProveedorProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecioProveedorProducto)
					},
					x =>
					{
						ListaPrecioProveedorProducto lppp = x[0] as ListaPrecioProveedorProducto;

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
		public List<ListaPrecioProveedorProducto> Search(ListaPrecioProveedorProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("listaPrecioProveedorId", entity.ListaPrecioProveedorId);
			parameters.AddParameter("containsDescripcion", entity.ListaPrecioProducto?.Producto?.Descripcion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecioProveedorProducto),
						typeof(ListaPrecioProducto),
						typeof(Producto)
					},
					x =>
					{
						ListaPrecioProveedorProducto lppp = x[0] as ListaPrecioProveedorProducto;
						ListaPrecioProducto lpp = x[1] as ListaPrecioProducto;
						Producto p = x[2] as Producto;

						lpp.Producto = p;
						lpp.ProductoId = p.EntityId;
						lppp.ListaPrecioProducto = lpp;
						lppp.ListaPrecioProductoId = lpp.ListaPrecioProductoId;

						return lppp;
					},
					"lista_p_prod_id,producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecioProveedorProducto SingleSearch(ListaPrecioProveedorProducto entity)
		{
			try
			{
				List<ListaPrecioProveedorProducto> listaPrecioProveedorProductoList = Search(entity);

				return listaPrecioProveedorProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecioProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProveedorId", entity.ListaPrecioProveedorId);
				parameters.AddParameter("listaPrecioProductoId", entity.ListaPrecioProductoId);
				parameters.AddParameter("porcentajePrecioBase", entity.PorcentajePrecioBase);
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
		public int Update(ListaPrecioProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProveedorProductoId", entity.ListaPrecioProveedorProductoId);
				parameters.AddParameter("porcentajePrecioBase", entity.PorcentajePrecioBase);
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
		public void Delete(ListaPrecioProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioProveedorProductoId", entity.ListaPrecioProveedorProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarSaldoAlmacen(ListaPrecioProveedorProducto entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("listaPrecioId", entity.ListaPrecioProducto.ListaPrecioId);
			parameters.AddParameter("listaPrecioProveedorId", entity.ListaPrecioProveedorId);
			parameters.AddParameter("mesContableId", entity.ListaPrecioProducto.MesContableId);
			parameters.AddParameter("periodoId", entity.ListaPrecioProducto.PeriodoId);
			parameters.AddParameter("producto", entity.ListaPrecioProducto.Producto.Codigo);
			parameters.AddParameter("descripcion", entity.ListaPrecioProducto.Producto.Descripcion);
			parameters.AddParameter("codigoReferencial", entity.ListaPrecioProducto.Producto.CodigoReferencial);
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
