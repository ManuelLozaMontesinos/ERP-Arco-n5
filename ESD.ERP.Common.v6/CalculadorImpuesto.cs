using System;

namespace ESD.ERP.Common
{
   public class CalculadorVentaItemDTO
   {
      public string TipoIGV { get; set; }
      public decimal Cantidad { get; set; }
      public decimal ValorUnitario { get; set; }
      public decimal PrecioUnitario { get; set; }
      public decimal ImpuestoTasa { get; set; }
      public decimal MontoBruto { get; set; }
      public string TipoDescuento { get; set; }
      public decimal DescuentoValor { get; set; }
      public decimal PorcentajeDescuento { get; set; }
      public decimal MontoDescuento1 { get; set; }
      public decimal MontoNeto { get; set; }
      public decimal MontoImpuesto { get; set; }
      public decimal MontoTotal { get; set; }
      public decimal DetraccionTasa { get; set; }
      public decimal MontoDetraccion { get; set; }
   }
   public class CalculadorVenta
   {
      public static CalculadorVentaItemDTO CalcularPrecios(CalculadorVentaItemDTO item, int decimalPlaces)
      {
         CalculadorVentaItemDTO calculadorVentaItemDTO = new CalculadorVentaItemDTO();

         if(item.PrecioUnitario > decimal.Zero)
         {
            if (item.TipoIGV == TipoIgvConstante.GRAVADO)
            {
               calculadorVentaItemDTO.ValorUnitario = Round(item.PrecioUnitario / (decimal.One + Round(item.ImpuestoTasa / 100)), decimalPlaces);
            }
            else
            {
               calculadorVentaItemDTO.ValorUnitario = item.PrecioUnitario;
            }
            calculadorVentaItemDTO.PrecioUnitario = item.PrecioUnitario;
         }
         else
         {
            if (item.TipoIGV == TipoIgvConstante.GRAVADO)
            {
               calculadorVentaItemDTO.PrecioUnitario = Math.Round(item.ValorUnitario * (decimal.One + Round(item.ImpuestoTasa / 100)), decimalPlaces, MidpointRounding.ToEven);
            }
            else
            {
               calculadorVentaItemDTO.PrecioUnitario = item.ValorUnitario;
            }
            calculadorVentaItemDTO.ValorUnitario = item.ValorUnitario;
         }

         return calculadorVentaItemDTO;
      }
      public static CalculadorVentaItemDTO CalcularPrecios(CalculadorVentaItemDTO item)
      {
         return CalcularPrecios(item,2);
      }
      public static CalculadorVentaItemDTO CalcularMontos(CalculadorVentaItemDTO item)
      {
         return CalcularMontos(item, 2);
      }
      public static CalculadorVentaItemDTO CalcularMontos(CalculadorVentaItemDTO item, int decimalPlaces)
      {
         CalculadorVentaItemDTO calculadorVentaItemDTO = CalcularPrecios(item, decimalPlaces);

         item.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
         item.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;

         if (item.TipoIGV == TipoIgvConstante.GRAVADO){

            decimal montoTotal = Round(item.Cantidad * item.PrecioUnitario);
            decimal porcentajeIGV = (decimal.One + Round(item.ImpuestoTasa / 100));

            calculadorVentaItemDTO.MontoBruto = Round(montoTotal / porcentajeIGV);

            decimal montoDescuento1 = decimal.Zero;

            if (!string.IsNullOrEmpty(item.TipoDescuento) || item.TipoDescuento != TipoDescuentoConstante.SIN_DESCUENTO)
            {
               decimal montoTotalDescuento = Round(item.DescuentoValor * item.PrecioUnitario);
               decimal montoBrutoDescuento = Round(montoTotalDescuento / porcentajeIGV);

               montoDescuento1 = montoBrutoDescuento;
               montoTotal = montoTotal - montoTotalDescuento;

               if(calculadorVentaItemDTO.MontoBruto != 0)
                  calculadorVentaItemDTO.PorcentajeDescuento = Round((100 * montoBrutoDescuento) / calculadorVentaItemDTO.MontoBruto);
            }

            calculadorVentaItemDTO.MontoDescuento1 = montoDescuento1;
            calculadorVentaItemDTO.MontoNeto = calculadorVentaItemDTO.MontoBruto - montoDescuento1;
            calculadorVentaItemDTO.MontoImpuesto = Round(montoTotal - calculadorVentaItemDTO.MontoNeto);
            
            calculadorVentaItemDTO.MontoTotal = montoTotal;
         }

         if (item.DetraccionTasa == decimal.Zero)
            calculadorVentaItemDTO.MontoDetraccion = decimal.Zero;
         else
            calculadorVentaItemDTO.MontoDetraccion = CalcularDetraccion(calculadorVentaItemDTO.MontoTotal, item.DetraccionTasa);

         return calculadorVentaItemDTO;
      }

      public static decimal CalcularDetraccion(decimal montoTotal, decimal porcentajeDetraccion)
      {
         return Round(montoTotal * Round(porcentajeDetraccion / 100),0);
      }
      public static decimal Round(decimal d)
      {
         int decimalPlaces = 2;

         return decimal.Round(d, decimalPlaces);
      }
      public static decimal Round(decimal d, int decimalPlaces)
      {
         return decimal.Round(d, decimalPlaces);
      }
   }
}