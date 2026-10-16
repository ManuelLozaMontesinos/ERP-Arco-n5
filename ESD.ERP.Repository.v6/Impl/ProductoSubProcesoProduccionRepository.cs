using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProductoSubProcesoProduccionRepository : IProductoSubProcesoProduccionRepository
	{
		private const string SP_PRE_NAME = "sp_produ_s_p_p";
		public ProductoSubProcesoProduccion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProductoSubProcesoProduccion)
					},
					x =>
					{
						ProductoSubProcesoProduccion pspp = x[0] as ProductoSubProcesoProduccion;

						return pspp;
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
		public List<ProductoSubProcesoProduccion> Search(ProductoSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("productoIdList", entity.ProductoIdList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProductoSubProcesoProduccion),
						typeof(SubProcesoProduccion),
						typeof(ProcesoProduccion)
					},
					x =>
					{
						ProductoSubProcesoProduccion pspp = x[0] as ProductoSubProcesoProduccion;
						SubProcesoProduccion spp = x[1] as SubProcesoProduccion;
						ProcesoProduccion pp = x[2] as ProcesoProduccion;

						pspp.SubProcesoProduccion = spp;
						pspp.SubProcesoProduccionId = spp.EntityId;
						spp.ProcesoProduccion = pp;
						spp.ProcesoProduccionId = pp.EntityId;

						return pspp;
					},
					"sub_proce_p_id,proce_prod_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ProductoSubProcesoProduccion SingleSearch(ProductoSubProcesoProduccion entity)
		{
			try
			{
				List<ProductoSubProcesoProduccion> productoSubProcesoProduccionList = Search(entity);

				return productoSubProcesoProduccionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProductoSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
				parameters.AddParameter("orden", entity.Orden);
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
		public int Update(ProductoSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoSubProcesoProduccionId", entity.ProductoSubProcesoProduccionId);
				parameters.AddParameter("orden", entity.Orden);
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
		public void Delete(ProductoSubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoSubProcesoProduccionId", entity.ProductoSubProcesoProduccionId);
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
