using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MotivoBajaDependienteRepository : IMotivoBajaDependienteRepository
	{
		private const string SP_PRE_NAME = "sp_motivo_b_depen";
		public MotivoBajaDependiente GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MotivoBajaDependiente)
					},
					x =>
					{
						MotivoBajaDependiente mbd = x[0] as MotivoBajaDependiente;

						return mbd;
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
		public List<MotivoBajaDependiente> Search(MotivoBajaDependiente entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MotivoBajaDependiente)
					},
					x =>
					{
						MotivoBajaDependiente mbd = x[0] as MotivoBajaDependiente;

						return mbd;
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
		public MotivoBajaDependiente SingleSearch(MotivoBajaDependiente entity)
		{
			try
			{
				List<MotivoBajaDependiente> motivoBajaDependienteList = Search(entity);

				return motivoBajaDependienteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MotivoBajaDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoBajaDependiente", entity.Codigo);
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
		public int Update(MotivoBajaDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoBajaDependienteId", entity.MotivoBajaDependienteId);
				parameters.AddParameter("motivoBajaDependiente", entity.Codigo);
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
		public void Delete(MotivoBajaDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoBajaDependienteId", entity.MotivoBajaDependienteId);
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
