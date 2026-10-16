using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DataImportRepository : IDataImportRepository
	{
		private const string SP_PRE_NAME = "sp_data_import";
		public DataImport GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(DataImport)
					},
					x =>
					{
						DataImport di = x[0] as DataImport;

						return di;
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
		public List<DataImport> Search(DataImport entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(DataImport)
					},
					x =>
					{
						DataImport di = x[0] as DataImport;

						return di;
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
		public DataImport SingleSearch(DataImport entity)
		{
			try
			{
				List<DataImport> dataImportList = Search(entity);

				return dataImportList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(DataImport entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImport", entity.Codigo);
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
		public int Update(DataImport entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImportId", entity.DataImportId);
				parameters.AddParameter("dataImport", entity.Codigo);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(DataImport entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImportId", entity.DataImportId);

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
