using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using System.Linq;
using ESD.ERP.Business.Resources;

namespace ESD.ERP.Business.Impl
{
	public class ComprobanteProductoBusiness : IComprobanteProductoBusiness
	{
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;
		private readonly IComprobanteRepository comprobanteRepository;
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;
		private readonly IMesContableRepository mesContableRepository;
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly IComboProductoRepository comboProductoRepository;
		private readonly IListaPrecioProductoRepository listaPrecioProductoRepository;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly ITipoDescuentoRepository tipoDescuentoRepository;
		private readonly IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository;

		public ComprobanteProductoBusiness(
			IComprobanteProductoRepository comprobanteProductoRepository,
			IComprobanteRepository comprobanteRepository,
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository,
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness,
			IMesContableRepository mesContableRepository,
			ICotizacionProductoRepository cotizacionProductoRepository,
			IComboProductoRepository comboProductoRepository,
			IListaPrecioProductoRepository listaPrecioProductoRepository,
			IEstadoDespachoRepository estadoDespachoRepository,
			ITipoDescuentoRepository tipoDescuentoRepository,
			IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository
			)
		{
			this.comprobanteProductoRepository = comprobanteProductoRepository;
			this.comprobanteRepository = comprobanteRepository;
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
			this.mesContableRepository = mesContableRepository;
			this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.comboProductoRepository = comboProductoRepository;
			this.listaPrecioProductoRepository = listaPrecioProductoRepository;
			this.estadoDespachoRepository = estadoDespachoRepository;
			this.tipoDescuentoRepository = tipoDescuentoRepository;
			this.comprobanteProductoEntregaRepository = comprobanteProductoEntregaRepository;
		}
		public ComprobanteProductoDTO GetById(int id)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = comprobanteProductoRepository.GetById(id);

				ComprobanteProductoDTO comprobanteProductoDTO = Mapper.Map<ComprobanteProductoDTO>(comprobanteProducto);

				return comprobanteProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteProductoDTO> Search(ComprobanteProductoDTO dto)
		{
			try
			{
				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(Mapper.Map<ComprobanteProducto>(dto));

				List<ComprobanteProductoDTO> comprobanteProductoDTOList = Mapper.Map<List<ComprobanteProductoDTO>>(comprobanteProductoList);

				return comprobanteProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteProductoDTO SingleSearch(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = comprobanteProductoRepository.SingleSearch(Mapper.Map<ComprobanteProducto>(dto));

				ComprobanteProductoDTO comprobanteProductoDTO = Mapper.Map<ComprobanteProductoDTO>(comprobanteProducto);

				return comprobanteProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = comprobanteProductoRepository.Insert(comprobanteProducto);
					comprobanteProducto.ComprobanteProductoId = newId;

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
						.EsCombo == SiNoConstante.N)
						actualizarReserva(comprobanteProducto, true);
               else
						actualizarComboReserva(comprobanteProducto, true);

					dto.ComprobanteProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{

					if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
						.EsCombo == SiNoConstante.N)
						actualizarReserva(comprobanteProducto, false);
					else
						actualizarComboReserva(comprobanteProducto, false);

					id = comprobanteProductoRepository.Update(comprobanteProducto);

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					comprobanteProducto = comprobanteProductoRepository.GetById(comprobanteProducto.EntityId);

					if (comprobanteProducto.Comprobante.CotizacionId.HasValue &&
						comprobanteProducto.RegistroReferencialId.HasValue)
					{
						cotizacionProductoRepository.ActualizarCantidadComprobante(new CotizacionProducto()
						{
							CotizacionId = comprobanteProducto.Comprobante.CotizacionId.Value,
							CotizacionProductoId = comprobanteProducto.RegistroReferencialId.Value
						});
					}

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteProductoRepository.Delete(comprobanteProducto);

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto
						.EsCombo == SiNoConstante.N)
						eliminarReserva(comprobanteProducto);
					else
						eliminarComboReserva(comprobanteProducto);

					comprobanteProducto = comprobanteProductoRepository.GetById(comprobanteProducto.EntityId);

               if (comprobanteProducto.Comprobante.CotizacionId.HasValue &&
						comprobanteProducto.RegistroReferencialId.HasValue)
               {
						cotizacionProductoRepository.ActualizarCantidadComprobante(new CotizacionProducto()
						{
							CotizacionId = dto.ComprobanteDTO.CotizacionId.Value,
							CotizacionProductoId = comprobanteProducto.RegistroReferencialId.Value
						});
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarReserva(ComprobanteProducto comprobanteProducto, bool esNuevo)
      {

			decimal cantidad = comprobanteProducto.Cantidad;
			decimal cantidadAnterior = decimal.Zero;

			comprobanteProducto = comprobanteProductoRepository.GetById(comprobanteProducto.EntityId);

			if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
				return;

			if (comprobanteProducto.Comprobante.CotizacionId.HasValue &&
				comprobanteProducto.RegistroReferencialId.HasValue)
				return;

			if (!esNuevo)
				cantidadAnterior = comprobanteProducto.Cantidad;

			MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, comprobanteProducto.Comprobante.Fecha));

			movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(new MovimientoAlmacenSaldoDTO() {
				PeriodoEmpresaSedeId = comprobanteProducto.Comprobante.PeriodoEmpresaSedeId,
				ProductoId = comprobanteProducto.ProductoId,
				MesContableId = mesContable.MesContableId,
				CantidadAnterior = cantidadAnterior,
				CantidadIngreso = cantidad
			});

			MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
			{
				PeriodoEmpresaSedeId = comprobanteProducto.Comprobante.PeriodoEmpresaSedeId,
				AlmacenId = comprobanteProducto.Comprobante.AlmacenId.Value,
				ProductoId = comprobanteProducto.ProductoId,
				MesContableId = mesContable.MesContableId
			});

			if(movimientoAlmacenSaldo == null)
         {
				movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = comprobanteProducto.Comprobante.PeriodoEmpresaSedeId,
					AlmacenId = comprobanteProducto.Comprobante.AlmacenId.Value,
					ProductoId = comprobanteProducto.ProductoId,
					MesContableId = mesContable.MesContableId,
					CantidadReserva = cantidad
				};

				movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
         }
         else
         {
				movimientoAlmacenSaldo.CantidadReserva -= cantidadAnterior;
				movimientoAlmacenSaldo.CantidadReserva += cantidad;

				movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);
			}

		}
		private void actualizarComboReserva(ComprobanteProducto comprobanteProducto, bool esNuevo)
      {
			List<ComprobanteProducto> comprobanteProductoList = new List<ComprobanteProducto>();

			List<ComboProducto> comboProductoList = comboProductoRepository.Search(new ComboProducto()
			{
				ProductoId = comprobanteProducto.ProductoId,
				Estado = EstadoConstante.ACTIVO
			});

			Comprobante comprobante = comprobanteRepository.GetById(comprobanteProducto.ComprobanteId);

			if (esNuevo)
         {
				comprobanteProductoList = crearComprobanteProductoDesdeCombo(comboProductoList, comprobante, comprobanteProducto);

				foreach (ComprobanteProducto comprobanteProductoTemp in comprobanteProductoList) {
					int newId = comprobanteProductoRepository.Insert(comprobanteProductoTemp);
					comprobanteProductoTemp.ComprobanteProductoId = newId;

					actualizarReserva(comprobanteProductoTemp, esNuevo);
				}
         }
         else{
				comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto() { 
					ComprobanteId = comprobanteProducto.ComprobanteId,
					RegistroComboId = comprobanteProducto.EntityId,
					Estado = EstadoConstante.ACTIVO
				});

				foreach(ComboProducto comboProducto in comboProductoList)
            {

					ComprobanteProducto comprobanteProductoTemp = comprobanteProductoList.
						Where(x => x.ProductoId == comboProducto.ProductoIncluidoId).FirstOrDefault();

					CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
					{
						PrecioUnitario = comprobanteProductoTemp.PrecioUnitario,
						Cantidad = comprobanteProducto.Cantidad * comboProducto.Cantidad,
						TipoIGV = comboProducto.ProductoIncluido.TipoIgv.Codigo,
						ImpuestoTasa = comprobante.ImpuestoTasa.Tasa
					});

					if(string.IsNullOrEmpty(comprobanteProductoTemp.PermiteDescuento))
						comprobanteProductoTemp.PermiteDescuento = SiNoConstante.N;
					comprobanteProductoTemp.Cantidad = comprobanteProducto.Cantidad * comboProducto.Cantidad;
					comprobanteProductoTemp.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
					comprobanteProductoTemp.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
					comprobanteProductoTemp.MontoBruto = calculadorVentaItemDTO.MontoBruto;
					comprobanteProductoTemp.MontoNeto = calculadorVentaItemDTO.MontoNeto;
					comprobanteProductoTemp.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
					comprobanteProductoTemp.MontoTotal = calculadorVentaItemDTO.MontoTotal;
					comprobanteProductoTemp.UsuarioCreacionId = comprobanteProducto.UsuarioCreacionId;

					actualizarReserva(comprobanteProductoTemp, esNuevo);

					comprobanteProductoRepository.Update(comprobanteProductoTemp);
				}
			}
		}
		private List<ComprobanteProducto> crearComprobanteProductoDesdeCombo(List<ComboProducto> comboProductoList, Comprobante comprobante, 
			ComprobanteProducto comprobanteProducto)
      {
			List<ComprobanteProducto> comprobanteProductoList = new List<ComprobanteProducto>();

			foreach (ComboProducto comboProducto in comboProductoList)
			{

				ListaPrecioProducto listaPrecioProducto = listaPrecioProductoRepository.SingleSearch(new ListaPrecioProducto()
				{
					ListaPrecioId = comprobante.ListaPrecioId,
					ProductoId = comboProducto.ProductoIncluidoId,
					Estado = EstadoConstante.ACTIVO
				});

				decimal precioUnitario = decimal.Round((listaPrecioProducto.PrecioUnitario * comboProducto.PorcentajePrecio) / 100,
					comboProducto.NumeroDecimalesRedondeo);

				CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
				{
					PrecioUnitario = precioUnitario,
					Cantidad = comprobanteProducto.Cantidad * comboProducto.Cantidad,
					TipoIGV = comboProducto.ProductoIncluido.TipoIgv.Codigo,
					ImpuestoTasa = listaPrecioProducto.ListaPrecio.ImpuestoTasa.Tasa
				});

				TipoDescuento tipoDescuento = tipoDescuentoRepository.ObtenerPorCodigo(TipoDescuentoConstante.SIN_DESCUENTO);

				comprobanteProductoList.Add(new ComprobanteProducto()
				{
					ComprobanteId = comprobanteProducto.ComprobanteId,
					Producto = comboProducto.ProductoIncluido,
					ProductoId = comboProducto.ProductoIncluidoId,
					Descripcion = string.Empty,
					Cantidad = comprobanteProducto.Cantidad * comboProducto.Cantidad,
					ValorUnitarioLista = listaPrecioProducto.ValorUnitario,
					PrecioUnitarioLista = listaPrecioProducto.PrecioUnitario,
					ValorUnitario = calculadorVentaItemDTO.ValorUnitario,
					PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario,
					CantidadBloqueada = SiNoConstante.N,
					IncluyeDescuentoListaPrecio = SiNoConstante.N,
					SolicitaAutorizacionDescuento = SiNoConstante.N,
					PermiteDescuento = SiNoConstante.N,
					MontoBruto = calculadorVentaItemDTO.MontoBruto,
					TipoDescuentoId = tipoDescuento.EntityId,
					MontoNeto = calculadorVentaItemDTO.MontoNeto,
					MontoIgv = calculadorVentaItemDTO.MontoImpuesto,
					MontoTotal = calculadorVentaItemDTO.MontoTotal,
					TipoDetraccionId = comboProducto.ProductoIncluido.FamiliaProducto.ClaseProducto.TipoDetraccionId,
					TipoAfectacionIgvId = comprobanteProducto.TipoAfectacionIgvId,
					RegistroComboId = comprobanteProducto.ComprobanteProductoId,
					ObservacionDespacho = string.Empty,
					EntregaDespacho = string.Empty,
					Estado = comprobanteProducto.Estado,
					UsuarioCreacionId = comprobanteProducto.UsuarioCreacionId
				});
			}

			return comprobanteProductoList;
		}
		private void eliminarReserva(ComprobanteProducto comprobanteProducto)
		{

			decimal cantidadAnterior = comprobanteProducto.Cantidad;

			comprobanteProducto = comprobanteProductoRepository.GetById(comprobanteProducto.EntityId);

			if (comprobanteProducto.Producto.FamiliaProducto.ClaseProducto.
				GrupoProducto.TipoProducto.Codigo == TipoProductoConstante.SERVICIO)
				return;

			if (comprobanteProducto.Comprobante.CotizacionId.HasValue &&
				comprobanteProducto.RegistroReferencialId.HasValue)
				return;

			MesContable mesContable = mesContableRepository.ObtenerPorCodigo(string.Format(StringFormatConstant.DATE_MONTH, comprobanteProducto.Comprobante.Fecha));

			MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
			{
				PeriodoEmpresaSedeId = comprobanteProducto.Comprobante.PeriodoEmpresaSedeId,
				AlmacenId = comprobanteProducto.Comprobante.AlmacenId.Value,
				ProductoId = comprobanteProducto.ProductoId,
				MesContableId = mesContable.MesContableId
			});

			movimientoAlmacenSaldo.CantidadReserva -= cantidadAnterior;

			movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);
		}
		private void eliminarComboReserva(ComprobanteProducto comprobanteProducto)
		{
			List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
			{
				ComprobanteId = comprobanteProducto.ComprobanteId,
				RegistroComboId = comprobanteProducto.EntityId,
				Estado = EstadoConstante.ACTIVO
			});

			foreach (ComprobanteProducto comprobanteProductoTemp in comprobanteProductoList)
         {
				comprobanteProductoRepository.Delete(comprobanteProductoTemp);

				eliminarReserva(comprobanteProductoTemp);
			}
		}
		public void AgregarCotizacionProducto(ComprobanteProductoDTO dto)
      {
			try
			{
				Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);

				using (TransactionScope tx = new())
				{
					List<CotizacionProducto> cotizacionProductoList = cotizacionProductoRepository.Search(new CotizacionProducto() { 
						CotizacionId = dto.ComprobanteDTO.CotizacionId.Value,
						CotizacionProductoIdList = dto.CotizacionProductoIdList
					});

					foreach(CotizacionProducto cotizacionProducto in cotizacionProductoList)
               {
						ComprobanteProducto comprobanteProducto = new()
						{
							ComprobanteId = dto.ComprobanteId,
							ProductoId = cotizacionProducto.ProductoId,
							Descripcion = cotizacionProducto.Descripcion,
							Cantidad = (cotizacionProducto.Cantidad - cotizacionProducto.CantidadComprobante),
							UnidadMedidaId = cotizacionProducto.UnidadMedidaId,
							CantidadPresentacion = cotizacionProducto.CantidadPresentacion,
							CantidadEquivalente = cotizacionProducto.CantidadEquivalente,
							ValorUnitarioLista = cotizacionProducto.ValorUnitarioLista,
							PrecioUnitarioLista = cotizacionProducto.PrecioUnitarioLista,
							ValorUnitario = cotizacionProducto.ValorUnitario,
							PrecioUnitario = cotizacionProducto.PrecioUnitario,
							IncluyeDescuentoListaPrecio = cotizacionProducto.IncluyeDescuentoListaPrecio,
							PermiteDescuento = cotizacionProducto.PermiteDescuento,
							CantidadBloqueada = cotizacionProducto.CantidadBloqueada,
							SolicitaAutorizacionDescuento = SiNoConstante.N,
							RegistroReferencialId = cotizacionProducto.EntityId,
							MontoBruto = cotizacionProducto.MontoBruto,
							TipoDescuento = cotizacionProducto.TipoDescuento,
							TipoDescuentoId = cotizacionProducto.TipoDescuentoId,
							DescuentoValor = cotizacionProducto.DescuentoValor,
							PorcentajeDescuento = cotizacionProducto.PorcentajeDescuento,
							MontoDescuento1 = cotizacionProducto.MontoDescuento1,
							MontoDescuento2 = cotizacionProducto.MontoDescuento2,
							MontoNeto = cotizacionProducto.MontoNeto,
							MontoIgv = cotizacionProducto.MontoIgv,
							MontoIsc = cotizacionProducto.MontoIsc,
							MontoTotal = cotizacionProducto.MontoTotal,
							TipoDetraccionId = cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.TipoDetraccionId,
							PorcentajeDetraccion = cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.Tasa,
							MontoDetraccion = CalculadorVenta.CalcularDetraccion(cotizacionProducto.MontoTotal,
										cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.TipoDetraccion.Tasa),
							TipoAfectacionIgvId = cotizacionProducto.TipoAfectacionIgvId,
							ObservacionDespacho = string.Empty,
							EntregaDespacho = string.Empty,
							Estado = EstadoConstante.ACTIVO,
							UsuarioCreacionId = dto.UsuarioCreacionId
						};

						CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
						{
							PrecioUnitario = comprobanteProducto.PrecioUnitario,
							Cantidad = comprobanteProducto.Cantidad,
							TipoIGV = cotizacionProducto.TipoAfectacionIgv.TipoIgv.Codigo,
							ImpuestoTasa = comprobante.ImpuestoTasa.Tasa,
							DetraccionTasa = comprobanteProducto.PorcentajeDetraccion,
							TipoDescuento = comprobanteProducto.TipoDescuento.Codigo,
							DescuentoValor = comprobanteProducto.DescuentoValor
						});

						comprobanteProducto.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
						comprobanteProducto.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
						comprobanteProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
						comprobanteProducto.MontoNeto = calculadorVentaItemDTO.MontoNeto;
						comprobanteProducto.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
						comprobanteProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;
						comprobanteProducto.MontoDetraccion = calculadorVentaItemDTO.MontoDetraccion;

						int newId = comprobanteProductoRepository.Insert(comprobanteProducto);
						comprobanteProducto.ComprobanteProductoId = newId;

						if (cotizacionProducto.Producto.FamiliaProducto.ClaseProducto.GrupoProducto.EsCombo == SiNoConstante.S)
                  {
							List<ComboProducto> comboProductoList = comboProductoRepository.Search(new ComboProducto()
							{
								ProductoId = comprobanteProducto.ProductoId,
								Estado = EstadoConstante.ACTIVO
							});

							List<ComprobanteProducto> comprobanteProductoList =crearComprobanteProductoDesdeCombo(comboProductoList, comprobante, 
								comprobanteProducto);

							foreach (ComprobanteProducto comprobanteProductoTemp in comprobanteProductoList)
								comprobanteProductoRepository.Insert(comprobanteProductoTemp);
						}

						cotizacionProductoRepository.ActualizarCantidadComprobante(new CotizacionProducto() { 
							CotizacionId = dto.ComprobanteDTO.CotizacionId.Value,
							CotizacionProductoId = cotizacionProducto.EntityId
						});
					}

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<ComprobanteProductoDTO> ListarPendienteCanje(ComprobanteProductoDTO dto)
      {
			try
			{
				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.ListarPendienteCanje(Mapper.Map<ComprobanteProducto>(dto));

				List<ComprobanteProductoDTO> comprobanteProductoDTOList = Mapper.Map<List<ComprobanteProductoDTO>>(comprobanteProductoList);

				return comprobanteProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
	  }
      public List<ComprobanteProductoDTO> ListarPorIdList(ComprobanteProductoDTO dto)
      {
			try
			{
				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.ListarPorIdList(Mapper.Map<ComprobanteProducto>(dto));

				List<ComprobanteProductoDTO> comprobanteProductoDTOList = Mapper.Map<List<ComprobanteProductoDTO>>(comprobanteProductoList);

				return comprobanteProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void AgregarComprobanteProductoCanjeSelectivo(ComprobanteProductoDTO dto)
      {
			try
			{
				Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.ListarPorIdList(new ComprobanteProducto()
				{
					ComprobanteProductoIdList = dto.ComprobanteProductoIdList
				});

				int totalGrupo = comprobanteProductoList.GroupBy(x => x.ProductoId).Count();
				if (totalGrupo > 1)
					throw new Exception(SR.GetString("EX_comprobante_canje_productos_no_iguales"));

				TipoDescuento tipoDescuento = tipoDescuentoRepository.ObtenerPorCodigo(TipoDescuentoConstante.SIN_DESCUENTO);

				using (TransactionScope tx = new())
				{

					ComprobanteProducto comprobanteProductoTmp = comprobanteProductoList.FirstOrDefault();

					ComprobanteProducto comprobanteProducto = new()
					{
						ComprobanteId = dto.ComprobanteId,
						ProductoId = comprobanteProductoTmp.ProductoId,
						Descripcion = string.Empty,
						Cantidad = comprobanteProductoList.Sum(x => x.Cantidad),
						ValorUnitarioLista = comprobanteProductoList.Min(x => x.ValorUnitarioLista),
						PrecioUnitarioLista = comprobanteProductoList.Min(x => x.PrecioUnitarioLista),
						ValorUnitario = comprobanteProductoList.Min(x => x.ValorUnitario),
						PrecioUnitario = comprobanteProductoList.Min(x => x.PrecioUnitario),
						IncluyeDescuentoListaPrecio = SiNoConstante.N,
						CantidadBloqueada = SiNoConstante.N,
						SolicitaAutorizacionDescuento = SiNoConstante.N,
						PermiteDescuento = SiNoConstante.N,
						TipoDetraccionId = comprobanteProductoTmp.TipoDetraccionId,
						PorcentajeDetraccion = comprobanteProductoTmp.PorcentajeDetraccion,
						TipoAfectacionIgvId = comprobanteProductoTmp.TipoAfectacionIgvId,
						ObservacionDespacho = string.Empty,
						EntregaDespacho = string.Empty,
						Estado = EstadoConstante.ACTIVO,
						UsuarioCreacionId = dto.UsuarioCreacionId
					};

					CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
					{
						PrecioUnitario = comprobanteProducto.PrecioUnitario,
						Cantidad = comprobanteProducto.Cantidad,
						TipoIGV = comprobanteProductoTmp.TipoAfectacionIgv.TipoIgv.Codigo,
						ImpuestoTasa = comprobante.ImpuestoTasa.Tasa,
						DetraccionTasa = comprobanteProducto.PorcentajeDetraccion
					});

					comprobanteProducto.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
					comprobanteProducto.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
					comprobanteProducto.MontoBruto = calculadorVentaItemDTO.MontoBruto;
					comprobanteProducto.TipoDescuentoId = tipoDescuento.EntityId;
					comprobanteProducto.MontoNeto = calculadorVentaItemDTO.MontoNeto;
					comprobanteProducto.MontoIgv = calculadorVentaItemDTO.MontoImpuesto;
					comprobanteProducto.MontoTotal = calculadorVentaItemDTO.MontoTotal;
					comprobanteProducto.MontoDetraccion = calculadorVentaItemDTO.MontoDetraccion;

					int newId = comprobanteProductoRepository.Insert(comprobanteProducto);

					foreach(ComprobanteProducto comprobanteProductoTmp2 in comprobanteProductoList)
						comprobanteProductoRepository.ActualizarRegistroCanjeId(new ComprobanteProducto()
						{
							ComprobanteProductoId = comprobanteProductoTmp2.ComprobanteProductoId,
							RegistroCanjeId = newId
						});

               comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void AgregarComprobanteProductoCanje(ComprobanteProductoDTO dto)
		{
			try
			{
				Comprobante comprobante = comprobanteRepository.GetById(dto.ComprobanteId);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.ListarPorIdList(new ComprobanteProducto()
				{
					ComprobanteProductoIdList = dto.ComprobanteProductoIdList
				});

				using (TransactionScope tx = new())
				{
					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
               {
						int comprobanteProductoId = comprobanteProducto.EntityId;

						comprobanteProducto.ComprobanteId = dto.ComprobanteId;
						comprobanteProducto.RegistroReferencialId = null;
						comprobanteProducto.RegistroCanjeId = null;
						comprobanteProducto.UsuarioCreacionId = dto.UsuarioCreacionId;

						int newId = comprobanteProductoRepository.Insert(comprobanteProducto);

						comprobanteProductoRepository.ActualizarRegistroCanjeId(new ComprobanteProducto()
						{
							ComprobanteProductoId = comprobanteProductoId,
							RegistroCanjeId = newId
						});
					}

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int ActualizarCanje(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{

					id = comprobanteProductoRepository.Update(comprobanteProducto);

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void EliminarCanje(ComprobanteProductoDTO dto)
		{
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteProductoRepository.Delete(comprobanteProducto);

					comprobanteProductoRepository.ActualizarNuloRegistroCanjeId(new ComprobanteProducto()
					{
						ComprobanteProductoId = dto.ComprobanteProductoId,
					});

					comprobanteRepository.ActualizarMonto(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarCantidadEntregada(ComprobanteProductoDTO dto)
      {
			try
			{
				ComprobanteProducto comprobanteProducto = Mapper.Map<ComprobanteProducto>(dto);

				EstadoDespacho estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.ATENDIDO_PARCIAL);

				using (TransactionScope tx = new())
				{

					comprobanteProductoRepository.ActualizarCantidadEntregada(comprobanteProducto);

					comprobanteRepository.ActualizarEstadoDespacho(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId,
						EstadoDespachoId = estadoDespacho.EntityId,
						FechaDespacho = null,
						ObservacionDespacho = string.Empty,
						EntregaDespacho = string.Empty,
					});
					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void AgregarComprobanteProductoEntrega(ComprobanteProductoDTO dto)
		{
			try
			{
				Comprobante comprobante = Mapper.Map<Comprobante>(dto);

				List<ComprobanteProducto> comprobanteProductoList = comprobanteProductoRepository.Search(new ComprobanteProducto()
				{
					ComprobanteId = dto.ComprobanteId,
					NullRegistroComboId = SiNoConstante.S,
					ComprobanteProductoIdList = dto.ComprobanteProductoIdList,
					Estado = EstadoConstante.ACTIVO
				});

				DateTime fechaDespacho = DateTime.Now;

				using (TransactionScope tx = new())
				{

					foreach (ComprobanteProducto comprobanteProducto in comprobanteProductoList)
					{
						comprobanteProductoEntregaRepository.Insert(new ComprobanteProductoEntrega()
						{
							ComprobanteProductoId = comprobanteProducto.EntityId,
							Cantidad = comprobanteProducto.CantidadPresentacion == decimal.Zero ?
														comprobanteProducto.Cantidad : comprobanteProducto.CantidadPresentacion,
							Fecha = comprobanteProducto.FechaDespacho.HasValue ? comprobanteProducto.FechaDespacho.Value : fechaDespacho,
							EntregaDespacho = dto.EntregaDespacho,
							Observacion = comprobanteProducto.ObservacionDespacho,
							PersonalId = comprobanteProducto.PersonalId == null ? dto.PersonalId.Value : comprobanteProducto.PersonalId.Value
						});

						comprobanteProductoRepository.ActualizarCantidadEntregada(new ComprobanteProducto()
						{
							ComprobanteProductoId = comprobanteProducto.EntityId,
						});
					}

					comprobanteRepository.ActualizarEstadoDespachoPorCantidadEntregada(new Comprobante()
					{
						ComprobanteId = dto.ComprobanteId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
