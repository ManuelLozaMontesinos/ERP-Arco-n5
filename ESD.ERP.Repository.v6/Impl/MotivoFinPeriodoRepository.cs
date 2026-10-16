using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MotivoFinPeriodoRepository : IMotivoFinPeriodoRepository
	{
		private const string SP_PRE_NAME = "sp_motivo_fin_peri";
		public MotivoFinPeriodo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MotivoFinPeriodo)
					},
					x =>
					{
						MotivoFinPeriodo mfp = x[0] as MotivoFinPeriodo;

						return mfp;
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
		public List<MotivoFinPeriodo> Search(MotivoFinPeriodo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MotivoFinPeriodo)
					},
					x =>
					{
						MotivoFinPeriodo mfp = x[0] as MotivoFinPeriodo;

						return mfp;
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
		public MotivoFinPeriodo SingleSearch(MotivoFinPeriodo entity)
		{
			try
			{
				List<MotivoFinPeriodo> motivoFinPeriodoList = Search(entity);

				return motivoFinPeriodoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MotivoFinPeriodo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoFinPeriodo", entity.Codigo);
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
		public int Update(MotivoFinPeriodo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("motivoFinPeriodo", entity.Codigo);
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
		public void Delete(MotivoFinPeriodo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
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
