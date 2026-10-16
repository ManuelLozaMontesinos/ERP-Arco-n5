using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class ProveedorListaPrecioProductoRepository : IProveedorListaPrecioProductoRepository
	{
		private const string SP_PRE_NAME = "sp_prove_l_p_p";
		public ProveedorListaPrecioProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProveedorListaPrecioProducto)
					},
					x =>
					{
						ProveedorListaPrecioProducto plpp = x[0] as ProveedorListaPrecioProducto;

						return plpp;
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
		public List<ProveedorListaPrecioProducto> Search(ProveedorListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("proveedorListaPrecioId", entity.ProveedorListaPrecioId);
			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("productoIdList", entity.ProductoIdList);
			parameters.AddParameter("containsDescripcion", entity.Producto?.Descripcion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProveedorListaPrecioProducto),
						typeof(Producto),
						typeof(TipoIgv)
					},
					x =>
					{
						ProveedorListaPrecioProducto plpp = x[0] as ProveedorListaPrecioProducto;
						Producto p = x[1] as Producto;
						TipoIgv ti = x[2] as TipoIgv;

						plpp.Producto = p;
						plpp.ProductoId = p.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;

						return plpp;
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
		public ProveedorListaPrecioProducto SingleSearch(ProveedorListaPrecioProducto entity)
		{
			try
			{
				List<ProveedorListaPrecioProducto> proveedorListaPrecioProductoList = Search(entity);

				return proveedorListaPrecioProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProveedorListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorListaPrecioId", entity.ProveedorListaPrecioId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("valorUnitarioAnterior", entity.ValorUnitarioAnterior);
				parameters.AddParameter("precioUnitarioAnterior", entity.PrecioUnitarioAnterior);
				parameters.AddParameter("fechaAnterior", entity.FechaAnterior);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("fechaVigente", entity.FechaVigente);
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
		public int Update(ProveedorListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorListaPrecioProductoId", entity.ProveedorListaPrecioProductoId);
				parameters.AddParameter("valorUnitarioAnterior", entity.ValorUnitarioAnterior);
				parameters.AddParameter("precioUnitarioAnterior", entity.PrecioUnitarioAnterior);
				parameters.AddParameter("fechaAnterior", entity.FechaAnterior);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("fechaVigente", entity.FechaVigente);
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
		public void Delete(ProveedorListaPrecioProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorListaPrecioProductoId", entity.ProveedorListaPrecioProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<ProveedorListaPrecioProducto> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<ProveedorListaPrecioProductoType> proveedorListaPrecioProductoTypeList = new();
				proveedorListaPrecioProductoTypeList = (from s in list
														 select new ProveedorListaPrecioProductoType
														 {
															 ProveedorListaPrecioProductoId = s.ProveedorListaPrecioProductoId,
															 ProveedorListaPrecioId = s.ProveedorListaPrecioId,
															 ProductoId = s.ProductoId,
															 ValorUnitarioAnterior = s.ValorUnitarioAnterior,
															 PrecioUnitarioAnterior = s.PrecioUnitarioAnterior,
															 FechaAnterior = s.FechaAnterior,
															 ValorUnitario = s.ValorUnitario,
															 PrecioUnitario = s.PrecioUnitario,
															 FechaVigente = s.FechaVigente,
															 Estado = s.Estado,
															 UsuarioCreacionId = s.UsuarioCreacionId,
														 }).ToList();

				parameters.AddTableParameter("proveedorListaPrecioProductoList", "dbo.ProveedorListaPrecioProductoType", proveedorListaPrecioProductoTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void EliminarPorProveedorIdProductoId(ProveedorListaPrecioProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorListaPrecio.ProveedorId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorProveedorIdProductoId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
