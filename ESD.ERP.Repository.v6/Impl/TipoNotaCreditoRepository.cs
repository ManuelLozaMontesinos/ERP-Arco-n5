using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoNotaCreditoRepository : ITipoNotaCreditoRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_nota_c";
		public TipoNotaCredito GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoNotaCredito)
					},
					x =>
					{
						TipoNotaCredito tnc = x[0] as TipoNotaCredito;

						return tnc;
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
		public List<TipoNotaCredito> Search(TipoNotaCredito entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoNotaCredito)
					},
					x =>
					{
						TipoNotaCredito tnc = x[0] as TipoNotaCredito;

						return tnc;
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
		public TipoNotaCredito SingleSearch(TipoNotaCredito entity)
		{
			try
			{
				List<TipoNotaCredito> tipoNotaCreditoList = Search(entity);

				return tipoNotaCreditoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoNotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoNotaCredito", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("afectaAlmacen", entity.AfectaAlmacen);
				parameters.AddParameter("esIngreso", entity.EsIngreso);
				parameters.AddParameter("permiteEditItem", entity.PermiteEditItem);
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
		public int Update(TipoNotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoNotaCreditoId", entity.TipoNotaCreditoId);
				parameters.AddParameter("tipoNotaCredito", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("afectaAlmacen", entity.AfectaAlmacen);
				parameters.AddParameter("esIngreso", entity.EsIngreso);
				parameters.AddParameter("permiteEditItem", entity.PermiteEditItem);
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
		public void Delete(TipoNotaCredito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoNotaCreditoId", entity.TipoNotaCreditoId);
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
