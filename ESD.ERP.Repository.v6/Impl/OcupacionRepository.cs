using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OcupacionRepository : IOcupacionRepository
	{
		private const string SP_PRE_NAME = "sp_ocupacion";
		public Ocupacion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Ocupacion)
					},
					x =>
					{
						Ocupacion o = x[0] as Ocupacion;

						return o;
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
		public List<Ocupacion> Search(Ocupacion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("esEjecutivo", entity.EsEjecutivo);
			parameters.AddParameter("esEmpleado", entity.EsEmpleado);
			parameters.AddParameter("esObrero", entity.EsObrero);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Ocupacion)
					},
					x =>
					{
						Ocupacion o = x[0] as Ocupacion;

						return o;
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
		public Ocupacion SingleSearch(Ocupacion entity)
		{
			try
			{
				List<Ocupacion> ocupacionList = Search(entity);

				return ocupacionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Ocupacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ocupacion", entity.Codigo);
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
		public int Update(Ocupacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ocupacionId", entity.OcupacionId);
				parameters.AddParameter("ocupacion", entity.Codigo);
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
		public void Delete(Ocupacion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ocupacionId", entity.OcupacionId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Ocupacion ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(Ocupacion)
					},
					x =>
					{
						Ocupacion o = x[0] as Ocupacion;

						return o;
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
