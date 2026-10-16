using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProductoDetalleRepository : IProductoDetalleRepository
	{
		private const string SP_PRE_NAME = "sp_produ_detalle";
		public ProductoDetalle GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProductoDetalle)
					},
					x =>
					{
						ProductoDetalle pd = x[0] as ProductoDetalle;

						return pd;
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
		public List<ProductoDetalle> Search(ProductoDetalle entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProductoDetalle)
					},
					x =>
					{
						ProductoDetalle pd = x[0] as ProductoDetalle;

						return pd;
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
		public ProductoDetalle SingleSearch(ProductoDetalle entity)
		{
			try
			{
				List<ProductoDetalle> productoDetalleList = Search(entity);

				return productoDetalleList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProductoDetalle entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(ProductoDetalle entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoDetalleId", entity.ProductoDetalleId);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public void Delete(ProductoDetalle entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoDetalleId", entity.ProductoDetalleId);
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
