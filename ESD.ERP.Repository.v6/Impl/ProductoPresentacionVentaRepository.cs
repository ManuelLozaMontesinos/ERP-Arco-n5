using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProductoPresentacionVentaRepository : IProductoPresentacionVentaRepository
	{
		private const string SP_PRE_NAME = "sp_produ_p_vta";
		public ProductoPresentacionVenta GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProductoPresentacionVenta)
					},
					x =>
					{
						ProductoPresentacionVenta ppv = x[0] as ProductoPresentacionVenta;

						return ppv;
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
		public List<ProductoPresentacionVenta> Search(ProductoPresentacionVenta entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProductoPresentacionVenta),
						typeof(UnidadMedida)
					},
					x =>
					{
						ProductoPresentacionVenta ppv = x[0] as ProductoPresentacionVenta;
						UnidadMedida um = x[1] as UnidadMedida;

						ppv.UnidadMedidaId = um.EntityId;
						ppv.UnidadMedida = um;

						return ppv;
					},
					"unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ProductoPresentacionVenta SingleSearch(ProductoPresentacionVenta entity)
		{
			try
			{
				List<ProductoPresentacionVenta> productoPresentacionVentaList = Search(entity);

				return productoPresentacionVentaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProductoPresentacionVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("cantidadEquivalente", entity.CantidadEquivalente);
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
		public int Update(ProductoPresentacionVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoPresentacionVentaId", entity.ProductoPresentacionVentaId);
				parameters.AddParameter("cantidadEquivalente", entity.CantidadEquivalente);
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
		public void Delete(ProductoPresentacionVenta entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoPresentacionVentaId", entity.ProductoPresentacionVentaId);
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
