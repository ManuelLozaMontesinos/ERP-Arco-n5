using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class UnidadMedidaRepository : IUnidadMedidaRepository
	{
		private const string SP_PRE_NAME = "sp_unidad_medida";
		public UnidadMedida GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(UnidadMedida)
					},
					x =>
					{
						UnidadMedida um = x[0] as UnidadMedida;

						return um;
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
		public List<UnidadMedida> Search(UnidadMedida entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(UnidadMedida)
					},
					x =>
					{
						UnidadMedida um = x[0] as UnidadMedida;

						return um;
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
		public UnidadMedida SingleSearch(UnidadMedida entity)
		{
			try
			{
				List<UnidadMedida> unidadMedidaList = Search(entity);

				return unidadMedidaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(UnidadMedida entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("unidadMedida", entity.Codigo);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("codigoISO", entity.CodigoISO);
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
		public int Update(UnidadMedida entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("unidadMedida", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("codigoISO", entity.CodigoISO);
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
		public void Delete(UnidadMedida entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public UnidadMedida ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(UnidadMedida)
					},
					x =>
					{
						UnidadMedida um = x[0] as UnidadMedida;

						return um;
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
