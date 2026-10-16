using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PedidoProductoRepository : IPedidoProductoRepository
	{
		private const string SP_PRE_NAME = "sp_pedido_prod";
		public PedidoProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PedidoProducto)
					},
					x =>
					{
						PedidoProducto pp = x[0] as PedidoProducto;

						return pp;
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
		public List<PedidoProducto> Search(PedidoProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PedidoProducto)
					},
					x =>
					{
						PedidoProducto pp = x[0] as PedidoProducto;

						return pp;
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
		public PedidoProducto SingleSearch(PedidoProducto entity)
		{
			try
			{
				List<PedidoProducto> pedidoProductoList = Search(entity);

				return pedidoProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PedidoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoId", entity.PedidoId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("cantPrese", entity.CantidadPresentacion);
				parameters.AddParameter("cantEquiv", entity.CantidadEquivalente);
				parameters.AddParameter("cantCompro", entity.CantidadComprobante);
				parameters.AddParameter("valorULista", entity.ValorUnitarioLista);
				parameters.AddParameter("precioULista", entity.PrecioUnitarioLista);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("incDctoLp", entity.IncDctoLp);
				parameters.AddParameter("soliAutoDcto", entity.SoliAutoDcto);
				parameters.AddParameter("tieneSaldo", entity.TieneSaldo);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoDcto1", entity.MontoDescuento1);
				parameters.AddParameter("montoDcto2", entity.MontoDescuento2);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIscId", entity.TipoIscId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("reclasifica", entity.Reclasifica);
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
		public int Update(PedidoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoProductoId", entity.PedidoProdId);
				parameters.AddParameter("pedidoId", entity.PedidoId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("cantPrese", entity.CantidadPresentacion);
				parameters.AddParameter("cantEquiv", entity.CantidadEquivalente);
				parameters.AddParameter("cantCompro", entity.CantidadComprobante);
				parameters.AddParameter("valorULista", entity.ValorUnitarioLista);
				parameters.AddParameter("precioULista", entity.PrecioUnitarioLista);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("incDctoLp", entity.IncDctoLp);
				parameters.AddParameter("soliAutoDcto", entity.SoliAutoDcto);
				parameters.AddParameter("tieneSaldo", entity.TieneSaldo);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoDcto1", entity.MontoDescuento1);
				parameters.AddParameter("montoDcto2", entity.MontoDescuento2);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIscId", entity.TipoIscId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("reclasifica", entity.Reclasifica);
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
		public void Delete(PedidoProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoProductoId", entity.PedidoProdId);
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
