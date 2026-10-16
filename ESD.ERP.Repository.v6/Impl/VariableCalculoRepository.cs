using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class VariableCalculoRepository : IVariableCalculoRepository
	{
		private const string SP_PRE_NAME = "sp_var_calc";
		public VariableCalculo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(VariableCalculo),
						typeof(TipoDato)
					},
					x =>
					{
						VariableCalculo vc = x[0] as VariableCalculo;
						TipoDato td = x[1] as TipoDato;

						vc.TipoDatoId = td.EntityId;
						vc.TipoDato = td;

						return vc;
					},
					"tipo_dato_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<VariableCalculo> Search(VariableCalculo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("notVariableCalculoId", entity.NotVariableCalculoId);
			parameters.AddParameter("variableCalculo", entity.Codigo);
			parameters.AddParameter("variableCalculoList", entity.VariableCalculoList);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("alcanceFormulaHora", entity.AlcanceFormulaHora);
			parameters.AddParameter("alcanceFormula", entity.AlcanceFormula);
			parameters.AddParameter("alcanceCondicion", entity.AlcanceCondicion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(VariableCalculo),
						typeof(TipoVariableCalculo),
						typeof(TipoDato)
					},
					x =>
					{
						VariableCalculo vc = x[0] as VariableCalculo;
						TipoVariableCalculo tvc = x[1] as TipoVariableCalculo;
						TipoDato td = x[2] as TipoDato;

						vc.TipoVariableCalculoId = tvc.EntityId;
						vc.TipoVariableCalculo = tvc;
						vc.TipoDatoId = td.EntityId;
						vc.TipoDato = td;

						return vc;
					},
					"tipo_v_calc_id,tipo_dato_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public VariableCalculo SingleSearch(VariableCalculo entity)
		{
			try
			{
				List<VariableCalculo> variableCalculoList = Search(entity);

				return variableCalculoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(VariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("variableCalculo", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("tipoVariableCalculoId", entity.TipoVariableCalculoId);
				parameters.AddParameter("condicion", entity.Condicion);
				parameters.AddParameter("tipoDatoId", entity.TipoDatoId);
				parameters.AddParameter("valorNumericoVerdad", entity.ValorNumericoVerdad);
				parameters.AddParameter("valorNumericoFalso", entity.ValorNumericoFalso);
				parameters.AddParameter("valorCadenaVerdad", entity.ValorCadenaVerdad);
				parameters.AddParameter("valorCadenaFalso", entity.ValorCadenaFalso);
				parameters.AddParameter("tablaOrigen", entity.TablaOrigen);
				parameters.AddParameter("tablaCondicion", entity.TablaCondicion);
				parameters.AddParameter("tablaAgrupacion", entity.TablaAgrupacion);
				parameters.AddParameter("esSubconsulta", entity.EsSubconsulta);
				parameters.AddParameter("subConsultaAlias", entity.SubConsultaAlias);
				parameters.AddParameter("subConsultaTipo", entity.SubConsultaTipo);
				parameters.AddParameter("subConsultaColumna", entity.SubConsultaColumna);
				parameters.AddParameter("subConsultaEnlace", entity.SubConsultaEnlace);
				parameters.AddParameter("alcanceFormulaHora", entity.AlcanceFormulaHora);
				parameters.AddParameter("alcanceFormula", entity.AlcanceFormula);
				parameters.AddParameter("alcanceCondicion", entity.AlcanceCondicion);
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
		public int Update(VariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("variableCalculoId", entity.VariableCalculoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("condicion", entity.Condicion);
				parameters.AddParameter("valorNumericoVerdad", entity.ValorNumericoVerdad);
				parameters.AddParameter("valorNumericoFalso", entity.ValorNumericoFalso);
				parameters.AddParameter("valorCadenaVerdad", entity.ValorCadenaVerdad);
				parameters.AddParameter("valorCadenaFalso", entity.ValorCadenaFalso);
				parameters.AddParameter("tablaOrigen", entity.TablaOrigen);
				parameters.AddParameter("tablaCondicion", entity.TablaCondicion);
				parameters.AddParameter("tablaAgrupacion", entity.TablaAgrupacion);
				parameters.AddParameter("esSubconsulta", entity.EsSubconsulta);
				parameters.AddParameter("subConsultaAlias", entity.SubConsultaAlias);
				parameters.AddParameter("subConsultaTipo", entity.SubConsultaTipo);
				parameters.AddParameter("subConsultaColumna", entity.SubConsultaColumna);
				parameters.AddParameter("subConsultaEnlace", entity.SubConsultaEnlace);
				parameters.AddParameter("alcanceFormulaHora", entity.AlcanceFormulaHora);
				parameters.AddParameter("alcanceFormula", entity.AlcanceFormula);
				parameters.AddParameter("alcanceCondicion", entity.AlcanceCondicion);
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
		public void Delete(VariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("variableCalculoId", entity.VariableCalculoId);
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
