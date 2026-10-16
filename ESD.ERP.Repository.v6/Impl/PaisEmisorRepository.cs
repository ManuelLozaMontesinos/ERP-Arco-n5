using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PaisEmisorRepository : IPaisEmisorRepository
	{
		private const string SP_PRE_NAME = "sp_pais_emisor";
		public PaisEmisor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PaisEmisor)
					},
					x =>
					{
						PaisEmisor pe = x[0] as PaisEmisor;

						return pe;
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
		public List<PaisEmisor> Search(PaisEmisor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PaisEmisor)
					},
					x =>
					{
						PaisEmisor pe = x[0] as PaisEmisor;

						return pe;
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
		public PaisEmisor SingleSearch(PaisEmisor entity)
		{
			try
			{
				List<PaisEmisor> paisEmisorList = Search(entity);

				return paisEmisorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PaisEmisor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("paisEmisor", entity.Codigo);
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
		public int Update(PaisEmisor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
				parameters.AddParameter("paisEmisor", entity.Codigo);
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
		public void Delete(PaisEmisor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
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
