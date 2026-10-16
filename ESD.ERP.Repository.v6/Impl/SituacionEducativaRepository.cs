using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class SituacionEducativaRepository : ISituacionEducativaRepository
	{
		private const string SP_PRE_NAME = "sp_situa_educa";
		public SituacionEducativa GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(SituacionEducativa)
					},
					x =>
					{
						SituacionEducativa ne = x[0] as SituacionEducativa;

						return ne;
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
		public List<SituacionEducativa> Search(SituacionEducativa entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(SituacionEducativa)
					},
					x =>
					{
						SituacionEducativa ne = x[0] as SituacionEducativa;

						return ne;
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
		public SituacionEducativa SingleSearch(SituacionEducativa entity)
		{
			try
			{
				List<SituacionEducativa> situacionEducativaList = Search(entity);

				return situacionEducativaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(SituacionEducativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEducativa", entity.Codigo);
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
		public int Update(SituacionEducativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEducativaId", entity.SituacionEducativaId);
				parameters.AddParameter("situacionEducativa", entity.Codigo);
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
		public void Delete(SituacionEducativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEducativaId", entity.SituacionEducativaId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public SituacionEducativa ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(SituacionEducativa)
					},
					x =>
					{
						SituacionEducativa se = x[0] as SituacionEducativa;

						return se;
					},
					string.Empty
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
