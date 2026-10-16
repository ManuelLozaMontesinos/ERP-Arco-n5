using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalPeriodoLaboralRepository : IPersonalPeriodoLaboralRepository
	{
		private const string SP_PRE_NAME = "sp_perso_per_lab";
		public PersonalPeriodoLaboral GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalPeriodoLaboral)
					},
					x =>
					{
						PersonalPeriodoLaboral ppl = x[0] as PersonalPeriodoLaboral;

						return ppl;
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
		public List<PersonalPeriodoLaboral> Search(PersonalPeriodoLaboral entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalPeriodoLaboral),
						typeof(MotivoFinPeriodo)
					},
					x =>
					{
						PersonalPeriodoLaboral ppl = x[0] as PersonalPeriodoLaboral;
						MotivoFinPeriodo mfp = x[1] as MotivoFinPeriodo;

						ppl.MotivoFinPeriodoId = mfp?.EntityId;
						ppl.MotivoFinPeriodo = mfp;

						return ppl;
					},
					"motivo_fin_peri_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalPeriodoLaboral SingleSearch(PersonalPeriodoLaboral entity)
		{
			try
			{
				List<PersonalPeriodoLaboral> personalPeriodoLaboralList = Search(entity);

				return personalPeriodoLaboralList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalPeriodoLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaIngreso", entity.FechaIngreso);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
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
		public int Update(PersonalPeriodoLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalPeriodoLaboralId", entity.PersonalPeriodoLaboralId);
				parameters.AddParameter("fechaIngreso", entity.FechaIngreso);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
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
		public void Delete(PersonalPeriodoLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalPeriodoLaboralId", entity.PersonalPeriodoLaboralId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCese(PersonalPeriodoLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalPeriodoLaboralId", entity.PersonalPeriodoLaboralId);
				parameters.AddParameter("fechaCese", entity.FechaCese);
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCese"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
