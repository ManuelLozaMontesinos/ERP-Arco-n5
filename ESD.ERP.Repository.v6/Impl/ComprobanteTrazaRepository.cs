using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteTrazaRepository : IComprobanteTrazaRepository
	{
		private const string SP_PRE_NAME = "sp_comprob_traza";
		public ComprobanteTraza GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComprobanteTraza)
					},
					x =>
					{
						ComprobanteTraza td = x[0] as ComprobanteTraza;

						return td;
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
		public List<ComprobanteTraza> Search(ComprobanteTraza entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComprobanteTraza),
						typeof(TipoAccionAuditoria),
						typeof(Usuario)
					},
					x =>
					{
						ComprobanteTraza ct = x[0] as ComprobanteTraza;
						TipoAccionAuditoria taa = x[1] as TipoAccionAuditoria;
						Usuario u = x[2] as Usuario;

						ct.TipoAccionAuditoria = taa;
						ct.TipoAccionAuditoriaId = taa.EntityId;
						ct.Usuario = u;
						ct.UsuarioId = u.EntityId;

						return ct;
					},
					"tipo_acc_audit_id,usuario_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ComprobanteTraza SingleSearch(ComprobanteTraza entity)
		{
			try
			{
				List<ComprobanteTraza> comprobanteTrazaList = Search(entity);

				return comprobanteTrazaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComprobanteTraza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("tipoAccionAuditoriaId", entity.TipoAccionAuditoriaId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
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
		public int Update(ComprobanteTraza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteTrazaId", entity.ComprobanteTrazaId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
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
		public void Delete(ComprobanteTraza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteTrazaId", entity.ComprobanteTrazaId);
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
