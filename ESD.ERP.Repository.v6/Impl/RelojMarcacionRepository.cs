using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RelojMarcacionRepository : IRelojMarcacionRepository
	{
		private const string SP_PRE_NAME = "sp_reloj_marca";
		public RelojMarcacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RelojMarcacion)
					},
					x =>
					{
						RelojMarcacion rm = x[0] as RelojMarcacion;

						return rm;
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
		public List<RelojMarcacion> Search(RelojMarcacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("fecha", entity.Fecha);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RelojMarcacion)
					},
					x =>
					{
						RelojMarcacion rm = x[0] as RelojMarcacion;

						return rm;
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
		public RelojMarcacion SingleSearch(RelojMarcacion entity)
		{
			try
			{
				List<RelojMarcacion> relojMarcacionList = Search(entity);

				return relojMarcacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RelojMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("relojId", entity.RelojId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("fechaFinal", entity.FechaFinal);
				parameters.AddParameter("observacion", entity.Observacion);
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
		public int Update(RelojMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("relojMarcacionId", entity.RelojMarcacionId);
				parameters.AddParameter("fechaFinal", entity.FechaFinal);
				parameters.AddParameter("observacion", entity.Observacion);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(RelojMarcacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("relojMarcacionId", entity.RelojMarcacionId);

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
