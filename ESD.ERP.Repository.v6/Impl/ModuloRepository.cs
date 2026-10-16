using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ModuloRepository : IModuloRepository
	{
		private const string SP_PRE_NAME = "sp_modulo";
		public Modulo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Modulo)
					},
					x =>
					{
						Modulo m = x[0] as Modulo;

						return m;
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
		public List<Modulo> Search(Modulo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Modulo)
					},
					x =>
					{
						Modulo m = x[0] as Modulo;

						return m;
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
		public Modulo SingleSearch(Modulo entity)
		{
			try
			{
				List<Modulo> moduloList = Search(entity);

				return moduloList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Modulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("modulo", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(Modulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("moduloId", entity.ModuloId);
				parameters.AddParameter("modulo", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(Modulo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("moduloId", entity.ModuloId);

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
