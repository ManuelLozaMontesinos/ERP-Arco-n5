using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class SubProcesoProduccionPersonalRepository : ISubProcesoProduccionPersonalRepository
	{
		private const string SP_PRE_NAME = "sp_sub_p_p_perso";
		public SubProcesoProduccionPersonal GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(SubProcesoProduccionPersonal)
					},
					x =>
					{
						SubProcesoProduccionPersonal sppp = x[0] as SubProcesoProduccionPersonal;

						return sppp;
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
		public List<SubProcesoProduccionPersonal> Search(SubProcesoProduccionPersonal entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
			parameters.AddParameter("containsRazonSocial", entity?.Personal?.Proveedor?.RazonSocial);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(SubProcesoProduccionPersonal),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						SubProcesoProduccionPersonal sppp = x[0] as SubProcesoProduccionPersonal;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;

						sppp.Personal = p;
						sppp.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;

						return sppp;
					},
					"personal_id,proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public SubProcesoProduccionPersonal SingleSearch(SubProcesoProduccionPersonal entity)
		{
			try
			{
				List<SubProcesoProduccionPersonal> subProcesoProduccionPersonalList = Search(entity);

				return subProcesoProduccionPersonalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(SubProcesoProduccionPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
				parameters.AddParameter("personalId", entity.PersonalId);
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
		public int Update(SubProcesoProduccionPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionPersonalId", entity.SubProcesoProduccionPersonalId);
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
		public void Delete(SubProcesoProduccionPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionPersonalId", entity.SubProcesoProduccionPersonalId);
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
