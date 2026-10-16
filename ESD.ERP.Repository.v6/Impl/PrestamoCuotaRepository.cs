using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class PrestamoCuotaRepository : IPrestamoCuotaRepository
	{
		private const string SP_PRE_NAME = "sp_presta_cuota";
		public PrestamoCuota GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PrestamoCuota)
					},
					x =>
					{
						PrestamoCuota pc = x[0] as PrestamoCuota;

						return pc;
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
		public List<PrestamoCuota> Search(PrestamoCuota entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("prestamoPersonalId", entity.PrestamoPersonalId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PrestamoCuota),
						typeof(TipoCancelacionCuota),
						typeof(EstadoCuota)
					},
					x =>
					{
						PrestamoCuota pc = x[0] as PrestamoCuota;
						TipoCancelacionCuota tcc = x[1] as TipoCancelacionCuota;
						EstadoCuota ec = x[2] as EstadoCuota;

						pc.TipoCancelacionCuota = tcc;
						pc.TipoCancelacionCuotaId = tcc.EntityId;
						pc.EstadoCuota = ec;
						pc.EstadoCuotaId = ec.EntityId;

						return pc;
					},
					"tipo_can_cuota_id,estado_cuota_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PrestamoCuota SingleSearch(PrestamoCuota entity)
		{
			try
			{
				List<PrestamoCuota> prestamoCuotaList = Search(entity);

				return prestamoCuotaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PrestamoCuota entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoPersonalId", entity.PrestamoPersonalId);
				parameters.AddParameter("cuota", entity.Cuota);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("cechaCancelacion", entity.FechaCancelacion);
				parameters.AddParameter("tipoCancelacionCuotaId", entity.TipoCancelacionCuotaId);
				parameters.AddParameter("estadoCuotaId", entity.EstadoCuotaId);
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
		public int Update(PrestamoCuota entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoCuotaId", entity.PrestamoCuotaId);
				parameters.AddParameter("cuota", entity.Cuota);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("fechaCancelacion", entity.FechaCancelacion);
				parameters.AddParameter("tipoCancelacionCuotaId", entity.TipoCancelacionCuotaId);
				parameters.AddParameter("estadoCuotaId", entity.EstadoCuotaId);
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
		public void Delete(PrestamoCuota entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("prestamoCuotaId", entity.PrestamoCuotaId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void BulkMerge(List<PrestamoCuota> list)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				List<PrestamoCuotaType> prestamoCuotaTypeList = new List<PrestamoCuotaType>();
				prestamoCuotaTypeList = (from s in list
														 select new PrestamoCuotaType
														 {
															 PrestamoCuotaId = s.PrestamoCuotaId,
															 PrestamoPersonalId = s.PrestamoPersonalId,
															 Cuota = s.Cuota,
															 Monto = s.Monto,
															 FechaVencimiento = s.FechaVencimiento,
															 TipoCancelacionCuotaId = s.TipoCancelacionCuotaId,
															 EstadoCuotaId = s.EstadoCuotaId,
															 Estado = s.Estado,
															 UsuarioCreacionId = s.UsuarioCreacionId,
														 }).ToList();

				parameters.AddTableParameter("prestamoCuotaList", "dbo.PrestamoCuotaType", prestamoCuotaTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_procesarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
