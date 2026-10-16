using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class GrupoProveedorItemRepository : IGrupoProveedorItemRepository
	{
		private const string SP_PRE_NAME = "sp_grupo_p_item";
		public GrupoProveedorItem GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(GrupoProveedorItem)
					},
					x =>
					{
						GrupoProveedorItem gpi = x[0] as GrupoProveedorItem;

						return gpi;
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
		public List<GrupoProveedorItem> Search(GrupoProveedorItem entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
			parameters.AddParameter("ignorarIncrementoFraccion", entity?.GrupoProveedor?.IgnorarIncrementoFraccion);
			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(GrupoProveedorItem),
						typeof(GrupoProveedor),
						typeof(Proveedor)
					},
					x =>
					{
						GrupoProveedorItem gpi = x[0] as GrupoProveedorItem;
						GrupoProveedor gp = x[1] as GrupoProveedor;
						Proveedor p = x[2] as Proveedor;

						gpi.GrupoProveedor = gp;
						gpi.GrupoProveedorId = gp.EntityId;
						gpi.Proveedor = p;
						gpi.ProveedorId = p.EntityId;

						return gpi;
					},
					"grupo_prove_id,proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public GrupoProveedorItem SingleSearch(GrupoProveedorItem entity)
		{
			try
			{
				List<GrupoProveedorItem> grupoProveedorItemList = Search(entity);

				return grupoProveedorItemList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(GrupoProveedorItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
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
		public int Update(GrupoProveedorItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProveedorItemId", entity.GrupoProveedorItemId);
				parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
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
		public void Delete(GrupoProveedorItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProveedorItemId", entity.GrupoProveedorItemId);
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
