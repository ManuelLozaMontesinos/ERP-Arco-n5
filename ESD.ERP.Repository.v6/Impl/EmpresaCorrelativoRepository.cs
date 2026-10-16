using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EmpresaCorrelativoRepository : IEmpresaCorrelativoRepository
	{
		private const string SP_PRE_NAME = "sp_empresa_corre";
		public EmpresaCorrelativo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EmpresaCorrelativo)
					},
					x =>
					{
						EmpresaCorrelativo ec = x[0] as EmpresaCorrelativo;

						return ec;
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
		public List<EmpresaCorrelativo> Search(EmpresaCorrelativo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);
			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("tablaId", entity.TablaId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EmpresaCorrelativo)
					},
					x =>
					{
						EmpresaCorrelativo ec = x[0] as EmpresaCorrelativo;

						return ec;
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
		public EmpresaCorrelativo SingleSearch(EmpresaCorrelativo entity)
		{
			try
			{
				List<EmpresaCorrelativo> empresaCorrelativoList = Search(entity);

				return empresaCorrelativoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EmpresaCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("tablaId", entity.TablaId);
				parameters.AddParameter("correlativo", entity.Correlativo);
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
		public int Update(EmpresaCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaCorrelativoId", entity.EmpresaCorrelativoId);
				parameters.AddParameter("correlativo", entity.Correlativo);
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
		public void Delete(EmpresaCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaCorreId", entity.EmpresaCorrelativoId);

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
