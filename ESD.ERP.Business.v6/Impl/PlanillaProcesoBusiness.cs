using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Types;

namespace ESD.ERP.Business.Impl
{
	public class PlanillaProcesoBusiness : IPlanillaProcesoBusiness
	{
		private readonly IPlanillaProcesoRepository planillaProcesoRepository;
		private readonly IPlanillaCalculoRepository planillaCalculoRepository;
		private readonly IPlanillaConceptoRemunerativoRepository planillaConceptoRemunerativoRepository;
		private readonly IVariableCalculoRepository variableCalculoRepository;

		public PlanillaProcesoBusiness(
			IPlanillaProcesoRepository planillaProcesoRepository,
			IPlanillaCalculoRepository planillaCalculoRepository,
			IPlanillaConceptoRemunerativoRepository planillaConceptoRemunerativoRepository,
			IVariableCalculoRepository variableCalculoRepository
			)
		{
			this.planillaProcesoRepository = planillaProcesoRepository;
			this.planillaCalculoRepository = planillaCalculoRepository;
			this.planillaConceptoRemunerativoRepository = planillaConceptoRemunerativoRepository;
			this.variableCalculoRepository = variableCalculoRepository;
		}
		public PlanillaProcesoDTO GetById(int id)
		{
			try
			{
				PlanillaProceso planillaProceso = planillaProcesoRepository.GetById(id);

				PlanillaProcesoDTO planillaProcesoDTO = Mapper.Map<PlanillaProcesoDTO>(planillaProceso);

				return planillaProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PlanillaProcesoDTO> Search(PlanillaProcesoDTO dto)
		{
			try
			{
				List<PlanillaProceso> planillaProcesoList = planillaProcesoRepository.Search(Mapper.Map<PlanillaProceso>(dto));

				List<PlanillaProcesoDTO> planillaProcesoDTOList = Mapper.Map<List<PlanillaProcesoDTO>>(planillaProcesoList);

				return planillaProcesoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PlanillaProcesoDTO SingleSearch(PlanillaProcesoDTO dto)
		{
			try
			{
				PlanillaProceso planillaProceso = planillaProcesoRepository.SingleSearch(Mapper.Map<PlanillaProceso>(dto));

				PlanillaProcesoDTO planillaProcesoDTO = Mapper.Map<PlanillaProcesoDTO>(planillaProceso);

				return planillaProcesoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PlanillaProcesoDTO dto)
		{
			try
			{
				PlanillaProceso planillaProceso = Mapper.Map<PlanillaProceso>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = planillaProcesoRepository.Insert(planillaProceso);

					dto.PlanillaProcesoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PlanillaProcesoDTO dto)
		{
			try
			{
				PlanillaProceso planillaProceso = Mapper.Map<PlanillaProceso>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = planillaProcesoRepository.Update(planillaProceso);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PlanillaProcesoDTO dto)
		{
			try
			{
				PlanillaProceso planillaProceso = Mapper.Map<PlanillaProceso>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					planillaProcesoRepository.Delete(planillaProceso);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void Calcular(PlanillaProcesoDTO dto)
      {
			try
			{
				string pattern = @"{(.*?)}";
				PlanillaProceso planillaProceso = planillaProcesoRepository.GetById(dto.PlanillaProcesoId);

				List<PlanillaConceptoRemunerativo> planillaConceptoRemunerativoList = planillaConceptoRemunerativoRepository.Search
					(new PlanillaConceptoRemunerativo() { OrderBy= "pcr.orden", PlanillaId = planillaProceso.PlanillaId });

				using (TransactionScope tx = new TransactionScope())
				{
					planillaCalculoRepository.CrearBase(new PlanillaCalculo() { PlanillaProcesoId = planillaProceso.EntityId });

					foreach (PlanillaConceptoRemunerativo planillaConceptoRemunerativo in planillaConceptoRemunerativoList) {
						List<string> tokenList = new List<string>();

						MatchCollection matchList = Regex.Matches(
							string.Concat(planillaConceptoRemunerativo.ConceptoRemunerativo.FormulaHora,
							planillaConceptoRemunerativo.ConceptoRemunerativo.Formula,
							planillaConceptoRemunerativo.ConceptoRemunerativo.Condicion),
							pattern);

						if(matchList.Count > 0)
							tokenList.AddRange(matchList.Cast<Match>().Select(match => match.Groups[1].Value).ToList());

                  VariableCalculo variableCalculo = new VariableCalculo
                  {
                     VariableCalculoList = tokenList
                  };

                  List<VariableCalculo> variableCalculoList = variableCalculoRepository.Search(variableCalculo);

						List<VariableProcesoType> variableProcesoTypeList = crearVariableProcesoTypeList(planillaProceso, variableCalculoList);

						PlanillaCalculo planillaCalculo = new PlanillaCalculo() {
							PlanillaProcesoId = planillaProceso.EntityId,
							ConceptoRemunerativoId = planillaConceptoRemunerativo.ConceptoRemunerativoId,
							VariableProcesoTypeList = variableProcesoTypeList
						};

						planillaCalculoRepository.CalcularConcepto(planillaCalculo);
					}
					planillaCalculoRepository.AgregarPrestamoCuota(new PlanillaCalculo() { PlanillaProcesoId = planillaProceso.EntityId });

					planillaCalculoRepository.AgregarRegularizacion(new PlanillaCalculo() { PlanillaProcesoId = planillaProceso.EntityId });

					planillaCalculoRepository.LimpiarBase(new PlanillaCalculo() { PlanillaProcesoId = planillaProceso.EntityId });

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
		private List<VariableProcesoType> crearVariableProcesoTypeList(PlanillaProceso planillaProceso, 
			List<VariableCalculo> variableCalculoList) {

			List<VariableProcesoType> variableProcesoTypeList = new List<VariableProcesoType>();

			foreach (VariableCalculo variableCalculo in variableCalculoList) {

				VariableProcesoType variableProcesoType = new VariableProcesoType
				{
					Variable = string.Concat("{", variableCalculo.Codigo, "}"),
					SubConsulta = SiNoConstante.N,
					Expresion = string.Empty
				};

				string valor = string.Empty;
				switch (variableCalculo.TipoVariableCalculo.Codigo) {
					case TipoVariableCalculoConstante.CAMPO_HORA:
						valor = string.Concat("(",
							string.Concat("(CAST(", variableCalculo.Codigo, " AS INT)+ROUND(CAST((", variableCalculo.Codigo, "%1)*100 AS INT)/60.0,2))"),
							"*", string.Format(StringFormatConstant.DECIMAL, variableCalculo.ValorNumericoVerdad), ")");
						break;
					case TipoVariableCalculoConstante.VARIABLE_TABLA:
						if (variableCalculo.EsSubconsulta == SiNoConstante.N)
							variableProcesoType.Expresion = string.Concat(variableCalculo.ValorCadenaVerdad, " FROM ", variableCalculo.TablaOrigen,
								string.IsNullOrEmpty(variableCalculo.TablaCondicion) ? string.Empty : string.Concat(" WHERE ", variableCalculo.TablaCondicion));
						else {
							valor = variableCalculo.ValorCadenaVerdad;
							variableProcesoType.Expresion = string.Concat(variableCalculo.SubConsultaColumna, " FROM ", variableCalculo.TablaOrigen,
								string.IsNullOrEmpty(variableCalculo.TablaCondicion) ? string.Empty : string.Concat(" WHERE ", variableCalculo.TablaCondicion),
								string.IsNullOrEmpty(variableCalculo.TablaAgrupacion) ? string.Empty : string.Concat(" GROUP BY ", variableCalculo.TablaAgrupacion),
								") ", variableCalculo.SubConsultaAlias," ON ", variableCalculo.SubConsultaEnlace);
							variableProcesoType.SubConsulta = SiNoConstante.S;
						}
						break;
					default:
						switch (variableCalculo.TipoDato.Codigo) {
							case TipoDatoConstante.CADENA:
								valor = variableCalculo.ValorCadenaVerdad;
								break;

							case TipoDatoConstante.ENTERO:
								valor = string.Format(StringFormatConstant.INTEGER, Convert.ToInt32(variableCalculo.ValorNumericoVerdad));
								break;

							case TipoDatoConstante.DECIMAL:
								valor = string.Format(StringFormatConstant.DECIMAL, variableCalculo.ValorNumericoVerdad);
								break;
						}
						break;
				}

				variableProcesoType.TipoDato = variableCalculo.TipoDato.Codigo;
				variableProcesoType.SubConsultaTipo = variableCalculo.SubConsultaTipo;
				variableProcesoType.Valor = valor;
				variableProcesoTypeList.Add(variableProcesoType);
			}

			foreach (VariableProcesoType item in variableProcesoTypeList) {
				StringBuilder stringBuilder = new StringBuilder(item.Expresion);

				stringBuilder.Replace("{fechaInicio}", string.Concat("'",string.Format(StringFormatConstant.DATE_ISO,planillaProceso.FechaDesde), "'"));
				stringBuilder.Replace("{fechaFin}", string.Concat("'", string.Format(StringFormatConstant.DATE_ISO, planillaProceso.FechaHasta), "'"));
				stringBuilder.Replace("{mes}", string.Concat("'", planillaProceso.Mes.Codigo, "'"));
				stringBuilder.Replace("{planillaId}", planillaProceso.PlanillaId.ToString());
				stringBuilder.Replace("{procesoAnual}", planillaProceso.ProcesoAnual.ToString());

				item.Expresion = stringBuilder.ToString();
			}

			return variableProcesoTypeList;
		}

	}
}
