using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AlmacenTransaccionRepository : IAlmacenTransaccionRepository
	{
		private const string SP_PRE_NAME = "sp_alm_transa";
		public AlmacenTransaccion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(AlmacenTransaccion),
						typeof(TransaccionAlmacen)
					},
					x =>
					{
						AlmacenTransaccion at = x[0] as AlmacenTransaccion;
						TransaccionAlmacen ta = x[1] as TransaccionAlmacen;

						at.TransaccionAlmacen = ta;
						at.TransaccionAlmacenId = ta.EntityId;

						return at;
					},
					"trans_alma_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<AlmacenTransaccion> Search(AlmacenTransaccion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
			parameters.AddParameter("tipoMovimientoAlmacenId", entity?.TransaccionAlmacen?.TipoMovimientoAlmacenId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(AlmacenTransaccion),
						typeof(TransaccionAlmacen)
					},
					x =>
					{
						AlmacenTransaccion at = x[0] as AlmacenTransaccion;
						TransaccionAlmacen ta = x[1] as TransaccionAlmacen;

						at.TransaccionAlmacen = ta;
						at.TransaccionAlmacenId = ta.EntityId;

						return at;
					},
					"trans_alma_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public AlmacenTransaccion SingleSearch(AlmacenTransaccion entity)
		{
			try
			{
				List<AlmacenTransaccion> almacenTransaccionList = Search(entity);

				return almacenTransaccionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(AlmacenTransaccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
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
		public int Update(AlmacenTransaccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almaTransaId", entity.AlmacenTransaccionId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
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
		public void Delete(AlmacenTransaccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenTransaccionId", entity.AlmacenTransaccionId);
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
