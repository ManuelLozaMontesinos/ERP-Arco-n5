using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class SituacionEspecialTrabajadorRepository : ISituacionEspecialTrabajadorRepository
	{
		private const string SP_PRE_NAME = "sp_situ_espe_trab";
		public SituacionEspecialTrabajador GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(SituacionEspecialTrabajador)
					},
					x =>
					{
						SituacionEspecialTrabajador set = x[0] as SituacionEspecialTrabajador;

						return set;
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
		public List<SituacionEspecialTrabajador> Search(SituacionEspecialTrabajador entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(SituacionEspecialTrabajador)
					},
					x =>
					{
						SituacionEspecialTrabajador set = x[0] as SituacionEspecialTrabajador;

						return set;
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
		public SituacionEspecialTrabajador SingleSearch(SituacionEspecialTrabajador entity)
		{
			try
			{
				List<SituacionEspecialTrabajador> situacionEspecialTrabajadorList = Search(entity);

				return situacionEspecialTrabajadorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(SituacionEspecialTrabajador entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEspecialTrabajador", entity.Codigo);
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
		public int Update(SituacionEspecialTrabajador entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEspecialTrabajadorId", entity.SituacionEspecialTrabajadorId);
				parameters.AddParameter("situacionEspecialTrabajador", entity.Codigo);
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
		public void Delete(SituacionEspecialTrabajador entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("situacionEspecialTrabajadorId", entity.SituacionEspecialTrabajadorId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public SituacionEspecialTrabajador ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(SituacionEspecialTrabajador)
					},
					x =>
					{
						SituacionEspecialTrabajador set = x[0] as SituacionEspecialTrabajador;

						return set;
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
