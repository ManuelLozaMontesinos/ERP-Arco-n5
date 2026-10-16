using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenSaldoRepository : IMovimientoAlmacenSaldoRepository
	{
		private const string SP_PRE_NAME = "sp_mov_alm_saldo";
		public MovimientoAlmacenSaldo GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacenSaldo)
					},
					x =>
					{
						MovimientoAlmacenSaldo mas = x[0] as MovimientoAlmacenSaldo;

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
		public List<MovimientoAlmacenSaldo> Search(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("productoId", entity.ProductoId);
			parameters.AddParameter("containsProducto", entity.Producto?.Codigo);
			parameters.AddParameter("containsDescripcion", entity.Producto?.Descripcion);
			parameters.AddParameter("containsCodigoReferencial", entity.Producto?.CodigoReferencial);
			parameters.AddParameter("mesContableId", entity.MesContableId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacenSaldo),
						typeof(Almacen),
						typeof(Producto),
						typeof(UnidadMedida),
						typeof(MesContable)
					},
					x =>
					{
						MovimientoAlmacenSaldo mas = x[0] as MovimientoAlmacenSaldo;
						Almacen a = x[1] as Almacen;
						Producto p = x[2] as Producto;
						UnidadMedida um = x[3] as UnidadMedida;
						MesContable mc = x[4] as MesContable;

						mas.Almacen = a;
						mas.AlmacenId = a.EntityId;
						mas.Producto = p;
						mas.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						mas.MesContable = mc;
						mas.MesContableId = mc.EntityId;

						return mas;
					},
					"periodo_emp_sede_id,almacen_id,producto_id,unidad_medida_id,mes_contable_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenSaldo SingleSearch(MovimientoAlmacenSaldo entity)
		{
			try
			{
				List<MovimientoAlmacenSaldo> movimientoAlmacenSaldoList = Search(entity);

				return movimientoAlmacenSaldoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
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
		public int Update(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenSaldoId", entity.MovimientoAlmacenSaldoId);
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
		public void Delete(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenSaldoId", entity.MovimientoAlmacenSaldoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenSaldo ObtenerPorEmpresaSede(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("mesContableId", entity.MesContableId);

				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_obtenerPorEmpresaSede"), parameters,
					new[]
					{
						typeof(MovimientoAlmacenSaldo),
						typeof(Producto)
					},
					x =>
					{
						MovimientoAlmacenSaldo mas = x[0] as MovimientoAlmacenSaldo;
						Producto p = x[1] as Producto;

						mas.Producto = p;
						mas.ProductoId = p.EntityId;

						return mas;
					},
					"producto_id"
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCantidadReserva(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenSaldoId", entity.MovimientoAlmacenSaldoId);
				parameters.AddParameter("cantidadReserva", entity.CantidadReserva);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCantidadReserva"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ReprocesarCantidadReserva(MovimientoAlmacenSaldo entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("mesContableId", entity.MesContableId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_reprocesarCantidadReserva"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<dynamic> ListarSaldoMultiAlmacen(MovimientoAlmacenSaldo entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("containsProducto", entity.Producto?.Codigo);
			parameters.AddParameter("containsDescripcion", entity.Producto?.Descripcion);
			parameters.AddParameter("mesContableId", entity.MesContableId);
			parameters.AddParameter("productoRef", entity.Producto?.CodigoReferencial);

			try
			{
				List<dynamic> resultList = DbFactory.Query<dynamic>(DbCommand.Create(SP_PRE_NAME, "_listarSaldoMultiAlmacen"), parameters);

				return resultList;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
