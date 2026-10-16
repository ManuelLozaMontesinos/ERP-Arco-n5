using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalVacacionRepository : IPersonalVacacionRepository
	{
		private const string SP_PRE_NAME = "sp_personal_vaca";
		public PersonalVacacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalVacacion)
					},
					x =>
					{
						PersonalVacacion pv = x[0] as PersonalVacacion;

						return pv;
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
		public List<PersonalVacacion> Search(PersonalVacacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalVacacion),
						typeof(Personal),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(CentroCosto),
						typeof(EstadoVacacion)
					},
					x =>
					{
						PersonalVacacion pv = x[0] as PersonalVacacion;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						TipoDocumentoIdentidad tdi = x[3] as TipoDocumentoIdentidad;
						CentroCosto cc = x[4] as CentroCosto;
						EstadoVacacion ev = x[5] as EstadoVacacion;

						pv.PersonalId = p.EntityId;
						pv.Personal = p;
						p.ProveedorId = prv.EntityId;
						p.Proveedor = prv;
						prv.TipoDocumentoIdentidad = tdi;
						prv.TipoDocumentoIdentidadId = tdi.EntityId;
						p.CentroCostoId = cc?.EntityId;
						p.CentroCosto = cc;
						pv.EstadoVacacionId = ev.EntityId;
						pv.EstadoVacacion = ev;
						
						return pv;
					},
					"personal_id,proveedor_id,tipo_doc_iden_id,centro_costo_id,estado_vaca_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalVacacion SingleSearch(PersonalVacacion entity)
		{
			try
			{
				List<PersonalVacacion> personalVacacionList = Search(entity);

				return personalVacacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalVacacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("diasACuenta", entity.DiasACuenta);
				parameters.AddParameter("estadoVacacionId", entity.EstadoVacacionId);
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
		public int Update(PersonalVacacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalVacacionId", entity.PersonalVacacionId);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("estadoVacacionId", entity.EstadoVacacionId);
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
		public void Delete(PersonalVacacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalVacacionId", entity.PersonalVacacionId);
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
