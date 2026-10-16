using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenProduccionSubProcesoRepository : IOrdenProduccionSubProcesoRepository
	{
		private const string SP_PRE_NAME = "sp_orden_p_s_p";
		public OrdenProduccionSubProceso GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenProduccionSubProceso)
					},
					x =>
					{
						OrdenProduccionSubProceso opp = x[0] as OrdenProduccionSubProceso;

						return opp;
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
		public List<OrdenProduccionSubProceso> Search(OrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity?.OrdenProduccion?.PeriodoEmpresaSedeId);
			parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
			parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
			parameters.AddParameter("procesoProduccionId", entity?.SubProcesoProduccion?.ProcesoProduccionId);
			parameters.AddParameter("usuarioId", entity?.Personal?.Proveedor?.UsuarioId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(OrdenProduccionSubProceso),
						typeof(OrdenProduccion),
						typeof(Cotizacion),
						typeof(Proveedor),
						typeof(SubProcesoProduccion),
						typeof(ProcesoProduccion),
						typeof(Personal),
						typeof(Proveedor),
						typeof(EstadoOrdenProduccionSubProceso),
						typeof(Maquina),
						typeof(Maquina),
						typeof(Almacen)
					},
					x =>
					{
						OrdenProduccionSubProceso opsp = x[0] as OrdenProduccionSubProceso;
						OrdenProduccion op = x[1] as OrdenProduccion;
						Cotizacion c = x[2] as Cotizacion;
						Proveedor prv = x[3] as Proveedor;
						SubProcesoProduccion spp = x[4] as SubProcesoProduccion;
						ProcesoProduccion pp = x[5] as ProcesoProduccion;
						Personal p = x[6] as Personal;
						Proveedor prv2 = x[7] as Proveedor;
						EstadoOrdenProduccionSubProceso eopsp = x[8] as EstadoOrdenProduccionSubProceso;
						Maquina m = x[9] as Maquina;
						Maquina m2 = x[10] as Maquina;
						Almacen al = x[11] as Almacen;

						opsp.OrdenProduccion = op;
						opsp.OrdenProduccionId = op.EntityId;
						op.Cotizacion = c;
						op.CotizacionId = c?.EntityId;
						op.Proveedor = prv;
						op.ProveedorId = prv.EntityId;
						opsp.SubProcesoProduccion = spp;
						opsp.SubProcesoProduccionId = spp.EntityId;
						spp.ProcesoProduccion = pp;
						spp.ProcesoProduccionId = pp.EntityId;
						opsp.Personal = p;
						opsp.PersonalId = p?.EntityId;
						if (!Equals(p, null))
						{
							p.Proveedor = prv2;
							p.ProveedorId = prv2.EntityId;
						}
						opsp.EstadoOrdenProduccionSubProceso = eopsp;
						opsp.EstadoOrdenProduccionSubProcesoId = eopsp.EntityId;
                        if (!Equals(m, null))
                        {
                            opsp.Maquina = m;
                            opsp.MaquinaId = m.EntityId;
                        }
                        if (!Equals(m2, null))
                        {
                            opsp.Maquina = m2;
                            opsp.MaquinaRealId = m2.EntityId;
                        }
                        if (!Equals(al, null))
                        {
                            opsp.Almacen = al;
                            opsp.AlmacenId = al.EntityId;
                        }

                        return opsp;
					},
                    "orden_pro_id,cotiza_id,proveedor_id,sub_proce_p_id,proce_prod_id,personal_id,proveedor_id,estado_o_p_s_p_id,maquina_id,maquina_id,almacen_id"
                );
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public OrdenProduccionSubProceso SingleSearch(OrdenProduccionSubProceso entity)
		{
			try
			{
				List<OrdenProduccionSubProceso> ordenProduccionSubProcesoList = Search(entity);

				return ordenProduccionSubProcesoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("orden", entity.Orden);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("subProcesoProduccionAnteriorId", entity.SubProcesoProduccionAnteriorId);
				parameters.AddParameter("subProcesoProduccionAnteriorConcluido", entity.SubProcesoProduccionAnteriorConcluido);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoOrdenProduccionSubProcesoId", entity.EstadoOrdenProduccionSubProcesoId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
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
		public int Update(OrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionSubProcesoId", entity.OrdenProduccionSubProcesoId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("fechaIni", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("maquinaRealId", entity.MaquinaRealId);
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
		public void Delete(OrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionSubProcesoId", entity.OrdenProduccionSubProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarInicio(OrdenProduccionSubProceso entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionSubProcesoId", entity.OrdenProduccionSubProcesoId);
				parameters.AddParameter("estadoOrdenProduccionSubProcesoId", entity.EstadoOrdenProduccionSubProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarInicio"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarFin(OrdenProduccionSubProceso entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionSubProcesoId", entity.OrdenProduccionSubProcesoId);
				parameters.AddParameter("estadoOrdenProduccionSubProcesoId", entity.EstadoOrdenProduccionSubProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarFin"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
	  }

	  public void Maquina(OrdenProduccionSubProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionId", entity.OrdenProduccionSubProcesoId);
                parameters.AddParameter("maquinaId", entity.MaquinaId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_Maquina"), parameters);
            }
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
    }
}
