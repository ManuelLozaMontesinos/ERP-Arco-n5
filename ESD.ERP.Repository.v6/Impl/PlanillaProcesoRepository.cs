using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaProcesoRepository : IPlanillaProcesoRepository
	{
		private const string SP_PRE_NAME = "sp_plan_proce";
		public PlanillaProceso GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PlanillaProceso),
						typeof(Mes),
						typeof(Moneda),
						typeof(EstadoPlanillaProceso)
					},
					x =>
					{
						PlanillaProceso pp = x[0] as PlanillaProceso;
						Mes m = x[1] as Mes;
						Moneda mn = x[2] as Moneda;
						EstadoPlanillaProceso epp = x[3] as EstadoPlanillaProceso;

						pp.MesId = m.EntityId;
						pp.Mes = m;
						pp.MonedaId = mn.EntityId;
						pp.Moneda = mn;
						pp.EstadoPlanillaProcesoId = epp.EntityId;
						pp.EstadoPlanillaProceso = epp;

						return pp;
					},
					"mes_id,moneda_id,estado_plan_pro_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PlanillaProceso> Search(PlanillaProceso entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("planillaId", entity.PlanillaId);
			parameters.AddParameter("mesId", entity.MesId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PlanillaProceso),
						typeof(Planilla),
						typeof(Mes),
						typeof(Moneda),
						typeof(PeriodicidadRemunerativa),
						typeof(EstadoPlanillaProceso)
					},
					x =>
					{
						PlanillaProceso pp = x[0] as PlanillaProceso;
						Planilla p = x[1] as Planilla;
						Mes m = x[2] as Mes;
						Moneda mn = x[3] as Moneda;
						PeriodicidadRemunerativa pr = x[4] as PeriodicidadRemunerativa;
						EstadoPlanillaProceso epp = x[5] as EstadoPlanillaProceso;

						pp.PlanillaId = p.EntityId;
						pp.Planilla = p;
						pp.MesId = m.EntityId;
						pp.Mes = m;
						pp.MonedaId = mn.EntityId;
						pp.Moneda = mn;
						pp.PeriodicidadRemunerativa = pr;
						pp.PeriodicidadRemunerativaId = pr.EntityId;
						pp.EstadoPlanillaProcesoId = epp.EntityId;
						pp.EstadoPlanillaProceso = epp;

						return pp;
					},
					"planilla_id,mes_id,moneda_id,peri_remu_id,estado_plan_pro_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PlanillaProceso SingleSearch(PlanillaProceso entity)
		{
			try
			{
				List<PlanillaProceso> planillaProcesoList = Search(entity);

				return planillaProcesoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PlanillaProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("planillaId", entity.PlanillaId);
				parameters.AddParameter("procesoAnual", entity.ProcesoAnual);
				parameters.AddParameter("mesId", entity.MesId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("periodicidadRemunerativaId", entity.PeriodicidadRemunerativaId);
				parameters.AddParameter("fechaDesde", entity.FechaDesde);
				parameters.AddParameter("fechaHasta", entity.FechaHasta);
				parameters.AddParameter("fechaProceso", entity.FechaProceso);
				parameters.AddParameter("estadoPlanillaProcesoId", entity.EstadoPlanillaProcesoId);
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
		public int Update(PlanillaProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTipoCambio", entity.MontoTipoCambio);
				parameters.AddParameter("periodicidadRemunerativaId", entity.PeriodicidadRemunerativaId);
				parameters.AddParameter("fechaDesde", entity.FechaDesde);
				parameters.AddParameter("fechaHasta", entity.FechaHasta);
				parameters.AddParameter("fechaProceso", entity.FechaProceso);
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
		public void Delete(PlanillaProceso entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
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
