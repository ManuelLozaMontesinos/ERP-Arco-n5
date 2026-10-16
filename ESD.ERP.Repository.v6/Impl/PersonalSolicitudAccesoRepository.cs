using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalSolicitudAccesoRepository : IPersonalSolicitudAccesoRepository
	{
		private const string SP_PRE_NAME = "sp_perso_sol_acc";
		public PersonalSolicitudAcceso GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalSolicitudAcceso),
						typeof(Personal),
						typeof(Proveedor),
						typeof(Personal),
						typeof(Proveedor),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						PersonalSolicitudAcceso psa = x[0] as PersonalSolicitudAcceso;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						Personal pa = x[3] as Personal;
						Proveedor prv2 = x[4] as Proveedor;
						EstadoAprobacion ea = x[5] as EstadoAprobacion;

						psa.Personal = p;
						psa.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;

						if (!Equals(pa, null))
						{
							psa.Personal = pa;
							psa.PersonalId = pa.EntityId;
							pa.Proveedor = prv2;
							pa.ProveedorId = prv2.EntityId;
						}

						psa.EstadoAprobacion = ea;
						psa.EstadoAprobacionId = ea.EntityId;

						return psa;
					},
					"personal_id,proveedor_id,personal_id,proveedor_id,estado_aproba_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PersonalSolicitudAcceso> Search(PersonalSolicitudAcceso entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaDesde);
			parameters.AddParameter("usuarioId", entity?.Personal?.Proveedor?.UsuarioId);
			parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalSolicitudAcceso),
						typeof(Personal),
						typeof(Proveedor),
						typeof(Personal),
						typeof(Proveedor),
						typeof(EstadoAprobacion)
					},
					x =>
					{
						PersonalSolicitudAcceso psa = x[0] as PersonalSolicitudAcceso;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						Personal pa = x[3] as Personal;
						Proveedor prv2 = x[4] as Proveedor;
						EstadoAprobacion ea = x[5] as EstadoAprobacion;

						psa.Personal = p;
						psa.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;

						if (!Equals(pa, null))
						{
							psa.PersonalAutoriza = pa;
							psa.PersonalAutorizaId = pa.EntityId;
							pa.Proveedor = prv2;
							pa.ProveedorId = prv2.EntityId;
						}

						psa.EstadoAprobacion = ea;
						psa.EstadoAprobacionId = ea.EntityId;

						return psa;
					},
					"personal_id,proveedor_id,personal_id,proveedor_id,estado_aproba_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalSolicitudAcceso SingleSearch(PersonalSolicitudAcceso entity)
		{
			try
			{
				List<PersonalSolicitudAcceso> personalSolicitudAccesoList = Search(entity);

				return personalSolicitudAccesoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalSolicitudAcceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaRegistro", entity.FechaRegistro);
				parameters.AddParameter("horaSolicitada", entity.HoraSolicitada);
				parameters.AddParameter("horaAutorizada", entity.HoraAutorizada);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
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
		public int Update(PersonalSolicitudAcceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalSolicitudAccesoId", entity.PersonalSolicitudAccesoId);
				parameters.AddParameter("horaSolicitada", entity.HoraSolicitada);
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
		public void Delete(PersonalSolicitudAcceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalSolicitudAccesoId", entity.PersonalSolicitudAccesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoAprobacion(PersonalSolicitudAcceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalSolicitudAccesoId", entity.PersonalSolicitudAccesoId);
				parameters.AddParameter("personalAutorizaId", entity.PersonalAutorizaId);
				parameters.AddParameter("horaAutorizada", entity.HoraAutorizada);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoAprobacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
