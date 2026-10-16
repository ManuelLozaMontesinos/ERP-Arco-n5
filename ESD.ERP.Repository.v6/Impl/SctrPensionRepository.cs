using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class SctrPensionRepository : ISctrPensionRepository
	{
		private const string SP_PRE_NAME = "sp_sctr_pension";
		public SctrPension GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(SctrPension)
					},
					x =>
					{
						SctrPension sp = x[0] as SctrPension;

						return sp;
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
		public List<SctrPension> Search(SctrPension entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(SctrPension)
					},
					x =>
					{
						SctrPension sp = x[0] as SctrPension;

						return sp;
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
		public SctrPension SingleSearch(SctrPension entity)
		{
			try
			{
				List<SctrPension> sctrPensionList = Search(entity);

				return sctrPensionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(SctrPension entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("sctrPension", entity.Codigo);
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
		public int Update(SctrPension entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("sctrPensionId", entity.SctrPensionId);
				parameters.AddParameter("sctrPension", entity.Codigo);
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
		public void Delete(SctrPension entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("sctrPensionId", entity.SctrPensionId);
				parameters.AddParameter("estado", entity.Estado);

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
