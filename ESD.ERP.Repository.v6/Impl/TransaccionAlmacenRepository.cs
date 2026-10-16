using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TransaccionAlmacenRepository : ITransaccionAlmacenRepository
	{
		private const string SP_PRE_NAME = "sp_trans_alma";
		public TransaccionAlmacen GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TransaccionAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(OrigenRegistro),
						typeof(TipoOperacion)
					},
					x =>
					{
						TransaccionAlmacen ta = x[0] as TransaccionAlmacen;
						TipoMovimientoAlmacen tma = x[1] as TipoMovimientoAlmacen;
						OrigenRegistro ori = x[2] as OrigenRegistro;
						TipoOperacion tpe = x[3] as TipoOperacion;

						ta.TipoMovimientoAlmacen = tma;
						ta.TipoMovimientoAlmacenId = tma?.EntityId;
						ta.OrigenRegistro = ori;
						ta.OrigenRegistroId = ori.EntityId;
						ta.TipoOperacion = tpe;
						ta.TipoOperacionId = tpe?.EntityId;

						return ta;
					},
					"tipo_mov_alma_id,origen_registro_id,tipo_operacion_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<TransaccionAlmacen> Search(TransaccionAlmacen entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("transaccionAlmacen", entity.Codigo);
			parameters.AddParameter("notTransaccionAlmacenList", entity.NotTransaccionAlmacenList);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
			parameters.AddParameter("notTipoMovimientoAlmacenId", entity.NotTipoMovimientoAlmacenId);
			parameters.AddParameter("tipoMovimientoAlmacen", entity?.TipoMovimientoAlmacen?.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TransaccionAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(OrigenRegistro),
						typeof(TipoOperacion)
					},
					x =>
					{
						TransaccionAlmacen ta = x[0] as TransaccionAlmacen;
						TipoMovimientoAlmacen tma = x[1] as TipoMovimientoAlmacen;
						OrigenRegistro ori = x[2] as OrigenRegistro;
						TipoOperacion tpe = x[3] as TipoOperacion;

						ta.TipoMovimientoAlmacen = tma;
						ta.TipoMovimientoAlmacenId = tma?.EntityId;
						ta.OrigenRegistro = ori;
						ta.OrigenRegistroId = ori.EntityId;
						ta.TipoOperacion = tpe;
						ta.TipoOperacionId = tpe?.EntityId;

						return ta;
					},
					"tipo_mov_alma_id,origen_registro_id,tipo_operacion_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TransaccionAlmacen SingleSearch(TransaccionAlmacen entity)
		{
			try
			{
				List<TransaccionAlmacen> transaccionAlmacenList = Search(entity);

				return transaccionAlmacenList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TransaccionAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("transaccionAlmacen", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
				parameters.AddParameter("origenRegistroId", entity.OrigenRegistroId);
				parameters.AddParameter("esConfigurable", entity.EsConfigurable);
				parameters.AddParameter("esCompra", entity.EsCompra);
				parameters.AddParameter("tieneMantenimiento", entity.TieneMantenimiento);
				parameters.AddParameter("tipoOperacionId", entity.TipoOperacionId);
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
		public int Update(TransaccionAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
				parameters.AddParameter("esCompra", entity.EsCompra);
				parameters.AddParameter("tipoOperacionId", entity.TipoOperacionId);
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
		public void Delete(TransaccionAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
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
