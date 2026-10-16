using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenCorrelativoRepository : IMovimientoAlmacenCorrelativoRepository
	{
		private const string SP_PRE_NAME = "sp_mov_alm_corre";
		public MovimientoAlmacenCorrelativo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacenCorrelativo)
					},
					x =>
					{
						MovimientoAlmacenCorrelativo mac = x[0] as MovimientoAlmacenCorrelativo;

						return mac;
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
		public List<MovimientoAlmacenCorrelativo> Search(MovimientoAlmacenCorrelativo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacenCorrelativo),
						typeof(TipoMovimientoAlmacen)
					},
					x =>
					{
						MovimientoAlmacenCorrelativo mac = x[0] as MovimientoAlmacenCorrelativo;
						TipoMovimientoAlmacen tma = x[1] as TipoMovimientoAlmacen;

						mac.TipoMovimientoAlmacen = tma;
						mac.TipoMovimientoAlmacenId = tma.EntityId;

						return mac;
					},
					"tipo_mov_alma_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenCorrelativo SingleSearch(MovimientoAlmacenCorrelativo entity)
		{
			try
			{
				List<MovimientoAlmacenCorrelativo> movimientoAlmacenCorrelativoList = Search(entity);

				return movimientoAlmacenCorrelativoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacenCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
				parameters.AddParameter("correlativo", entity.Correlativo);
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
		public int Update(MovimientoAlmacenCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenCorrelativoId", entity.MovimientoAlmacenCorrelativoId);
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
		public void Delete(MovimientoAlmacenCorrelativo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenCorrelativoId", entity.MovimientoAlmacenCorrelativoId);
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
