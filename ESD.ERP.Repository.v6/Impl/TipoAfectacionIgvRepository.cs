using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoAfectacionIgvRepository : ITipoAfectacionIgvRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_afecta_igv";
		public TipoAfectacionIgv GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv)
					},
					x =>
					{
						TipoAfectacionIgv tai = x[0] as TipoAfectacionIgv;
						TipoIgv ti = x[1] as TipoIgv;

						tai.TipoIgv = ti;
						tai.TipoIgvId = ti.EntityId;

						return tai;
					},
					"tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<TipoAfectacionIgv> Search(TipoAfectacionIgv entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoIgvId", entity.TipoIgvId);
			parameters.AddParameter("tipoIgv", entity.TipoIgv?.Codigo);
			parameters.AddParameter("tipoAfectacionIgv", entity.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv)
					},
					x =>
					{
						TipoAfectacionIgv tai = x[0] as TipoAfectacionIgv;
						TipoIgv ti = x[1] as TipoIgv;

						tai.TipoIgv = ti;
						tai.TipoIgvId = ti.EntityId;

						return tai;
					},
					"tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoAfectacionIgv SingleSearch(TipoAfectacionIgv entity)
		{
			try
			{
				List<TipoAfectacionIgv> tipoAfectacionIgvList = Search(entity);

				return tipoAfectacionIgvList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoAfectacionIgv entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoIgvId", entity.TipoIgvId);
				parameters.AddParameter("tipoAfectacionIgv", entity.Codigo);
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
		public int Update(TipoAfectacionIgv entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIgvId", entity.TipoIgvId);
				parameters.AddParameter("tipoAfectacionIgv", entity.Codigo);
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
		public void Delete(TipoAfectacionIgv entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
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
