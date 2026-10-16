using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class VinculoFamiliarRepository : IVinculoFamiliarRepository
	{
		private const string SP_PRE_NAME = "sp_vincu_fami";
		public VinculoFamiliar GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(VinculoFamiliar)
					},
					x =>
					{
						VinculoFamiliar vf = x[0] as VinculoFamiliar;

						return vf;
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
		public List<VinculoFamiliar> Search(VinculoFamiliar entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(VinculoFamiliar)
					},
					x =>
					{
						VinculoFamiliar vf = x[0] as VinculoFamiliar;

						return vf;
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
		public VinculoFamiliar SingleSearch(VinculoFamiliar entity)
		{
			try
			{
				List<VinculoFamiliar> vinculoFamiliarList = Search(entity);

				return vinculoFamiliarList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(VinculoFamiliar entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vinculoFamiliar", entity.Codigo);
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
		public int Update(VinculoFamiliar entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vinculoFamiliarId", entity.VinculoFamiliarId);
				parameters.AddParameter("vinculoFamiliar", entity.Codigo);
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
		public void Delete(VinculoFamiliar entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vinculoFamiliarId", entity.VinculoFamiliarId);
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
