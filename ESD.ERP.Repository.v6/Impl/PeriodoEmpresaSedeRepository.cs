using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PeriodoEmpresaSedeRepository : IPeriodoEmpresaSedeRepository
	{
		private const string SP_PRE_NAME = "sp_periodo_emp_sede";
		public PeriodoEmpresaSede GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PeriodoEmpresaSede),
						typeof(Periodo),
						typeof(EmpresaSede),
						typeof(Empresa)
					},
					x =>
					{
						PeriodoEmpresaSede pes = x[0] as PeriodoEmpresaSede;
						Periodo p = x[1] as Periodo;
						EmpresaSede es = x[2] as EmpresaSede;
						Empresa e = x[3] as Empresa;

						pes.PeriodoId = p.EntityId;
						pes.Periodo = p;
						pes.EmpresaSedeId = es.EntityId;
						pes.EmpresaSede = es;
						es.EmpresaId = e.EntityId;
						es.Empresa = e;

						return pes;
					},
					"periodo_id,empresa_sede_id,empresa_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PeriodoEmpresaSede> Search(PeriodoEmpresaSede entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoId", entity.PeriodoId);
			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("empresaSedeIdList", entity.EmpresaSedeIdList);
			parameters.AddParameter("empresaId", entity.EmpresaSede?.EmpresaId);
			parameters.AddParameter("esCentral", entity.EmpresaSede?.EsCentral);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(Empresa)
					},
					x =>
					{
						PeriodoEmpresaSede pes = x[0] as PeriodoEmpresaSede;
						EmpresaSede es = x[1] as EmpresaSede;
						Empresa e = x[2] as Empresa;

						pes.EmpresaSedeId = es.EntityId;
						pes.EmpresaSede = es;
						es.EmpresaId = e.EntityId;
						es.Empresa = e;

						return pes;
					},
					"empresa_sede_id,empresa_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PeriodoEmpresaSede SingleSearch(PeriodoEmpresaSede entity)
		{
			try
			{
				List<PeriodoEmpresaSede> periodoEmpresaSedeList = Search(entity);

				return periodoEmpresaSedeList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PeriodoEmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoId", entity.PeriodoId);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
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
		public int Update(PeriodoEmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("periodoId", entity.PeriodoId);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
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
		public void Delete(PeriodoEmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
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
