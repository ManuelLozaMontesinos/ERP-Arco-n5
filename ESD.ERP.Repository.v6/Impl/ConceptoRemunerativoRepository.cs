using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ConceptoRemunerativoRepository : IConceptoRemunerativoRepository
	{
		private const string SP_PRE_NAME = "sp_concep_remu";
		public ConceptoRemunerativo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ConceptoRemunerativo)
					},
					x =>
					{
						ConceptoRemunerativo cr = x[0] as ConceptoRemunerativo;

						return cr;
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
		public List<ConceptoRemunerativo> Search(ConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("esRegularizacion", entity.EsRegularizacion);
			parameters.AddParameter("usaFormula", entity.UsaFormula);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ConceptoRemunerativo),
						typeof(TipoConceptoRemunerativo)
					},
					x =>
					{
						ConceptoRemunerativo cr = x[0] as ConceptoRemunerativo;
						TipoConceptoRemunerativo tcr = x[1] as TipoConceptoRemunerativo;

						cr.TipoConceptoRemunerativoId = tcr.EntityId;
						cr.TipoConceptoRemunerativo = tcr;

						return cr;
					},
					"tipo_con_remu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ConceptoRemunerativo SingleSearch(ConceptoRemunerativo entity)
		{
			try
			{
				List<ConceptoRemunerativo> conceptoRemunerativoList = Search(entity);

				return conceptoRemunerativoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("conceptoRemunerativo", entity.Codigo);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("nombreBoleta", entity.NombreBoleta);
				parameters.AddParameter("esRegularizacion", entity.EsRegularizacion);
				parameters.AddParameter("tipoConceptoRemunerativoId", entity.TipoConceptoRemunerativoId);
				parameters.AddParameter("ingresoTributoDescuentoId", entity.IngresoTributoDescuentoId);
				parameters.AddParameter("usaFormula", entity.UsaFormula);
				parameters.AddParameter("formulaHora", entity.FormulaHora);
				parameters.AddParameter("formula", entity.Formula);
				parameters.AddParameter("condicion", entity.Condicion);
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
		public int Update(ConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("nombreBoleta", entity.NombreBoleta);
				parameters.AddParameter("esRegularizacion", entity.EsRegularizacion);
				parameters.AddParameter("tipoConceptoRemunerativoId", entity.TipoConceptoRemunerativoId);
				parameters.AddParameter("ingresoTributoDescuentoId", entity.IngresoTributoDescuentoId);
				parameters.AddParameter("usaFormula", entity.UsaFormula);
				parameters.AddParameter("formulaHora", entity.FormulaHora);
				parameters.AddParameter("formula", entity.Formula);
				parameters.AddParameter("condicion", entity.Condicion);
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
		public void Delete(ConceptoRemunerativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
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
