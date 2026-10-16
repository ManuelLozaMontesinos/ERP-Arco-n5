using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaProcesoRegularizacionRepository : IPlanillaProcesoRegularizacionRepository
	{
		private const string SP_PRE_NAME = "sp_plan_p_regu";
		public PlanillaProcesoRegularizacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PlanillaProcesoRegularizacion)
					},
					x =>
					{
						PlanillaProcesoRegularizacion ppr = x[0] as PlanillaProcesoRegularizacion;

						return ppr;
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
		public List<PlanillaProcesoRegularizacion> Search(PlanillaProcesoRegularizacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PlanillaProcesoRegularizacion),
						typeof(Personal),
						typeof(Proveedor),
						typeof(ConceptoRemunerativo)
					},
					x =>
					{
						PlanillaProcesoRegularizacion ppr = x[0] as PlanillaProcesoRegularizacion;
						Personal p = x[1] as Personal;
						Proveedor prv = x[2] as Proveedor;
						ConceptoRemunerativo cr = x[3] as ConceptoRemunerativo;

						ppr.Personal = p;
						ppr.PersonalId = p.EntityId;
						p.Proveedor = prv;
						p.ProveedorId = prv.EntityId;
						ppr.ConceptoRemunerativo = cr;
						ppr.ConceptoRemunerativoId = cr.EntityId;

						return ppr;
					},
					"personal_id,proveedor_id,concep_remu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PlanillaProcesoRegularizacion SingleSearch(PlanillaProcesoRegularizacion entity)
		{
			try
			{
				List<PlanillaProcesoRegularizacion> planillaProcesoRegularizacionList = Search(entity);

				return planillaProcesoRegularizacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PlanillaProcesoRegularizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("PersonalId", entity.PersonalId);
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
				parameters.AddParameter("cantidadHora", entity.CantidadHora);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("observacion", entity.Observacion);
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
		public int Update(PlanillaProcesoRegularizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoRegularizacionId", entity.PlanillaProcesoRegularizacionId);
				parameters.AddParameter("cantidadHora", entity.CantidadHora);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("observacion", entity.Observacion);
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
		public void Delete(PlanillaProcesoRegularizacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoRegularizacionId", entity.PlanillaProcesoRegularizacionId);
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
