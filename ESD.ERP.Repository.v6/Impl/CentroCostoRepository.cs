using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CentroCostoRepository : ICentroCostoRepository
	{
		private const string SP_PRE_NAME = "sp_centro_costo";
		public CentroCosto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CentroCosto)
					},
					x =>
					{
						CentroCosto cc = x[0] as CentroCosto;

						return cc;
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
		public List<CentroCosto> Search(CentroCosto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CentroCosto)
					},
					x =>
					{
						CentroCosto cc = x[0] as CentroCosto;

						return cc;
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
		public CentroCosto SingleSearch(CentroCosto entity)
		{
			try
			{
				List<CentroCosto> centroCostoList = Search(entity);

				return centroCostoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CentroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("centroCostoPadreId", entity.CentroCostoPadreId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("sigla", entity.Sigla);
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
		public int Update(CentroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("sigla", entity.Sigla);
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
		public void Delete(CentroCosto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
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
