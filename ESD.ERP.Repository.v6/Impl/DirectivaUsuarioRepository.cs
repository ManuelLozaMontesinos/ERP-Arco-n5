using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DirectivaUsuarioRepository : IDirectivaUsuarioRepository
	{
		private const string SP_PRE_NAME = "sp_directiva_usuario";
		public DirectivaUsuario GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(DirectivaUsuario)
					},
					x =>
					{
						DirectivaUsuario du = x[0] as DirectivaUsuario;

						return du;
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
		public List<DirectivaUsuario> Search(DirectivaUsuario entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			try
			{
				parameters.AddParameter("directivaReglaId", entity.DirectivaReglaId);
				parameters.AddParameter("directivaRegla", entity?.DirectivaRegla?.Codigo);
				parameters.AddParameter("usuarioId", entity.UsuarioId);

				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(DirectivaUsuario)
					},
					x =>
					{
						DirectivaUsuario du = x[0] as DirectivaUsuario;

						return du;
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
		public DirectivaUsuario SingleSearch(DirectivaUsuario entity)
		{
			try
			{
				List<DirectivaUsuario> directivaUsuarioList = Search(entity);

				return directivaUsuarioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(DirectivaUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaReglaId", entity.DirectivaReglaId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("valor", entity.Valor);
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
		public int Update(DirectivaUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaUsuarioId", entity.DirectivaUsuarioId);
				parameters.AddParameter("directivaReglaId", entity.DirectivaReglaId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("valor", entity.Valor);
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
		public void Delete(DirectivaUsuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaUsuarioId", entity.DirectivaUsuarioId);

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
