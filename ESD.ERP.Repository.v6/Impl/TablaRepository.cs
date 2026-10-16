using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TablaRepository : ITablaRepository
	{
		private const string SP_PRE_NAME = "sp_tabla";
		public Tabla GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Tabla)
					},
					x =>
					{
						Tabla t = x[0] as Tabla;

						return t;
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
		public List<Tabla> Search(Tabla entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Tabla)
					},
					x =>
					{
						Tabla t = x[0] as Tabla;

						return t;
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
		public Tabla SingleSearch(Tabla entity)
		{
			try
			{
				List<Tabla> tablaList = Search(entity);

				return tablaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Tabla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tabla", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("correlativoPorEmpresa", entity.CorrelativoPorEmpresa);
				parameters.AddParameter("correlativoPorEmpresaSede", entity.CorrelativoPorEmpresaSede);
				parameters.AddParameter("correlativo", entity.Correlativo);
				parameters.AddParameter("formatoCorrelativo", entity.FormatoCorrelativo);
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
		public int Update(Tabla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tablaId", entity.TablaId);
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
		public void Delete(Tabla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tablaId", entity.TablaId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Tabla ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(Tabla)
					},
					x =>
					{
						Tabla t = x[0] as Tabla;

						return t;
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
