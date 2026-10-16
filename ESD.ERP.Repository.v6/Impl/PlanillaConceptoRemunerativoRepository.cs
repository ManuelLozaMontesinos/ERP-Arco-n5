using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaConceptoRemunerativoRepository : IPlanillaConceptoRemunerativoRepository
	{
		private const string SP_PRE_NAME = "sp_plan_con_rem";
		public PlanillaConceptoRemunerativo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PlanillaConceptoRemunerativo),
						typeof(ConceptoRemunerativo)
					},
					x =>
					{
						PlanillaConceptoRemunerativo pcr = x[0] as PlanillaConceptoRemunerativo;
						ConceptoRemunerativo cr = x[1] as ConceptoRemunerativo;

						pcr.ConceptoRemunerativoId = cr.EntityId;
						pcr.ConceptoRemunerativo = cr;

						return pcr;
					},
					"concep_remu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PlanillaConceptoRemunerativo> Search(PlanillaConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("planillaId", entity.PlanillaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PlanillaConceptoRemunerativo),
						typeof(ConceptoRemunerativo),
						typeof(TipoConceptoRemunerativo)
					},
					x =>
					{
						PlanillaConceptoRemunerativo pcr = x[0] as PlanillaConceptoRemunerativo;
						ConceptoRemunerativo cr = x[1] as ConceptoRemunerativo;
						TipoConceptoRemunerativo tcr = x[2] as TipoConceptoRemunerativo;

						pcr.ConceptoRemunerativoId = cr.EntityId;
						pcr.ConceptoRemunerativo = cr;
						cr.TipoConceptoRemunerativoId = tcr.EntityId;
						cr.TipoConceptoRemunerativo = tcr;

						return pcr;
					},
					"concep_remu_id,tipo_con_remu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PlanillaConceptoRemunerativo SingleSearch(PlanillaConceptoRemunerativo entity)
		{
			try
			{
				List<PlanillaConceptoRemunerativo> planillaConceptoRemunerativoList = Search(entity);

				return planillaConceptoRemunerativoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PlanillaConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaId", entity.PlanillaId);
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
				parameters.AddParameter("orden", entity.Orden);
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
		public int Update(PlanillaConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaConceptoRemunerativoId", entity.PlanillaConceptoRemunerativoId);
				parameters.AddParameter("orden", entity.Orden);
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
		public void Delete(PlanillaConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaConceptoRemunerativoId", entity.PlanillaConceptoRemunerativoId);
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
