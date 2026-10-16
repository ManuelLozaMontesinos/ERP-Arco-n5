using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RemuneracionMinimaVitalRepository : IRemuneracionMinimaVitalRepository
	{
		private const string SP_PRE_NAME = "sp_rem_min_vit";
		public RemuneracionMinimaVital GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RemuneracionMinimaVital)
					},
					x =>
					{
						RemuneracionMinimaVital rmv = x[0] as RemuneracionMinimaVital;

						return rmv;
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
		public List<RemuneracionMinimaVital> Search(RemuneracionMinimaVital entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("fechaBajaNull", entity.FechaBajaNull);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RemuneracionMinimaVital)
					},
					x =>
					{
						RemuneracionMinimaVital rmv = x[0] as RemuneracionMinimaVital;

						return rmv;
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
		public RemuneracionMinimaVital SingleSearch(RemuneracionMinimaVital entity)
		{
			try
			{
				List<RemuneracionMinimaVital> remuneracionMinimaVitalList = Search(entity);

				return remuneracionMinimaVitalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RemuneracionMinimaVital entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public int Update(RemuneracionMinimaVital entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("remuneracionMinimaVitalId", entity.RemuneracionMinimaVitalId);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public void Delete(RemuneracionMinimaVital entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("remuneracionMinimaVitalId", entity.RemuneracionMinimaVitalId);
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
