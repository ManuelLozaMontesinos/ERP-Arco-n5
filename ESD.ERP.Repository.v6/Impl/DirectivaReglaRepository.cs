using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DirectivaReglaRepository : IDirectivaReglaRepository
	{
		private const string SP_PRE_NAME = "sp_directiva_regla";
		public DirectivaRegla GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(DirectivaRegla)
					},
					x =>
					{
						DirectivaRegla dr = x[0] as DirectivaRegla;

						return dr;
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
		public List<DirectivaRegla> Search(DirectivaRegla entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(DirectivaRegla)
					},
					x =>
					{
						DirectivaRegla dr = x[0] as DirectivaRegla;

						return dr;
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
		public DirectivaRegla SingleSearch(DirectivaRegla entity)
		{
			try
			{
				List<DirectivaRegla> directivaReglaList = Search(entity);

				return directivaReglaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(DirectivaRegla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaRegla", entity.Codigo);
				parameters.AddParameter("descripcion", entity.Descripcion);
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
		public int Update(DirectivaRegla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaReglaId", entity.DirectivaReglaId);
				parameters.AddParameter("directivaRegla", entity.Codigo);
				parameters.AddParameter("descripcion", entity.Descripcion);
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
		public void Delete(DirectivaRegla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("directivaReglaId", entity.DirectivaReglaId);
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
