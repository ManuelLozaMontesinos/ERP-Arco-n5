using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoVariableCalculoRepository : ITipoVariableCalculoRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_v_calc";
		public TipoVariableCalculo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoVariableCalculo)
					},
					x =>
					{
						TipoVariableCalculo tvc = x[0] as TipoVariableCalculo;

						return tvc;
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
		public List<TipoVariableCalculo> Search(TipoVariableCalculo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoVariableCalculo)
					},
					x =>
					{
						TipoVariableCalculo tvc = x[0] as TipoVariableCalculo;

						return tvc;
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
		public TipoVariableCalculo SingleSearch(TipoVariableCalculo entity)
		{
			try
			{
				List<TipoVariableCalculo> tipoVariableCalculoList = Search(entity);

				return tipoVariableCalculoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoVariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoVariableCalculo", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(TipoVariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoVariableCalculoId", entity.TipoVariableCalculoId);
				parameters.AddParameter("tipoVariableCalculo", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public void Delete(TipoVariableCalculo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoVariableCalculoId", entity.TipoVariableCalculoId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoVariableCalculo ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(TipoVariableCalculo)
					},
					x =>
					{
						TipoVariableCalculo tvc = x[0] as TipoVariableCalculo;

						return tvc;
					},
					string.Empty
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
