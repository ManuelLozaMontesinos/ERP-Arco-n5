using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DiaRepository : IDiaRepository
	{
		private const string SP_PRE_NAME = "sp_dia";
		public Dia GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Dia)
					},
					x =>
					{
						Dia d = x[0] as Dia;

						return d;
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
		public List<Dia> Search(Dia entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Dia)
					},
					x =>
					{
						Dia d = x[0] as Dia;

						return d;
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
		public Dia SingleSearch(Dia entity)
		{
			try
			{
				List<Dia> diaList = Search(entity);

				return diaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Dia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("dia", entity.Codigo);
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
		public int Update(Dia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("diaId", entity.DiaId);
				parameters.AddParameter("dia", entity.Codigo);
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
		public void Delete(Dia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("diaId", entity.DiaId);
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
