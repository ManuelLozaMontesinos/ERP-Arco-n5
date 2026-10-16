using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaCalculoRepository : IPlanillaCalculoRepository
	{
		private const string SP_PRE_NAME = "sp_plan_calculo";
		public PlanillaCalculo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PlanillaCalculo)
					},
					x =>
					{
						PlanillaCalculo pc = x[0] as PlanillaCalculo;

						return pc;
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
		public List<PlanillaCalculo> Search(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PlanillaCalculo)
					},
					x =>
					{
						PlanillaCalculo pc = x[0] as PlanillaCalculo;

						return pc;
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
		public PlanillaCalculo SingleSearch(PlanillaCalculo entity)
		{
			try
			{
				List<PlanillaCalculo> planillaCalculoList = Search(entity);

				return planillaCalculoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planProceId", entity.PlanillaProcesoId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("concepRemuId", entity.ConceptoRemunerativoId);
				parameters.AddParameter("horas", entity.Horas);
				parameters.AddParameter("montoSub", entity.MontoSubTotal);
				parameters.AddParameter("monto", entity.Monto);
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
		public int Update(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planCalculoId", entity.PlanillaCalculoId);
				parameters.AddParameter("planProceId", entity.PlanillaProcesoId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("concepRemuId", entity.ConceptoRemunerativoId);
				parameters.AddParameter("horas", entity.Horas);
				parameters.AddParameter("montoSub", entity.MontoSubTotal);
				parameters.AddParameter("monto", entity.Monto);
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
		public void Delete(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planCalculoId", entity.PlanillaCalculoId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void CrearBase(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_crearBase"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void LimpiarBase(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_limpiarBase"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void CalcularConcepto(PlanillaCalculo entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
				parameters.AddTableParameter("variableProcesoList", "dbo.VariableProcesoType", entity.VariableProcesoTypeList);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_calcularConcepto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void AgregarPrestamoCuota(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_agregarPrestamoCuota"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void AgregarRegularizacion(PlanillaCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_agregarRegularizacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
