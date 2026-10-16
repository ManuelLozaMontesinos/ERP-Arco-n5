using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaProcesoHoraRepository : IPlanillaProcesoHoraRepository
	{
		private const string SP_PRE_NAME = "sp_plan_p_hora";
		public PlanillaProcesoHora GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PlanillaProcesoHora)
					},
					x =>
					{
						PlanillaProcesoHora pph = x[0] as PlanillaProcesoHora;

						return pph;
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
		public List<PlanillaProcesoHora> Search(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PlanillaProcesoHora)
					},
					x =>
					{
						PlanillaProcesoHora pph = x[0] as PlanillaProcesoHora;

						return pph;
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
		public PlanillaProcesoHora SingleSearch(PlanillaProcesoHora entity)
		{
			try
			{
				List<PlanillaProcesoHora> planillaProcesoHoraList = Search(entity);

				return planillaProcesoHoraList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planProceId", entity.PlanillaProcesoId);
				parameters.AddParameter("horaRemId", entity.HoraRemunerativaId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("cantidad", entity.Cantidad);
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
		public int Update(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoHoraId", entity.PlanillaProcesoHoraId);
				parameters.AddParameter("planProceId", entity.PlanillaProcesoId);
				parameters.AddParameter("horaRemId", entity.HoraRemunerativaId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("cantidad", entity.Cantidad);
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
		public void Delete(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoHoraId", entity.PlanillaProcesoHoraId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarHoraPivot(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarPivot"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<PlanillaProcesoHora> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<PlanillaProcesoHoraType> planillaProcesoHoraTypeList = new List<PlanillaProcesoHoraType>();
				planillaProcesoHoraTypeList = (from s in list
														select new PlanillaProcesoHoraType
														{
															PlanillaProcesoHoraId = s.PlanillaProcesoHoraId,
															PlanillaProcesoId = s.PlanillaProcesoId,
															HoraRemunerativaId = s.HoraRemunerativaId,
															Personal = string.Empty,
															PersonalId = s.PersonalId,
															Cantidad = s.Cantidad,
															Estado = s.Estado,
															UsuarioCreacionId = s.UsuarioCreacionId,
														}).ToList();

				parameters.AddTableParameter("planillaProcesoHoraList", "dbo.PlanillaProcesoHoraType", planillaProcesoHoraTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLotePorPersonal(List<PlanillaProcesoHora> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<PlanillaProcesoHoraType> planillaProcesoHoraTypeList = new List<PlanillaProcesoHoraType>();
				planillaProcesoHoraTypeList = (from s in list
														 select new PlanillaProcesoHoraType
														 {
															 PlanillaProcesoHoraId = s.PlanillaProcesoHoraId,
															 PlanillaProcesoId = s.PlanillaProcesoId,
															 HoraRemunerativaId = s.HoraRemunerativaId,
															 Personal = s.Personal.Codigo,
															 Cantidad = s.Cantidad,
															 Estado = s.Estado,
															 UsuarioCreacionId = s.UsuarioCreacionId,
														 }).ToList();

				parameters.AddTableParameter("planillaProcesoHoraList", "dbo.PlanillaProcesoHoraType", planillaProcesoHoraTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLotePorPersonal"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarListaPersonalId(PlanillaProcesoHora entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("personalIdList", entity.PersonalIdList);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarListaPersonalId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarDesdeMarcacion(PlanillaProcesoHora entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaProcesoId", entity.PlanillaProcesoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarDesdeMarcacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
