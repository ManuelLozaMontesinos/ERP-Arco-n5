using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EPSRepository : IEPSRepository
	{
		private const string SP_PRE_NAME = "sp_eps";
		public EPS GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EPS)
					},
					x =>
					{
						EPS e = x[0] as EPS;

						return e;
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
		public List<EPS> Search(EPS entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EPS)
					},
					x =>
					{
						EPS e = x[0] as EPS;

						return e;
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
		public EPS SingleSearch(EPS entity)
		{
			try
			{
				List<EPS> epsList = Search(entity);

				return epsList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EPS entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("eps", entity.Codigo);
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
		public int Update(EPS entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("epsId", entity.EPSId);
				parameters.AddParameter("eps", entity.Codigo);
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
		public void Delete(EPS entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("epsId", entity.EPSId);
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
