using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PedidoRepository : IPedidoRepository
	{
		private const string SP_PRE_NAME = "sp_pedido";
		public Pedido GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Pedido)
					},
					x =>
					{
						Pedido p = x[0] as Pedido;

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
		public List<Pedido> Search(Pedido entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Pedido)
					},
					x =>
					{
						Pedido p = x[0] as Pedido;

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
		public Pedido SingleSearch(Pedido entity)
		{
			try
			{
				List<Pedido> pedidoList = Search(entity);

				return pedidoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Pedido entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("pedido", entity.Codigo);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTc", entity.MontoTipoCambio);
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("montoAnticipo", entity.MontoAnticipo);
				parameters.AddParameter("cantAprobReq", entity.CantAprobReq);
				parameters.AddParameter("soliAutoDcto", entity.SoliAutoDcto);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("fechaAutoDcto", entity.FechaAutoDcto);
				parameters.AddParameter("exoAutoAde", entity.ExoAutoAde);
				parameters.AddParameter("soliAutoAde", entity.SoliAutoAde);
				parameters.AddParameter("usuAutoAdeId", entity.UsuAutoAdeId);
				parameters.AddParameter("fechaAutoAde", entity.FechaAutoAde);
				parameters.AddParameter("esPedido", entity.EsPedido);
				parameters.AddParameter("tieneComproAnti", entity.TieneComproAnti);
				parameters.AddParameter("estadoPedidoId", entity.EstadoPedidoId);
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
		public int Update(Pedido entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoId", entity.PedidoId);
				parameters.AddParameter("periodoEmpSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("pedido", entity.Codigo);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoTc", entity.MontoTipoCambio);
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("observa", entity.Observacion);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("montoAnticipo", entity.MontoAnticipo);
				parameters.AddParameter("cantAprobReq", entity.CantAprobReq);
				parameters.AddParameter("soliAutoDcto", entity.SoliAutoDcto);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("fechaAutoDcto", entity.FechaAutoDcto);
				parameters.AddParameter("exoAutoAde", entity.ExoAutoAde);
				parameters.AddParameter("soliAutoAde", entity.SoliAutoAde);
				parameters.AddParameter("usuAutoAdeId", entity.UsuAutoAdeId);
				parameters.AddParameter("fechaAutoAde", entity.FechaAutoAde);
				parameters.AddParameter("esPedido", entity.EsPedido);
				parameters.AddParameter("tieneComproAnti", entity.TieneComproAnti);
				parameters.AddParameter("estadoPedidoId", entity.EstadoPedidoId);
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
		public void Delete(Pedido entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoId", entity.PedidoId);
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
