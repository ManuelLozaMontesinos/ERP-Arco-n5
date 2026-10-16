using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RolUsuarioRepository : IRolUsuarioRepository
	{
		private const string SP_PRE_NAME = "sp_rol_usuario";
		public RolUsuario GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RolUsuario)
					},
					x =>
					{
						RolUsuario ru = x[0] as RolUsuario;

						return ru;
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
		public List<RolUsuario> Search(RolUsuario entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("rolId", entity.RolId);
			parameters.AddParameter("rolList", entity.RolList);
			parameters.AddParameter("usuarioId", entity.UsuarioId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RolUsuario),
						typeof(Rol),
						typeof(Usuario)
					},
					x =>
					{
						RolUsuario ru = x[0] as RolUsuario;
						Rol r = x[1] as Rol;
						Usuario u = x[2] as Usuario;

						ru.Rol = r;
						ru.RolId = r.EntityId;
						ru.Usuario = u;
						ru.UsuarioId = u.EntityId;

						return ru;
					},
					"rol_id,usuario_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public RolUsuario SingleSearch(RolUsuario entity)
		{
			try
			{
				List<RolUsuario> rolUsuarioList = Search(entity);

				return rolUsuarioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RolUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("rolId", entity.RolId);
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
		public int Update(RolUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolUsuarioId", entity.RolUsuarioId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("rolId", entity.RolId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(RolUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("rolUsuarioId", entity.RolUsuarioId);

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
