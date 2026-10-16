using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DataImportItemRepository : IDataImportItemRepository
	{
		private const string SP_PRE_NAME = "sp_data_imp_item";
		public DataImportItem GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(DataImportItem),
						typeof(DataImport)
					},
					x =>
					{
						DataImportItem dii = x[0] as DataImportItem;
						DataImport di = x[1] as DataImport;

						dii.DataImport = di;
						dii.DataImportId = di.EntityId;

						return dii;
					},
					"data_import_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<DataImportItem> Search(DataImportItem entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("dataImport", entity.DataImport.Codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(DataImportItem),
						typeof(DataImport)
					},
					x =>
					{
						DataImportItem dii = x[0] as DataImportItem;
						DataImport di = x[1] as DataImport;

						dii.DataImport = di;
						dii.DataImportId = di.EntityId;

						return dii;
					},
					"data_import_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public DataImportItem SingleSearch(DataImportItem entity)
		{
			try
			{
				List<DataImportItem> dataImportItemList = Search(entity);

				return dataImportItemList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(DataImportItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImportId", entity.DataImportId);
				parameters.AddParameter("columna", entity.Columna);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("orden", entity.Orden);
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
		public int Update(DataImportItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImportItemId", entity.DataImportItemId);
				parameters.AddParameter("dataImportId", entity.DataImportId);
				parameters.AddParameter("columna", entity.Columna);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("orden", entity.Orden);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(DataImportItem entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dataImportItemId", entity.DataImportItemId);

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
