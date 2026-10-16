using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PlanillaRepository : IPlanillaRepository
	{
		private const string SP_PRE_NAME = "sp_planilla";
		public Planilla GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Planilla)
					},
					x =>
					{
						Planilla p = x[0] as Planilla;

						return p;
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
		public List<Planilla> Search(Planilla entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Planilla),
						typeof(TipoTrabajador)
					},
					x =>
					{
						Planilla p = x[0] as Planilla;
						TipoTrabajador tt = x[1] as TipoTrabajador;

						p.TipoTrabajador = tt;
						p.TipoTrabajadorId = tt.EntityId;

						return p;
					},
					"tipo_trabaja_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Planilla SingleSearch(Planilla entity)
		{
			try
			{
				List<Planilla> planillaList = Search(entity);

				return planillaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Planilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("procesoAnual", entity.ProcesoAnual);
				parameters.AddParameter("tipoTrabajadorId", entity.TipoTrabajadorId);
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
		public int Update(Planilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaId", entity.PlanillaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("procesoAnual", entity.ProcesoAnual);
				parameters.AddParameter("tipoTrabajadorId", entity.TipoTrabajadorId);
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
		public void Delete(Planilla entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("planillaId", entity.PlanillaId);
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
