using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalPlanillaRepository : IPersonalPlanillaRepository
	{
		private const string SP_PRE_NAME = "sp_perso_plan";
		public PersonalPlanilla GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalPlanilla)
					},
					x =>
					{
						PersonalPlanilla pp = x[0] as PersonalPlanilla;

						return pp;
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
		public List<PersonalPlanilla> Search(PersonalPlanilla entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("planillaId", entity.PlanillaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalPlanilla),
						typeof(Planilla)
					},
					x =>
					{
						PersonalPlanilla pp = x[0] as PersonalPlanilla;
						Planilla p = x[1] as Planilla;

						pp.Planilla = p;
						pp.PlanillaId = p.EntityId;

						return pp;
					},
					"planilla_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalPlanilla SingleSearch(PersonalPlanilla entity)
		{
			try
			{
				List<PersonalPlanilla> personalPlanillaList = Search(entity);

				return personalPlanillaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalPlanilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("planillaId", entity.PlanillaId);
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
		public int Update(PersonalPlanilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalPlanillaId", entity.PersonalPlanillaId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("planillaId", entity.PlanillaId);
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
		public void Delete(PersonalPlanilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalPlanillaId", entity.PersonalPlanillaId);
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
