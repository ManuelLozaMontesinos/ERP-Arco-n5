using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenCierreRepository : IMovimientoAlmacenCierreRepository
	{
		private const string SP_PRE_NAME = "sp_mov_alm_cierre";
		public MovimientoAlmacenCierre GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacenCierre)
					},
					x =>
					{
						MovimientoAlmacenCierre mac = x[0] as MovimientoAlmacenCierre;

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
		public List<MovimientoAlmacenCierre> Search(MovimientoAlmacenCierre entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("mesContableId", entity.MesContableId);
			parameters.AddParameter("mesContable", entity?.MesContable?.Codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacenCierre),
						typeof(MesContable),
						typeof(EstadoAperturaCierre)
					},
					x =>
					{
						MovimientoAlmacenCierre mac = x[0] as MovimientoAlmacenCierre;
						MesContable mc = x[1] as MesContable;
						EstadoAperturaCierre eac = x[2] as EstadoAperturaCierre;

						mac.MesContable = mc;
						mac.MesContableId = mc.EntityId;
						mac.EstadoAperturaCierre = eac;
						mac.EstadoAperturaCierreId = eac.EntityId;

						return mac;
					},
					"mes_contable_id,estado_ape_cie_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenCierre SingleSearch(MovimientoAlmacenCierre entity)
		{
			try
			{
				List<MovimientoAlmacenCierre> movAlmCierreList = Search(entity);

				return movAlmCierreList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacenCierre entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("estadoApeCieId", entity.EstadoAperturaCierreId);
				parameters.AddParameter("fechaCierre", entity.FechaCierre);
				parameters.AddParameter("tieneMovPCierre", entity.TieneMovimientoPostCierre);
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
		public int Update(MovimientoAlmacenCierre entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenCierreId", entity.MovimientoAlmacenCierreId);
				parameters.AddParameter("estadoAperturaCierreId", entity.EstadoAperturaCierreId);
				parameters.AddParameter("fechaCierre", entity.FechaCierre);
				parameters.AddParameter("tieneMovimientoPostCierre", entity.TieneMovimientoPostCierre);
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
		public void Delete(MovimientoAlmacenCierre entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenCierreId", entity.MovimientoAlmacenCierreId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void Reprocesar(MovimientoAlmacenCierre entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("mesContableId", entity.MesContableId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_reprocesar"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
