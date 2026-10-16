using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CajaPersonalRepository : ICajaPersonalRepository
	{
		private const string SP_PRE_NAME = "sp_caja_perso";
		public CajaPersonal GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CajaPersonal),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						CajaPersonal cp = x[0] as CajaPersonal;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;

						cp.Personal = p;
						cp.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;

						return cp;
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
		public List<CajaPersonal> Search(CajaPersonal entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaSedeId", entity?.Caja?.EmpresaSedeId);
			parameters.AddParameter("cajaId", entity.CajaId);
			parameters.AddParameter("usuarioId", entity?.Personal?.Proveedor?.UsuarioId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CajaPersonal),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						CajaPersonal cp = x[0] as CajaPersonal;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;

						cp.Personal = p;
						cp.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;

						return cp;
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
		public CajaPersonal SingleSearch(CajaPersonal entity)
		{
			try
			{
				List<CajaPersonal> cajaList = Search(entity);

				return cajaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CajaPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cajaId", entity.CajaId);
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
		public int Update(CajaPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cajaPersonalId", entity.CajaPersonalId);
				parameters.AddParameter("personalId", entity.PersonalId);
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
		public void Delete(CajaPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cajaPersonalId", entity.CajaPersonalId);
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
