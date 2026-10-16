using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class GrupoProveedorRepository : IGrupoProveedorRepository
	{
		private const string SP_PRE_NAME = "sp_grupo_prove";
		public GrupoProveedor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(GrupoProveedor)
					},
					x =>
					{
						GrupoProveedor gp = x[0] as GrupoProveedor;

						return gp;
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
		public List<GrupoProveedor> Search(GrupoProveedor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(GrupoProveedor)
					},
					x =>
					{
						GrupoProveedor gp = x[0] as GrupoProveedor;

						return gp;
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
		public GrupoProveedor SingleSearch(GrupoProveedor entity)
		{
			try
			{
				List<GrupoProveedor> grupoProveedorList = Search(entity);

				return grupoProveedorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(GrupoProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("ignorarIncrementoFraccion", entity.IgnorarIncrementoFraccion);
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
		public int Update(GrupoProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("ignorarIncrementoFraccion", entity.IgnorarIncrementoFraccion);
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
		public void Delete(GrupoProveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("grupoProveedorId", entity.GrupoProveedorId);
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
