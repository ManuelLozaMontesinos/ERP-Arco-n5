using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenSaldoConsolidadoRepository : IMovimientoAlmacenSaldoConsolidadoRepository
	{
		private const string SP_PRE_NAME = "sp_mov_alm_s_con";
		public MovimientoAlmacenSaldoConsolidado GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacenSaldoConsolidado)
					},
					x =>
					{
						MovimientoAlmacenSaldoConsolidado mas = x[0] as MovimientoAlmacenSaldoConsolidado;

						return mas;
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
		public List<MovimientoAlmacenSaldoConsolidado> Search(MovimientoAlmacenSaldoConsolidado entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("producto", entity.Producto?.Codigo);
			parameters.AddParameter("containsDescripcion", entity.Producto?.Descripcion);
			parameters.AddParameter("containsCodigoReferencial", entity.Producto?.CodigoReferencial);
			parameters.AddParameter("mesContableId", entity.MesContableId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacenSaldoConsolidado),
						typeof(Producto),
						typeof(UnidadMedida),
						typeof(MesContable)
					},
					x =>
					{
						MovimientoAlmacenSaldoConsolidado mas = x[0] as MovimientoAlmacenSaldoConsolidado;
						Producto p = x[1] as Producto;
						UnidadMedida um = x[2] as UnidadMedida;
						MesContable mc = x[3] as MesContable;

						mas.Producto = p;
						mas.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						mas.MesContable = mc;
						mas.MesContableId = mc.EntityId;

						return mas;
					},
					"periodo_emp_sede_id,producto_id,unidad_medida_id,mes_contable_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenSaldoConsolidado SingleSearch(MovimientoAlmacenSaldoConsolidado entity)
		{
			try
			{
				List<MovimientoAlmacenSaldoConsolidado> movimientoAlmacenSaldoList = Search(entity);

				return movimientoAlmacenSaldoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacenSaldoConsolidado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("cantidadAnterior", entity.CantidadAnterior);
				parameters.AddParameter("valorUnitarioAnterior", entity.ValorUnitarioAnterior);
				parameters.AddParameter("cantidadIngreso", entity.CantidadIngreso);
				parameters.AddParameter("cantidadSalida", entity.CantidadSalida);
				parameters.AddParameter("cantidadReserva", entity.CantidadReserva);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
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
		public int Update(MovimientoAlmacenSaldoConsolidado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenSaldoId", entity.MovimientoAlmacenSaldoConsolidadoId);
				parameters.AddParameter("cantidadAnterior", entity.CantidadAnterior);
				parameters.AddParameter("valorUnitarioAnterior", entity.ValorUnitarioAnterior);
				parameters.AddParameter("cantidadIngreso", entity.CantidadIngreso);
				parameters.AddParameter("cantidadSalida", entity.CantidadSalida);
				parameters.AddParameter("cantidadReserva", entity.CantidadReserva);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(MovimientoAlmacenSaldoConsolidado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenSaldoId", entity.MovimientoAlmacenSaldoConsolidadoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Valorizar(MovimientoAlmacenSaldoConsolidado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("mesContableId", entity.MesContableId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_cerrarMes"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
