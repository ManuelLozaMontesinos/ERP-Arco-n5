using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;

namespace ESD.ERP.Web.Controllers
{
   public class TopPedidoDTO
   {
      public int Id { get; set; }
      public string Codigo { get; set; }
      public string Cliente { get; set; }
      public decimal Monto { get; set; }
      public string MontoFormateado { get; set; }
   }

   public class ClienteInactivoDTO
   {
      public int Id { get; set; }
      public string Codigo { get; set; }
      public string Cliente { get; set; }
      public string Vendedor { get; set; }
      public string NumeroDocumentoIdentidad { get; set; }
      public DateTime FechaUltimaCompra { get; set; }
      public string FechaFormateada { get; set; }
      public int MesesSinComprar { get; set; }
   }

   public class VendedorDTO
   {
      public int id { get; set; }
      public string nombre { get; set; }
   }

   public class RolDTO
   {
      public int id { get; set; }
      public string codigo { get; set; }
      public string nombre { get; set; }
   }

   public class ProductoStockDTO
   {
      public int Orden { get; set; }
      public string Codigo { get; set; }
      public int ProductoId { get; set; }
      public string Descripcion { get; set; }
      public string UnidadMedida { get; set; }
      public decimal StockActual { get; set; }
      public decimal ConsumoMes { get; set; }
      public string StockActualFormateado { get; set; }
      public string ConsumoMesFormateado { get; set; }
      public string EstadoStock { get; set; }
      public string BadgeColor { get; set; }
   }

   public class AlmacenItemDTO
   {
      public int AlmacenId { get; set; }
      public string Nombre { get; set; }
   }

   public class TopMenorStockDTO
   {
      public int Orden { get; set; }
      public int ProductoId { get; set; }
      public string Codigo { get; set; }
      public string Nombre { get; set; }
      public decimal Cantidad { get; set; }
      public string CantidadFormateada { get; set; }
      public string UnidadMedida { get; set; }
      public string GrupoNombre { get; set; }
      public string ClaseNombre { get; set; }
      public string FamiliaNombre { get; set; }
      public string CategoriaCompleta
      {
         get
         {
            var partes = new[] { GrupoNombre, ClaseNombre, FamiliaNombre }
               .Where(s => !string.IsNullOrWhiteSpace(s))
               .ToArray();
            return partes.Length > 0 ? string.Join(" / ", partes) : "-";
         }
      }
      public string EstadoStock { get; set; }
      public string BadgeColor { get; set; }
   }

   public class CategoriaFiltroItemDTO
   {
      public int Id { get; set; }
      public string Nombre { get; set; }
   }

   public class CalendarioAreaDTO
   {
      public int AreaId { get; set; }
      public string Nombre { get; set; }
      public string Descripcion { get; set; }
      public string Color { get; set; }
   }

   public class HomeController : AuthorizedController
   {
      private static readonly DateTime TopPedidosFecha = new DateTime(2026, 10, 19);

      private const string ClientesInactivosSql = @"
         ;WITH Movimientos AS (
            SELECT c.proveedor_id, c.fecha, c.vendedor_id, 1 AS origen, c.comprobante_id AS registro_id
            FROM comprobante c
            LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
            WHERE c.estado = 'A'
              AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR pes.empresa_sede_id = @sedeSesion)
            UNION ALL
            SELECT co.proveedor_id, co.fecha, co.vendedor_id, 0 AS origen, co.cotiza_id AS registro_id
            FROM cotiza co
            LEFT JOIN periodo_emp_sede pes ON co.periodo_emp_sede_id = pes.periodo_emp_sede_id
            WHERE co.es_pedido = 'S' AND co.estado = 'A'
              AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR pes.empresa_sede_id = @sedeSesion)
         ),
         UltimaCompra AS (
            SELECT proveedor_id, fecha, vendedor_id,
                   ROW_NUMBER() OVER (PARTITION BY proveedor_id ORDER BY fecha DESC, origen DESC, registro_id DESC) AS rn
            FROM Movimientos
         )
         SELECT TOP 10
            p.proveedor_id AS ClienteId,
            p.proveedor AS CodigoCliente,
            p.num_doc_iden AS NumeroDocumentoIdentidad,
            ISNULL(NULLIF(RTRIM(LTRIM(p.razon_social)), ''), ISNULL(RTRIM(LTRIM(p.nombres + ' ' + ISNULL(p.apellido_pat, ''))), 'Sin Nombre')) AS ClienteNombre,
            COALESCE(NULLIF(RTRIM(LTRIM(vp.razon_social)), ''),
                     NULLIF(RTRIM(LTRIM(ISNULL(vp.nombres, '') + ' ' + ISNULL(vp.apellido_pat, ''))), ''),
                     NULLIF(RTRIM(LTRIM(v.abrev)), ''), 'Sin vendedor') AS VendedorNombre,
            u.fecha AS FechaUltimaCompra,
            DATEDIFF(month, u.fecha, GETDATE()) AS MesesSinComprar
         FROM UltimaCompra u
         INNER JOIN proveedor p ON u.proveedor_id = p.proveedor_id
         LEFT JOIN vendedor v ON u.vendedor_id = v.vendedor_id
         LEFT JOIN proveedor vp ON v.proveedor_id = vp.proveedor_id
         WHERE u.rn = 1
           AND (@vid IS NULL OR u.vendedor_id = @vid)
           AND u.fecha <= DATEADD(month, -@meses, GETDATE())
         ORDER BY u.fecha DESC";

      private readonly IComprobanteProxy comprobanteProxy;
      private readonly ICotizacionProxy cotizacionProxy;
      private readonly IOrdenProduccionProxy ordenProduccionProxy;
      private readonly IProductoProxy productoProxy;

      public HomeController(
         IComprobanteProxy comprobanteProxy,
         ICotizacionProxy cotizacionProxy,
         IOrdenProduccionProxy ordenProduccionProxy,
         IProductoProxy productoProxy
         )
      {
         this.comprobanteProxy = comprobanteProxy;
         this.cotizacionProxy = cotizacionProxy;
         this.ordenProduccionProxy = ordenProduccionProxy;
         this.productoProxy = productoProxy;
      }

      private static string _cachedConnectionString;

      private static string GetDbConnectionString()
      {
         if (!string.IsNullOrEmpty(_cachedConnectionString))
         {
            return _cachedConnectionString;
         }

         string[] candidates = new[]
         {
            "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;",
            "Server=localhost;Database=esderp_v6;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=3;",
            "Server=.;Database=esderp_v6;User Id=sa;Password=admin123*;TrustServerCertificate=True;Connect Timeout=3;"
         };

         foreach (var cs in candidates)
         {
            try
            {
               using (var c = new SqlConnection(cs))
               {
                  c.Open();
                  _cachedConnectionString = cs;
                  return cs;
               }
            }
            catch { }
         }

         _cachedConnectionString = candidates[0];
         return _cachedConnectionString;
      }

      private bool EsAdminOGerencia()
      {
         try
         {
            if (EsAdministrador()) return true;
            var roles = ObtenerRoles();
            if (roles != null && (roles.Contains("S0001") || roles.Contains("00003") || roles.Any(r => r.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))))
            {
               return true;
            }
         }
         catch { }
         return false;
      }

      private (int? vendedorId, string vendedorNombre) ObtenerVendedorUsuarioActual()
      {
         try
         {
            var usuarioDTO = ObtenerUsuarioDTO();
            if (usuarioDTO != null)
            {
               using (var conn = new SqlConnection(GetDbConnectionString()))
               {
                  conn.Open();
                  // 1. Busqueda directa por p.usuario_id
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT TOP 1 v.vendedor_id, v.abrev
                        FROM vendedor v
                        INNER JOIN proveedor p ON v.proveedor_id = p.proveedor_id
                        WHERE v.estado = 'A' AND p.usuario_id = @uid";
                     cmd.Parameters.AddWithValue("@uid", usuarioDTO.UsuarioId);
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           return (Convert.ToInt32(reader["vendedor_id"]), reader["abrev"]?.ToString()?.Trim());
                        }
                     }
                  }

                  // 2. Busqueda de respaldo por usuario_logon o nombres
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT TOP 1 v.vendedor_id, v.abrev
                        FROM vendedor v
                        INNER JOIN proveedor p ON v.proveedor_id = p.proveedor_id
                        WHERE v.estado = 'A' AND (
                           UPPER(v.abrev) = UPPER(@logon)
                           OR UPPER(p.razon_social) LIKE '%' + UPPER(@logon) + '%'
                           OR UPPER(p.nombres) LIKE '%' + UPPER(@logon) + '%'
                        )";
                     cmd.Parameters.AddWithValue("@logon", usuarioDTO.UsuarioLogon);
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           return (Convert.ToInt32(reader["vendedor_id"]), reader["abrev"]?.ToString()?.Trim());
                        }
                     }
                  }
               }
            }
         }
         catch { }

         return (null, null);
      }

      private dynamic ObtenerMetricasDesdeBD(int? vendedorIdFiltro = null, int mesesInactivos = 5, int? vendedorClientesFiltro = null, int? sedeIdFiltro = null, int? anioSesion = null, int? sedeSesionId = null, int? vendedorGraficoId = null)
      {
         decimal facturacionMes = 0m;
         int pedidosActivos = 0;
         int cotizacionesActivas = 0;
         int ordenesProduccionMes = 0;
         int ordenesCompraMes = 0;
         int salidasConsumoInternoMes = 0;
         int ingresosMermaMes = 0;
         int salidasMermaMes = 0;
         int guiasRemisionEmitidasMes = 0;
         var topPedidos = new List<TopPedidoDTO>();
         var clientesInactivos = new List<ClienteInactivoDTO>();
         double sumatoriaPedidosMes = 0;
         int cantidadPedidosMes = 0;
         var listaVendedores = new List<VendedorDTO>();
         int primerVendedorId = vendedorIdFiltro ?? (vendedorGraficoId.HasValue && vendedorGraficoId.Value > 0 ? vendedorGraficoId.Value : 50);
         string primerVendedorNombre = "CRISTIAN";
         var vendedorCantidades = new List<int>();
         var vendedorMontos = new List<double>();
         var listaRoles = new List<RolDTO>();

         string[] nombresMeses = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
         DateTime hoy = DateTime.Today;

         int anioConsulta = anioSesion ?? hoy.Year;
         int? sedeSesionFiltro = (sedeSesionId.HasValue && sedeSesionId.Value > 0) ? sedeSesionId.Value : (int?)null;
         int mesConsulta = (anioConsulta == hoy.Year) ? hoy.Month : 12;

         // Compara los ultimos 12 meses del anio de sesion con el anio anterior
         DateTime inicioMesActual = new DateTime(anioConsulta, mesConsulta, 1);
         DateTime inicioPeriodoActual = inicioMesActual.AddMonths(-11);
         DateTime inicioConsultaVentas = inicioPeriodoActual.AddYears(-1);
         DateTime finPeriodoActual = inicioMesActual.AddMonths(1);
         var mesesVentas = Enumerable.Range(0, 12)
            .Select(i => nombresMeses[inicioPeriodoActual.AddMonths(i).Month])
            .ToArray();
         var ventasPeriodoActual = new double[12];
         var ventasPeriodoAnterior = new double[12];
         var ventasBarraActual = new double?[12];

         string[] mesesCortos = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Set", "Oct", "Nov", "Dic" };
         var fechasLista = new List<DateTime>();
         var stackedDias = new List<string>();

         DateTime fechaAncla = (anioConsulta == hoy.Year)
            ? hoy
            : new DateTime(anioConsulta, mesConsulta, (mesConsulta == hoy.Month ? Math.Min(hoy.Day, DateTime.DaysInMonth(anioConsulta, mesConsulta)) : DateTime.DaysInMonth(anioConsulta, mesConsulta)));

         for (int i = 11; i >= 0; i--)
         {
            DateTime f = fechaAncla.AddDays(-i).Date;
            fechasLista.Add(f);
            stackedDias.Add(f.Day + " " + mesesCortos[f.Month]);
         }

         var dsCentral = new double[12];
         var dsIlo = new double[12];

         int prodEnProceso = 0;
         int prodCulminadas = 0;
         int prodPendientes = 0;
         int prodAnuladas = 0;

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               // 1. Facturacion del mes actual (filtrado por vendedor y sede si aplica)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT ISNULL(SUM(c.monto_total), 0) AS total
                        FROM comprobante c
                        INNER JOIN estado_compro ec ON c.estado_compro_id = ec.estado_compro_id
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE YEAR(c.fecha) = @anio 
                          AND MONTH(c.fecha) = @mes 
                          AND c.estado = 'A'
                          AND (ec.estado_compro = '3' OR UPPER(ec.nombre) LIKE '%EMITID%' OR c.estado_compro_id = 3)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value)
                     {
                        facturacionMes = Convert.ToDecimal(val);
                     }
                  }
               }
               catch { }

               // 2. Pedidos Activos (filtrado por vendedor y sede si aplica)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'S' AND c.estado_cotiza_id IN (2, 4) AND c.estado = 'A'
                          AND (@anio IS NULL OR @anio = 0 OR YEAR(c.fecha) = @anio)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value)
                     {
                        pedidosActivos = Convert.ToInt32(val);
                     }
                  }
               }
               catch { }

               // 3. Cotizaciones Activas (filtrado por vendedor y sede si aplica)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'N' AND c.estado_cotiza_id = 1 AND c.estado = 'A' 
                          AND (@anio IS NULL OR @anio = 0 OR YEAR(c.fecha) = @anio)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value)
                     {
                        cotizacionesActivas = Convert.ToInt32(val);
                     }
                  }
               }
               catch { }

                // 4. Ordenes de Produccion del mes actual (filtradas por vendedor cuando aplica)
                try
                {
                   using (var cmd = conn.CreateCommand())
                   {
                      cmd.CommandText = @"
                         SELECT COUNT(*)
                         FROM orden_pro o
                         LEFT JOIN cotiza c ON c.cotiza_id = o.cotiza_id
                         LEFT JOIN periodo_emp_sede pes ON o.periodo_emp_sede_id = pes.periodo_emp_sede_id
                          WHERE YEAR(o.fecha) = @anio
                            AND MONTH(o.fecha) = @mes
                           AND ISNULL(o.estado, 'A') <> 'I'
                           AND ISNULL(o.estado_orden_p_id, 0) <> 4
                           AND (@vid IS NULL OR c.vendedor_id = @vid)
                           AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                       cmd.Parameters.AddWithValue("@anio", anioConsulta);
                       cmd.Parameters.AddWithValue("@mes", mesConsulta);
                      cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                      cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                      var val = cmd.ExecuteScalar();
                      if (val != null && val != DBNull.Value)
                      {
                         ordenesProduccionMes = Convert.ToInt32(val);
                      }
                   }
                }
                catch { }

                // La distribucion de produccion y compras es un indicador general de gestion.
               if (!vendedorIdFiltro.HasValue)
               {
                  // 4b. Estado de Produccion del mes actual (Doughnut Chart)
                  try
                  {
                     using (var cmd = conn.CreateCommand())
                     {
                        cmd.CommandText = @"
                           SELECT 
                               CASE 
                                   WHEN o.estado = 'I' OR o.estado_orden_p_id = 4 THEN 'Anuladas'
                                   WHEN o.estado_orden_p_id = 1 THEN 'Pendientes'
                                   WHEN o.estado_orden_p_id = 2 THEN 'En Proceso'
                                   WHEN o.estado_orden_p_id = 3 THEN 'Culminadas'
                                   ELSE 'En Proceso'
                               END AS Estado,
                               COUNT(*) AS Cantidad
                           FROM orden_pro o
                           LEFT JOIN periodo_emp_sede pes ON o.periodo_emp_sede_id = pes.periodo_emp_sede_id
                           WHERE YEAR(o.fecha) = @anio
                             AND MONTH(o.fecha) = @mes
                             AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR pes.empresa_sede_id = @sedeSesion)
                           GROUP BY 
                               CASE 
                                   WHEN o.estado = 'I' OR o.estado_orden_p_id = 4 THEN 'Anuladas'
                                   WHEN o.estado_orden_p_id = 1 THEN 'Pendientes'
                                   WHEN o.estado_orden_p_id = 2 THEN 'En Proceso'
                                   WHEN o.estado_orden_p_id = 3 THEN 'Culminadas'
                                   ELSE 'En Proceso'
                               END";
                        cmd.Parameters.AddWithValue("@anio", anioConsulta);
                        cmd.Parameters.AddWithValue("@mes", mesConsulta);
                        cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionFiltro ?? DBNull.Value);
                        using (var reader = cmd.ExecuteReader())
                        {
                           while (reader.Read())
                           {
                              string est = reader["Estado"]?.ToString();
                              int cnt = Convert.ToInt32(reader["Cantidad"]);
                              if (est == "En Proceso") prodEnProceso = cnt;
                              else if (est == "Culminadas") prodCulminadas = cnt;
                              else if (est == "Pendientes") prodPendientes = cnt;
                              else if (est == "Anuladas") prodAnuladas = cnt;
                           }
                        }
                     }
                  }
                  catch { }

                  // 5. Ordenes de Compra del mes actual
                  try
                  {
                     using (var cmd = conn.CreateCommand())
                     {
                        cmd.CommandText = @"
                           SELECT COUNT(*) 
                           FROM orden_co o
                           LEFT JOIN periodo_emp_sede pes ON o.periodo_emp_sede_id = pes.periodo_emp_sede_id
                            WHERE YEAR(o.fecha) = @anio 
                              AND MONTH(o.fecha) = @mes 
                             AND o.estado = 'A'
                             AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                         cmd.Parameters.AddWithValue("@anio", anioConsulta);
                         cmd.Parameters.AddWithValue("@mes", mesConsulta);
                        cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                        var val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value)
                        {
                           ordenesCompraMes = Convert.ToInt32(val);
                        }
                     }
                  }
                  catch { }

               }

               // 5c. Movimientos de Almacen: Consumo Interno (9), Ingreso Merma (7), Salida Merma (8) Aprobados
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT 
                            m.trans_alma_id,
                            COUNT(*) AS cantidad
                        FROM mov_almacen m
                        LEFT JOIN periodo_emp_sede pes ON m.periodo_emp_sede_id = pes.periodo_emp_sede_id
                          WHERE YEAR(m.fecha) = @anio
                            AND MONTH(m.fecha) = @mes
                          AND m.estado = 'A'
                          AND ISNULL(m.estado_mov_alm_id, 0) <> 3
                          AND m.estado_aproba_id = 2
                          AND m.trans_alma_id IN (7, 8, 9)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)
                        GROUP BY m.trans_alma_id";
                       cmd.Parameters.AddWithValue("@anio", anioConsulta);
                       cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           int tid = Convert.ToInt32(reader["trans_alma_id"]);
                           int cant = Convert.ToInt32(reader["cantidad"]);
                           if (tid == 9) salidasConsumoInternoMes = cant;
                           else if (tid == 7) ingresosMermaMes = cant;
                           else if (tid == 8) salidasMermaMes = cant;
                        }
                     }
                  }
               }
               catch { }
               // 5d. Guias de Remision del mes actual con estado Emitido (2)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*)
                        FROM guia_remision g
                        LEFT JOIN periodo_emp_sede pes ON g.periodo_emp_sede_id = pes.periodo_emp_sede_id
                          WHERE YEAR(g.fecha) = @anio
                            AND MONTH(g.fecha) = @mes
                          AND g.estado = 'A'
                          AND g.estado_guia_r_id = 2
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                       cmd.Parameters.AddWithValue("@anio", anioConsulta);
                       cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value)
                     {
                        guiasRemisionEmitidasMes = Convert.ToInt32(val);
                     }
                  }
               }
               catch { }

                 // 6. Evolucion de Ventas: comparativa del ano seleccionado y el ano anterior
                 try
                 {
                    using (var cmd = conn.CreateCommand())
                    {
                       cmd.CommandText = @"
                          SELECT YEAR(c.fecha) AS anio, MONTH(c.fecha) AS mes, ROUND(SUM(c.monto_total) / 1000.0, 1) AS miles
                          FROM comprobante c
                          LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                          WHERE c.fecha >= @inicioConsultaVentas AND c.fecha < @finPeriodoActual
                            AND c.estado = 'A'
                            AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR pes.empresa_sede_id = @sedeSesion)
                          GROUP BY YEAR(c.fecha), MONTH(c.fecha)
                          ORDER BY anio, mes";
                       cmd.Parameters.Add("@inicioConsultaVentas", SqlDbType.Date).Value = inicioConsultaVentas.Date;
                       cmd.Parameters.Add("@finPeriodoActual", SqlDbType.Date).Value = finPeriodoActual.Date;
                       cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionFiltro ?? DBNull.Value);

                       var salesByYearMonth = new Dictionary<string, double>();
                       using (var reader = cmd.ExecuteReader())
                       {
                          while (reader.Read())
                          {
                             int anio = Convert.ToInt32(reader["anio"]);
                             int mes = Convert.ToInt32(reader["mes"]);
                             double miles = Convert.ToDouble(reader["miles"]);
                             salesByYearMonth[string.Format("{0}-{1}", anio, mes)] = miles;
                          }
                       }

                       for (int k = 0; k < 12; k++)
                       {
                          DateTime mesActual = inicioPeriodoActual.AddMonths(k);
                          DateTime mesAnterior = mesActual.AddYears(-1);

                          string keyActual = string.Format("{0}-{1}", mesActual.Year, mesActual.Month);
                          string keyAnterior = string.Format("{0}-{1}", mesAnterior.Year, mesAnterior.Month);

                          double valActual = salesByYearMonth.ContainsKey(keyActual) ? salesByYearMonth[keyActual] : 0.0;
                          double valAnterior = salesByYearMonth.ContainsKey(keyAnterior) ? salesByYearMonth[keyAnterior] : 0.0;

                          ventasPeriodoActual[k] = valActual;

                          if (mesActual.Year < anioConsulta)
                          {
                             ventasPeriodoAnterior[k] = valActual; // Noviembre y Diciembre del año pasado para la barra gris
                             ventasBarraActual[k] = null;          // Sin barra azul en los meses del año anterior
                          }
                          else
                          {
                             ventasPeriodoAnterior[k] = valAnterior; // Ene a Dic: comparativa contra el año anterior
                             ventasBarraActual[k] = valActual;
                          }
                       }
                    }
                 }
                 catch { }

                // 7. Diferencia diaria: compara las ventas de ambas sedes para todos los usuarios con el widget.
                try
                {
                   using (var cmd = conn.CreateCommand())
                   {
                      cmd.CommandText = @"
                         SELECT
                             CAST(c.fecha AS DATE) AS fecha_dia,
                             ROUND(SUM(CASE WHEN s.empresa_sede_id = 1 THEN c.monto_total ELSE 0 END), 2) AS central,
                             ROUND(SUM(CASE WHEN s.empresa_sede_id = 2 THEN c.monto_total ELSE 0 END), 2) AS ilo
                         FROM comprobante c
                         INNER JOIN periodo_emp_sede p ON c.periodo_emp_sede_id = p.periodo_emp_sede_id
                         INNER JOIN empresa_sede s ON p.empresa_sede_id = s.empresa_sede_id
                          WHERE c.fecha >= @fDesde AND c.fecha < @fHasta
                           AND c.estado = 'A'
                         GROUP BY CAST(c.fecha AS DATE)
                         ORDER BY fecha_dia";
                       cmd.Parameters.Add("@fDesde", SqlDbType.Date).Value = fechasLista[0].Date;
                       cmd.Parameters.Add("@fHasta", SqlDbType.Date).Value = fechaAncla.AddDays(1).Date;
                      using (var reader = cmd.ExecuteReader())
                      {
                         while (reader.Read())
                         {
                            DateTime fDia = Convert.ToDateTime(reader["fecha_dia"]).Date;
                            int idx = fechasLista.IndexOf(fDia);
                            if (idx >= 0 && idx < 12)
                            {
                               dsCentral[idx] = Convert.ToDouble(reader["central"]);
                               dsIlo[idx] = Convert.ToDouble(reader["ilo"]);
                            }
                         }
                      }
                   }
                }
                catch { }

                // 8. Top 10 pedidos globales del dia de la copia restaurada
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT TOP 10 
                            c.cotiza_id, 
                            c.cotiza, 
                            ISNULL(NULLIF(RTRIM(LTRIM(p.razon_social)), ''), ISNULL(RTRIM(LTRIM(p.nombres)), 'Sin Cliente')) AS cliente, 
                            c.monto_total
                        FROM cotiza c
                        LEFT JOIN proveedor p ON c.proveedor_id = p.proveedor_id
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'S' 
                          AND c.estado = 'A' 
                          AND YEAR(c.fecha) = @anio
                          AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR pes.empresa_sede_id = @sedeSesion)
                        ORDER BY c.monto_total DESC";
                      cmd.Parameters.AddWithValue("@anio", anioConsulta);
                      cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           decimal monto = Convert.ToDecimal(reader["monto_total"]);
                           topPedidos.Add(new TopPedidoDTO
                           {
                              Id = Convert.ToInt32(reader["cotiza_id"]),
                              Codigo = reader["cotiza"]?.ToString()?.Trim(),
                              Cliente = reader["cliente"]?.ToString()?.Trim(),
                              Monto = monto,
                              MontoFormateado = "S/ " + monto.ToString("N2")
                           });
                        }
                     }
                  }

                  // Ordenar estrictamente descendente por Monto (el de mayor total es el #1)
                  topPedidos = topPedidos.OrderByDescending(p => p.Monto).ToList();
               }
               catch { }

               // 9. Clientes sin compras (filtrado por vendedor si aplica)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = ClientesInactivosSql;
                     cmd.Parameters.AddWithValue("@meses", mesesInactivos);
                     cmd.Parameters.AddWithValue("@vid", (object)(vendedorClientesFiltro ?? vendedorIdFiltro) ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           DateTime fUltima = Convert.ToDateTime(reader["FechaUltimaCompra"]);
                           int meses = reader["MesesSinComprar"] != DBNull.Value ? Convert.ToInt32(reader["MesesSinComprar"]) : mesesInactivos;
                           clientesInactivos.Add(new ClienteInactivoDTO
                           {
                              Id = Convert.ToInt32(reader["ClienteId"]),
                              Codigo = reader["CodigoCliente"]?.ToString()?.Trim(),
                              Cliente = reader["ClienteNombre"]?.ToString()?.Trim(),
                              Vendedor = reader["VendedorNombre"]?.ToString()?.Trim(),
                              NumeroDocumentoIdentidad = reader["NumeroDocumentoIdentidad"]?.ToString()?.Trim(),
                              FechaUltimaCompra = fUltima,
                              FechaFormateada = fUltima.ToString("dd/MM/yyyy"),
                              MesesSinComprar = meses
                           });
                        }
                     }
                  }
               }
               catch { }

               // 5b. Sumatoria de Pedidos del Mes Actual (filtrado por vendedor si aplica)
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT 
                            ROUND(ISNULL(SUM(c.monto_total), 0), 2) AS sumatoria,
                            COUNT(*) AS cantidad
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'S' 
                          AND c.estado = 'A'
                          AND c.estado_cotiza_id IN (2, 3, 4)
                            AND YEAR(c.fecha) = @anio 
                            AND MONTH(c.fecha) = @mes
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                       cmd.Parameters.AddWithValue("@anio", anioConsulta);
                       cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeIdFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           if (reader["sumatoria"] != DBNull.Value) sumatoriaPedidosMes = Convert.ToDouble(reader["sumatoria"]);
                           if (reader["cantidad"] != DBNull.Value) cantidadPedidosMes = Convert.ToInt32(reader["cantidad"]);
                        }
                     }
                  }
               }
               catch { }

               // 9b. Lista de Vendedores y Vendedor Inicial
               if (vendedorIdFiltro.HasValue)
               {
                  primerVendedorId = vendedorIdFiltro.Value;
                  try
                  {
                     using (var cmd = conn.CreateCommand())
                     {
                        cmd.CommandText = "SELECT abrev FROM vendedor WHERE vendedor_id = @vid";
                        cmd.Parameters.AddWithValue("@vid", primerVendedorId);
                        var val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value)
                        {
                           primerVendedorNombre = val.ToString().Trim();
                        }
                     }
                  }
                  catch { }

                  listaVendedores.Add(new VendedorDTO { id = primerVendedorId, nombre = primerVendedorNombre });
               }
               else
               {
                  try
                  {
                     using (var cmd = conn.CreateCommand())
                     {
                        cmd.CommandText = @"
                           SELECT DISTINCT v.vendedor_id, v.abrev AS nombre
                           FROM vendedor v
                           INNER JOIN proveedor p ON v.proveedor_id = p.proveedor_id
                           INNER JOIN usuario u ON p.usuario_id = u.usuario_id
                           INNER JOIN rol_usuario ru ON ru.usuario_id = u.usuario_id
                           INNER JOIN rol r ON ru.rol_id = r.rol_id
                           LEFT JOIN personal per ON per.proveedor_id = p.proveedor_id
                           WHERE v.estado = 'A'
                             AND (r.rol IN ('00003', '00004', '00010') OR r.nombre LIKE '%Venta%')
                             AND LEN(LTRIM(RTRIM(v.abrev))) > 0
                             AND (per.fecha_cese IS NULL OR per.fecha_cese > GETDATE())
                             AND (per.estado IS NULL OR per.estado = 'A')
                           ORDER BY v.abrev ASC";
                        using (var reader = cmd.ExecuteReader())
                        {
                           while (reader.Read())
                           {
                              int vid = Convert.ToInt32(reader["vendedor_id"]);
                              string vnom = reader["nombre"].ToString().Trim();
                              listaVendedores.Add(new VendedorDTO { id = vid, nombre = vnom });
                           }
                        }
                     }

                     if (listaVendedores.Count > 0)
                     {
                        var defaultVendedor = listaVendedores.FirstOrDefault(x => x.id == primerVendedorId) ?? listaVendedores[0];
                        primerVendedorId = defaultVendedor.id;
                        primerVendedorNombre = defaultVendedor.nombre;
                     }
                  }
                  catch { }
               }

               // Pedidos diarios del vendedor seleccionado/inicial (ultimos 12 dias)
               try
               {
                  var mapaVendedor = new Dictionary<string, (int cantidad, double monto)>();
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT 
                            CAST(c.fecha AS DATE) AS dia,
                            COUNT(*) AS cantidad,
                            ROUND(SUM(c.monto_total), 2) AS monto
                        FROM cotiza c
                        WHERE c.es_pedido = 'S' AND c.estado = 'A'
                          AND c.vendedor_id = @vid
                          AND c.fecha >= @fDesde AND c.fecha < @fHasta
                        GROUP BY CAST(c.fecha AS DATE)";
                     cmd.Parameters.AddWithValue("@vid", primerVendedorId);
                     cmd.Parameters.AddWithValue("@fDesde", fechasLista[0]);
                      cmd.Parameters.AddWithValue("@fHasta", fechaAncla.AddDays(1).Date);

                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           DateTime d = Convert.ToDateTime(reader["dia"]);
                           int cant = Convert.ToInt32(reader["cantidad"]);
                           double mto = Convert.ToDouble(reader["monto"]);
                           mapaVendedor[d.ToString("yyyy-MM-dd")] = (cant, mto);
                        }
                     }
                  }

                  foreach (var f in fechasLista)
                  {
                     string k = f.ToString("yyyy-MM-dd");
                     if (mapaVendedor.ContainsKey(k))
                     {
                        vendedorCantidades.Add(mapaVendedor[k].cantidad);
                        vendedorMontos.Add(mapaVendedor[k].monto);
                     }
                     else
                     {
                        vendedorCantidades.Add(0);
                        vendedorMontos.Add(0.0);
                     }
                  }
               }
               catch { }

               // 10. Lista de Roles del sistema
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT rol_id, rol AS codigo, nombre
                        FROM rol
                        WHERE estado = 'A'
                        ORDER BY nombre";
                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           listaRoles.Add(new RolDTO
                           {
                              id = Convert.ToInt32(reader["rol_id"]),
                              codigo = reader["codigo"].ToString().Trim(),
                              nombre = reader["nombre"].ToString().Trim()
                           });
                        }
                     }
                  }
               }
               catch { }
            }
         }
         catch { }

         return new
         {
            facturacionMes = "S/ " + facturacionMes.ToString("N2"),
            pedidosMesMonto = "S/ " + sumatoriaPedidosMes.ToString("N2"),
            pedidosMesCantidad = cantidadPedidosMes,
            listaVendedores = listaVendedores,
            primerVendedorId = primerVendedorId,
            primerVendedorNombre = primerVendedorNombre,
            vendedorCantidades = vendedorCantidades,
            vendedorMontos = vendedorMontos,
            listaRoles = listaRoles,
            pedidosActivos = pedidosActivos.ToString("N0"),
            cotizacionesActivas = cotizacionesActivas.ToString("N0"),
            ordenesProduccionMes = ordenesProduccionMes.ToString("N0"),
            ordenesCompraMes = ordenesCompraMes.ToString("N0"),
            consumoInternoMes = salidasConsumoInternoMes.ToString("N0"),
            ingresosMermaMes = ingresosMermaMes.ToString("N0"),
            salidasMermaMes = salidasMermaMes.ToString("N0"),
            guiasRemisionMes = guiasRemisionEmitidasMes.ToString("N0"),
            mesesVentas = mesesVentas,
            ventasAnterior = ventasPeriodoAnterior,
            ventasActual = ventasPeriodoActual,
            ventasBarraActual = ventasBarraActual,
            stackedLabels = stackedDias,
            dsCentral = dsCentral,
            dsIlo = dsIlo,
            prodLabels = new[] { "En Proceso", "Culminadas", "Pendientes", "Anuladas" },
            prodValores = new[] { prodEnProceso, prodCulminadas, prodPendientes, prodAnuladas },
            topPedidos = topPedidos,
            topPedidosFecha = TopPedidosFecha.ToString("dd/MM/yyyy"),
            clientesInactivos = clientesInactivos
         };
      }

      public IActionResult Index()
      {
         string cid = Request.Query["cid"];
         if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = null;
         try
         {
            if (!string.IsNullOrEmpty(cid))
            {
               periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
            }
         }
         catch { }
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();

         int anioSesion = 2026;
         if (periodoEmpresaSedeDTO?.PeriodoDTO != null && periodoEmpresaSedeDTO.PeriodoDTO.Codigo > 0)
         {
            anioSesion = periodoEmpresaSedeDTO.PeriodoDTO.Codigo;
         }
         int sedeSesionId = periodoEmpresaSedeDTO?.EmpresaSedeDTO?.EmpresaSedeId ?? 1;
         string sedeSesionNombre = periodoEmpresaSedeDTO?.EmpresaSedeDTO?.Nombre ?? "SEDE CENTRAL";

         ViewBag.Empresa = periodoEmpresaSedeDTO?.EmpresaSedeDTO?.EmpresaDTO?.NombreComercial ?? "ARCO";
         ViewBag.Usuario = usuarioDTO?.UsuarioLogon ?? "administrador";
         ViewBag.Periodo = anioSesion.ToString();
         ViewBag.Sede = sedeSesionNombre;
         ViewBag.AnioSesion = anioSesion;
         ViewBag.SedeSesionId = sedeSesionId;
         ViewBag.SedeSesionNombre = sedeSesionNombre;
         ViewBag.cid = cid;

         bool esAdminOGerencia = EsAdminOGerencia();
         ViewBag.EsAdminOGerencia = esAdminOGerencia;
         ViewBag.EsAdmin = esAdminOGerencia;
         ViewBag.PuedeFiltrarVendedorClientes = EsAdministrador();

         int? vendedorIdFiltro = null;
         if (!esAdminOGerencia)
         {
            var (vid, _) = ObtenerVendedorUsuarioActual();
            vendedorIdFiltro = vid;
         }

         int? vendedorClientesInicial = null;
         if (!EsAdministrador())
         {
            var (vendedorClienteId, _) = ObtenerVendedorUsuarioActual();
            vendedorClientesInicial = vendedorClienteId;
         }
         int? sedeDashboard = null;
         if (Request.Query.ContainsKey("sedeDashboard") && int.TryParse(Request.Query["sedeDashboard"], out int sId) && sId > 0)
         {
            sedeDashboard = sId;
         }
         ViewBag.SedeDashboardInicial = sedeDashboard ?? 0;
         var metricas = ObtenerMetricasDesdeBD(vendedorIdFiltro, 5, vendedorClientesInicial, sedeDashboard, anioSesion, sedeSesionId);

         List<string> userRoles = new List<string>();
         try { userRoles = ObtenerRoles(); } catch { }
         ViewBag.UserRoles = userRoles;
         ViewBag.RolActualCodigo = (userRoles != null && userRoles.Count > 0) ? userRoles[0] : "S0001";
         ViewBag.ListaRoles = metricas.listaRoles;

         ViewBag.FacturacionMes = metricas.facturacionMes;
         ViewBag.PedidosMesMonto = metricas.pedidosMesMonto;
         ViewBag.PedidosMesCantidad = metricas.pedidosMesCantidad;
         ViewBag.ListaVendedores = metricas.listaVendedores;
         ViewBag.VendedorIdInicial = metricas.primerVendedorId;
         ViewBag.VendedorNombreInicial = metricas.primerVendedorNombre;
         ViewBag.VendedorCantidades = metricas.vendedorCantidades;
         ViewBag.VendedorMontos = metricas.vendedorMontos;
         ViewBag.PedidosActivos = metricas.pedidosActivos;
         ViewBag.CotizacionesActivas = metricas.cotizacionesActivas;
         ViewBag.OrdenesProduccionMes = metricas.ordenesProduccionMes;
         ViewBag.OrdenesCompraMes = metricas.ordenesCompraMes;
         ViewBag.ConsumoInternoMes = metricas.consumoInternoMes;
         ViewBag.IngresosMermaMes = metricas.ingresosMermaMes;
         ViewBag.SalidasMermaMes = metricas.salidasMermaMes;
         ViewBag.GuiasRemisionMes = metricas.guiasRemisionMes;
         ViewBag.MesesVentas = metricas.mesesVentas;
         ViewBag.VentasAnterior = metricas.ventasAnterior;
         ViewBag.VentasActual = metricas.ventasActual;
         ViewBag.VentasBarraActual = metricas.ventasBarraActual;
         ViewBag.TopPedidos = metricas.topPedidos;
         ViewBag.TopPedidosFecha = metricas.topPedidosFecha;
         ViewBag.ClientesInactivos = metricas.clientesInactivos;
         ViewBag.StackedLabels = metricas.stackedLabels;
         ViewBag.DsCentral = metricas.dsCentral;
         ViewBag.DsIlo = metricas.dsIlo;
         ViewBag.ProdLabels = metricas.prodLabels;
         ViewBag.ProdValores = metricas.prodValores;
         ViewBag.ListaStockInsumos = ObtenerStockInsumosInterno(0, sedeSesionId);
         ViewBag.ListaAlmacenesStock = ObtenerAlmacenesActivos(sedeSesionId);
         ViewBag.MesActualStock = DateTime.Today.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
         ViewBag.ListaGruposProducto = ObtenerGruposActivos();
         ViewBag.ListaTopMenorStock = ObtenerTopMenorStockInterno(null, null, null, sedeSesionId);

         int usuarioIdAgenda = ObtenerUsuarioId();
         if (usuarioIdAgenda <= 0 && usuarioDTO != null) usuarioIdAgenda = usuarioDTO.UsuarioId;
         bool esAdminAgenda = esAdminOGerencia || EsAdministrador();
         var areasAgenda = ObtenerAreasUsuarioAgenda(usuarioIdAgenda, esAdminAgenda);
         ViewBag.AreasAgenda = areasAgenda;
         ViewBag.TieneMultiplesAreasAgenda = esAdminAgenda || (areasAgenda != null && areasAgenda.Count > 1);
         ViewBag.EsAdminAgenda = esAdminAgenda;
         ViewBag.AreaAgendaInicialId = areasAgenda.Count > 0 ? areasAgenda[0].AreaId : (esAdminAgenda ? 1 : 2);
         ViewBag.AreaAgendaInicialNombre = areasAgenda.Count > 0 ? areasAgenda[0].Nombre : (esAdminAgenda ? "Administración" : "Ventas");

         bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                    || !string.IsNullOrEmpty(Request.Headers["x-app-cid"])
                    || string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase);

         if (isAjax)
         {
            return PartialView();
         }

         return View();
      }

      [HttpGet]
      public JsonResult ObtenerKpisPorSede(int? sedeId = null, int? anio = null, string cid = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            int anioConsulta = anio ?? 2026;
            if (!anio.HasValue || anio.Value <= 0)
            {
               if (!string.IsNullOrEmpty(cid))
               {
                  try
                  {
                     var pDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
                     if (pDTO?.PeriodoDTO != null && pDTO.PeriodoDTO.Codigo > 0)
                     {
                        anioConsulta = pDTO.PeriodoDTO.Codigo;
                     }
                  }
                  catch { }
               }
            }

            DateTime hoy = DateTime.Today;
            int mesConsulta = (anioConsulta == hoy.Year) ? hoy.Month : 12;

            bool esAdminOGerencia = EsAdminOGerencia();
            int? vendedorIdFiltro = null;
            if (!esAdminOGerencia)
            {
               var (vid, _) = ObtenerVendedorUsuarioActual();
               vendedorIdFiltro = vid;
            }

            int? sedeFiltro = (sedeId.HasValue && sedeId.Value > 0) ? sedeId.Value : (int?)null;

            decimal facturacionMes = 0m;
            int pedidosActivos = 0;
            int cotizacionesActivas = 0;
            int ordenesProduccionMes = 0;
            int ordenesCompraMes = 0;
            double sumatoriaPedidosMes = 0;
            int cantidadPedidosMes = 0;
            int salidasConsumoInternoMes = 0;
            int ingresosMermaMes = 0;
            int salidasMermaMes = 0;
            int guiasRemisionEmitidasMes = 0;

            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();

               // 1. Facturacion del mes
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT ISNULL(SUM(c.monto_total), 0) AS total
                        FROM comprobante c
                        INNER JOIN estado_compro ec ON c.estado_compro_id = ec.estado_compro_id
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE YEAR(c.fecha) = @anio 
                          AND MONTH(c.fecha) = @mes 
                          AND c.estado = 'A'
                          AND (ec.estado_compro = '3' OR UPPER(ec.nombre) LIKE '%EMITID%' OR c.estado_compro_id = 3)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value) facturacionMes = Convert.ToDecimal(val);
                  }
               }
               catch { }

               // 2. Pedidos Activos
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'S' AND c.estado_cotiza_id IN (2, 4) AND c.estado = 'A'
                          AND (@anio IS NULL OR @anio = 0 OR YEAR(c.fecha) = @anio)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value) pedidosActivos = Convert.ToInt32(val);
                  }
               }
               catch { }

               // 3. Cotizaciones Activas
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*) 
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'N' AND c.estado_cotiza_id = 1 AND c.estado = 'A' 
                          AND (@anio IS NULL OR @anio = 0 OR YEAR(c.fecha) = @anio)
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value) cotizacionesActivas = Convert.ToInt32(val);
                  }
               }
               catch { }

               // 4. Ordenes de Produccion del mes
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*)
                        FROM orden_pro o
                        LEFT JOIN cotiza c ON c.cotiza_id = o.cotiza_id
                        LEFT JOIN periodo_emp_sede pes ON o.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE YEAR(o.fecha) = @anio
                          AND MONTH(o.fecha) = @mes
                          AND ISNULL(o.estado, 'A') <> 'I'
                          AND ISNULL(o.estado_orden_p_id, 0) <> 4
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value) ordenesProduccionMes = Convert.ToInt32(val);
                  }
               }
               catch { }

               // 5. Ordenes de Compra del mes
               if (!vendedorIdFiltro.HasValue)
               {
                  try
                  {
                     using (var cmd = conn.CreateCommand())
                     {
                        cmd.CommandText = @"
                           SELECT COUNT(*) 
                           FROM orden_co o
                           LEFT JOIN periodo_emp_sede pes ON o.periodo_emp_sede_id = pes.periodo_emp_sede_id
                           WHERE YEAR(o.fecha) = @anio 
                             AND MONTH(o.fecha) = @mes 
                             AND o.estado = 'A'
                             AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                        cmd.Parameters.AddWithValue("@anio", anioConsulta);
                        cmd.Parameters.AddWithValue("@mes", mesConsulta);
                        cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                        var val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value) ordenesCompraMes = Convert.ToInt32(val);
                     }
                  }
                  catch { }
               }

               // 5b. Sumatoria de Pedidos del Mes
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT 
                            ROUND(ISNULL(SUM(c.monto_total), 0), 2) AS sumatoria,
                            COUNT(*) AS cantidad
                        FROM cotiza c
                        LEFT JOIN periodo_emp_sede pes ON c.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE c.es_pedido = 'S' 
                          AND c.estado = 'A'
                          AND c.estado_cotiza_id IN (2, 3, 4)
                          AND YEAR(c.fecha) = @anio 
                          AND MONTH(c.fecha) = @mes
                          AND (@vid IS NULL OR c.vendedor_id = @vid)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           if (reader["sumatoria"] != DBNull.Value) sumatoriaPedidosMes = Convert.ToDouble(reader["sumatoria"]);
                           if (reader["cantidad"] != DBNull.Value) cantidadPedidosMes = Convert.ToInt32(reader["cantidad"]);
                        }
                     }
                  }
               }
               catch { }

               // 5c. Consumo Interno y Mermas
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT 
                            m.trans_alma_id,
                            COUNT(*) AS cantidad
                        FROM mov_almacen m
                        LEFT JOIN periodo_emp_sede pes ON m.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE YEAR(m.fecha) = @anio
                          AND MONTH(m.fecha) = @mes
                          AND m.estado = 'A'
                          AND ISNULL(m.estado_mov_alm_id, 0) <> 3
                          AND m.estado_aproba_id = 2
                          AND m.trans_alma_id IN (7, 8, 9)
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)
                        GROUP BY m.trans_alma_id";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     using (var reader = cmd.ExecuteReader())
                     {
                        while (reader.Read())
                        {
                           int tid = Convert.ToInt32(reader["trans_alma_id"]);
                           int cant = Convert.ToInt32(reader["cantidad"]);
                           if (tid == 9) salidasConsumoInternoMes = cant;
                           else if (tid == 7) ingresosMermaMes = cant;
                           else if (tid == 8) salidasMermaMes = cant;
                        }
                     }
                  }
               }
               catch { }

               // 5d. Guias de Remision
               try
               {
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT COUNT(*)
                        FROM guia_remision g
                        LEFT JOIN periodo_emp_sede pes ON g.periodo_emp_sede_id = pes.periodo_emp_sede_id
                        WHERE YEAR(g.fecha) = @anio
                          AND MONTH(g.fecha) = @mes
                          AND g.estado = 'A'
                          AND g.estado_guia_r_id = 2
                          AND (@sede IS NULL OR @sede = 0 OR pes.empresa_sede_id = @sede)";
                     cmd.Parameters.AddWithValue("@anio", anioConsulta);
                     cmd.Parameters.AddWithValue("@mes", mesConsulta);
                     cmd.Parameters.AddWithValue("@sede", (object)sedeFiltro ?? DBNull.Value);
                     var val = cmd.ExecuteScalar();
                     if (val != null && val != DBNull.Value) guiasRemisionEmitidasMes = Convert.ToInt32(val);
                  }
               }
               catch { }
            }

            return Json(new
            {
               success = true,
               kpis = new
               {
                  facturacionMes = "S/ " + facturacionMes.ToString("N2"),
                  pedidosActivos = pedidosActivos.ToString("N0"),
                  cotizacionesActivas = cotizacionesActivas.ToString("N0"),
                  ordenesProduccionMes = ordenesProduccionMes.ToString("N0"),
                  ordenesCompraMes = ordenesCompraMes.ToString("N0"),
                  pedidosMesMonto = "S/ " + sumatoriaPedidosMes.ToString("N2"),
                  pedidosMesCantidad = cantidadPedidosMes,
                  consumoInternoMes = salidasConsumoInternoMes.ToString("N0"),
                  ingresosMermaMes = ingresosMermaMes.ToString("N0"),
                  salidasMermaMes = salidasMermaMes.ToString("N0"),
                  guiasRemisionMes = guiasRemisionEmitidasMes.ToString("N0")
               }
            });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      [HttpGet]
      public JsonResult ObtenerDatosDashboard(string cid, int mesesInactivos = 5, int? vendedorClientesId = null, int? sedeId = null, int? anio = null, int? vendedorId = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            bool esAdminOGerencia = EsAdminOGerencia();
            int? vendedorIdFiltro = null;
            if (!esAdminOGerencia)
            {
               var (vid, _) = ObtenerVendedorUsuarioActual();
               vendedorIdFiltro = vid;
            }

            int mesesFiltro = new[] { 3, 4, 5, 6 }.Contains(mesesInactivos) ? mesesInactivos : 5;
            int? vendedorInactivosFiltro = null;
            if (EsAdministrador())
            {
               vendedorInactivosFiltro = vendedorClientesId.GetValueOrDefault() > 0 ? vendedorClientesId.Value : (int?)null;
            }
            else
            {
               var (vendedorClienteId, _) = ObtenerVendedorUsuarioActual();
               vendedorInactivosFiltro = vendedorClienteId;
            }
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = null;
            try
            {
               if (!string.IsNullOrEmpty(cid))
               {
                  periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
               }
            }
            catch { }

            int anioSesion = anio ?? 2026;
            if (!anio.HasValue || anio.Value <= 0)
            if (periodoEmpresaSedeDTO?.PeriodoDTO != null && periodoEmpresaSedeDTO.PeriodoDTO.Codigo > 0)
            {
               anioSesion = periodoEmpresaSedeDTO.PeriodoDTO.Codigo;
            }
            int sedeSesionId = periodoEmpresaSedeDTO?.EmpresaSedeDTO?.EmpresaSedeId ?? 1;

            int? vendedorGrafico = null;
            if (EsAdministrador() && vendedorId.GetValueOrDefault() > 0)
            {
               vendedorGrafico = vendedorId.Value;
            }
            int? sedeFiltro = (sedeId.HasValue && sedeId.Value > 0) ? sedeId.Value : (int?)null;
            var metricas = ObtenerMetricasDesdeBD(vendedorIdFiltro, mesesFiltro, vendedorInactivosFiltro, sedeFiltro, anioSesion, sedeSesionId, vendedorGrafico);

            var response = new
            {
               success = true,
               esAdmin = esAdminOGerencia,
               kpis = new
               {
                  facturacionMes = metricas.facturacionMes,
                  pedidosActivos = metricas.pedidosActivos,
                  cotizacionesActivas = metricas.cotizacionesActivas,
                  ordenesProduccionMes = metricas.ordenesProduccionMes,
                  ordenesCompraMes = metricas.ordenesCompraMes,
                  pedidosMesMonto = metricas.pedidosMesMonto,
                  pedidosMesCantidad = metricas.pedidosMesCantidad,
                  consumoInternoMes = metricas.consumoInternoMes,
                  ingresosMermaMes = metricas.ingresosMermaMes,
                  salidasMermaMes = metricas.salidasMermaMes,
                  guiasRemisionMes = metricas.guiasRemisionMes
               },
               vendedores = metricas.listaVendedores,
               vendedorData = new
               {
                  id = metricas.primerVendedorId,
                  nombre = metricas.primerVendedorNombre,
                  cantidades = metricas.vendedorCantidades,
                  montos = metricas.vendedorMontos
               },
               topPedidos = metricas.topPedidos,
               topPedidosFecha = metricas.topPedidosFecha,
               clientesInactivos = metricas.clientesInactivos,
               graficoVentas = new
               {
                  meses = metricas.mesesVentas,
                  ventasAnterior = metricas.ventasAnterior,
                  ventasActual = metricas.ventasActual,
                  ventasBarraActual = metricas.ventasBarraActual
               },
               graficoStackedBar = new
               {
                  labels = metricas.stackedLabels,
                  datasets = new object[]
                  {
                     new
                     {
                        label = "Ilo",
                        data = metricas.dsIlo
                     },
                     new
                     {
                        label = "Central",
                        data = metricas.dsCentral
                     }
                  }
               },
               graficoProduccion = new
               {
                  labels = metricas.prodLabels,
                  valores = metricas.prodValores
               }
            };

            return Json(response);
         }
         catch (Exception ex)
         {
            return Json(new
            {
               success = false,
               message = ex.Message
            });
         }
      }

      [HttpGet]
      public JsonResult ObtenerPedidosVendedor(int vendedorId, int dias = 12, int? anio = null, string cid = null)
      {
         try
         {
            bool esAdminOGerencia = EsAdminOGerencia();
            if (!esAdminOGerencia)
            {
               var (vid, _) = ObtenerVendedorUsuarioActual();
               if (vid.HasValue)
               {
                  vendedorId = vid.Value; // El vendedor solo puede consultar sus propias ventas
               }
            }

            if (dias < 4 || dias > 30) dias = 12;

            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            int anioConsulta = anio ?? 0;
            if (anioConsulta <= 0 && !string.IsNullOrEmpty(cid))
            {
               try
               {
                  var periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
                  if (periodoEmpresaSedeDTO?.PeriodoDTO != null && periodoEmpresaSedeDTO.PeriodoDTO.Codigo > 0)
                  {
                     anioConsulta = periodoEmpresaSedeDTO.PeriodoDTO.Codigo;
                  }
               }
               catch { }
            }
            if (anioConsulta <= 0)
            {
               anioConsulta = DateTime.Today.Year;
            }

            DateTime hoy = DateTime.Today;
            int mesConsulta = (anioConsulta == hoy.Year) ? hoy.Month : 12;
            DateTime fechaAncla = (anioConsulta == hoy.Year)
               ? hoy
               : new DateTime(anioConsulta, mesConsulta, (mesConsulta == hoy.Month ? Math.Min(hoy.Day, DateTime.DaysInMonth(anioConsulta, mesConsulta)) : DateTime.DaysInMonth(anioConsulta, mesConsulta)));

            string[] mesesCortos = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Set", "Oct", "Nov", "Dic" };
            var diasList = new List<DateTime>();
            var labelsList = new List<string>();

            for (int i = dias - 1; i >= 0; i--)
            {
               DateTime f = fechaAncla.AddDays(-i).Date;
               diasList.Add(f);
               labelsList.Add(f.Day + " " + mesesCortos[f.Month]);
            }

            var mapa = new Dictionary<string, (int cantidad, double monto)>();
            int totalPedidos = 0;
            double totalMonto = 0;

            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT 
                         CAST(c.fecha AS DATE) AS dia,
                         COUNT(*) AS cantidad,
                         ROUND(SUM(c.monto_total), 2) AS monto
                     FROM cotiza c
                     WHERE c.es_pedido = 'S' AND c.estado = 'A'
                       AND c.vendedor_id = @vid
                       AND c.fecha >= @fDesde AND c.fecha < @fHasta
                     GROUP BY CAST(c.fecha AS DATE)";
                  cmd.Parameters.AddWithValue("@vid", vendedorId);
                  cmd.Parameters.AddWithValue("@fDesde", diasList[0]);
                  cmd.Parameters.AddWithValue("@fHasta", fechaAncla.AddDays(1).Date);

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        DateTime d = Convert.ToDateTime(reader["dia"]);
                        int cant = Convert.ToInt32(reader["cantidad"]);
                        double mto = Convert.ToDouble(reader["monto"]);
                        mapa[d.ToString("yyyy-MM-dd")] = (cant, mto);
                        totalPedidos += cant;
                        totalMonto += mto;
                     }
                  }
               }
            }

            var cantidades = new List<int>();
            var montos = new List<double>();
            foreach (var f in diasList)
            {
               string k = f.ToString("yyyy-MM-dd");
               if (mapa.ContainsKey(k))
               {
                  cantidades.Add(mapa[k].cantidad);
                  montos.Add(mapa[k].monto);
               }
               else
               {
                  cantidades.Add(0);
                  montos.Add(0.0);
               }
            }

            return Json(new
            {
               success = true,
               vendedorId = vendedorId,
               dias = dias,
               labels = labelsList,
               cantidades = cantidades,
               montos = montos,
               totalPedidos = totalPedidos,
               totalMonto = "S/ " + totalMonto.ToString("N2")
            });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      [HttpGet]
      public JsonResult ObtenerClientesInactivos(int meses = 5, string cid = "", int? vendedorId = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            int? sedeSesionFiltro = null;
            try
            {
               if (!string.IsNullOrEmpty(cid))
               {
                  var pDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
                  if (pDTO?.EmpresaSedeDTO != null && pDTO.EmpresaSedeDTO.EmpresaSedeId > 0)
                  {
                     sedeSesionFiltro = pDTO.EmpresaSedeDTO.EmpresaSedeId;
                  }
               }
            }
            catch { }

            int? vendedorIdFiltro = null;
            if (EsAdministrador())
            {
               vendedorIdFiltro = vendedorId.GetValueOrDefault() > 0 ? vendedorId.Value : (int?)null;
            }
            else
            {
               var (vid, _) = ObtenerVendedorUsuarioActual();
               vendedorIdFiltro = vid;
            }

            int mesesFiltro = new[] { 3, 4, 5, 6 }.Contains(meses) ? meses : 5;
            var lista = new List<ClienteInactivoDTO>();
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = ClientesInactivosSql;
                  cmd.Parameters.AddWithValue("@meses", mesesFiltro);
                  cmd.Parameters.AddWithValue("@vid", (object)vendedorIdFiltro ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionFiltro ?? DBNull.Value);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        DateTime fUltima = Convert.ToDateTime(reader["FechaUltimaCompra"]);
                        int diffMeses = reader["MesesSinComprar"] != DBNull.Value ? Convert.ToInt32(reader["MesesSinComprar"]) : mesesFiltro;
                        lista.Add(new ClienteInactivoDTO
                        {
                           Id = Convert.ToInt32(reader["ClienteId"]),
                           Codigo = reader["CodigoCliente"]?.ToString()?.Trim(),
                           Cliente = reader["ClienteNombre"]?.ToString()?.Trim(),
                           Vendedor = reader["VendedorNombre"]?.ToString()?.Trim(),
                           NumeroDocumentoIdentidad = reader["NumeroDocumentoIdentidad"]?.ToString()?.Trim(),
                           FechaUltimaCompra = fUltima,
                           FechaFormateada = fUltima.ToString("dd/MM/yyyy"),
                           MesesSinComprar = diffMeses
                        });
                     }
                  }
               }
            }
            return Json(new { success = true, meses = mesesFiltro, data = lista });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      private List<AlmacenItemDTO> ObtenerAlmacenesActivos(int? sedeSesionId = null)
      {
         var almacenes = new List<AlmacenItemDTO>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT almacen_id, nombre 
                     FROM dbo.almacen 
                     WHERE estado = 'A' 
                       AND (@sedeSesion IS NULL OR @sedeSesion = 0 OR empresa_sede_id = @sedeSesion) 
                     ORDER BY almacen_id";
                  cmd.Parameters.AddWithValue("@sedeSesion", (object)sedeSesionId ?? DBNull.Value);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        almacenes.Add(new AlmacenItemDTO
                        {
                           AlmacenId = Convert.ToInt32(reader["almacen_id"]),
                           Nombre = reader["nombre"]?.ToString()?.Trim()
                        });
                     }
                  }
               }
            }
         }
         catch { }
         return almacenes;
      }

      private List<ProductoStockDTO> ObtenerStockInsumosInterno(int? almacenId = null, int? sedeSesionId = null)
      {
         var lista = new List<ProductoStockDTO>();
         int sedeFiltro = (sedeSesionId.HasValue && sedeSesionId.Value > 0) ? sedeSesionId.Value : 1;
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     DECLARE @targetCodes TABLE (codigo VARCHAR(50), orden_fijo INT);
                     INSERT INTO @targetCodes VALUES 
                     ('FERLMP001003', 1),
                     ('FERHER005002', 2),
                     ('FERPYB001004', 3),
                     ('FERHER001021', 4),
                     ('FERHER001033', 5),
                     ('FERHER001034', 6),
                     ('FERPYB002020', 7),
                     ('FERPYB002021', 8),
                     ('FERPYB002022', 9);

                     WITH UltimoSaldoAlmacen AS (
                         SELECT 
                             s.producto_id,
                             s.almacen_id,
                             (s.cant_ant + s.cant_ing - s.cant_sal) AS saldo_alm,
                             ROW_NUMBER() OVER (
                                 PARTITION BY s.producto_id, s.almacen_id 
                                 ORDER BY pes.periodo_id DESC, s.mes_contable_id DESC
                             ) AS rn
                         FROM dbo.mov_alm_saldo s
                         INNER JOIN dbo.periodo_emp_sede pes ON s.periodo_emp_sede_id = pes.periodo_emp_sede_id
                         INNER JOIN dbo.producto p ON s.producto_id = p.producto_id
                         INNER JOIN @targetCodes tc ON RTRIM(p.producto) = tc.codigo
                         WHERE (@almacenId IS NULL OR @almacenId = 0 OR s.almacen_id = @almacenId)
                           AND pes.empresa_sede_id = @sedeSesion
                     ),
                     ConsolidadoSaldo AS (
                         SELECT 
                             producto_id,
                             SUM(saldo_alm) AS stock_total
                         FROM UltimoSaldoAlmacen
                         WHERE rn = 1
                         GROUP BY producto_id
                     ),
                     ConsumoMesActual AS (
                         SELECT 
                             map.producto_id,
                             SUM(map.cantidad) AS consumo_mes
                         FROM dbo.mov_alm_prod map
                         INNER JOIN dbo.mov_almacen ma ON map.mov_almacen_id = ma.mov_almacen_id
                         LEFT JOIN dbo.periodo_emp_sede pes ON ma.periodo_emp_sede_id = pes.periodo_emp_sede_id
                         INNER JOIN dbo.producto p ON map.producto_id = p.producto_id
                         INNER JOIN @targetCodes tc ON RTRIM(p.producto) = tc.codigo
                         WHERE ma.tipo_mov_alma_id = 2 -- Salida
                           AND ma.estado = 'A'
                           AND (@almacenId IS NULL OR @almacenId = 0 OR ma.almacen_id = @almacenId)
                           AND pes.empresa_sede_id = @sedeSesion
                           AND ma.fecha >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
                           AND ma.fecha < DATEADD(month, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))
                         GROUP BY map.producto_id
                     )
                     SELECT 
                         tc.orden_fijo,
                         p.producto_id,
                         p.producto AS codigo,
                         p.descripcion,
                         ISNULL(um.abrev, 'UND') AS unidad_medida,
                         ISNULL(cs.stock_total, 0) AS stock_actual,
                         ISNULL(avp.stock_min, 20) AS stock_min,
                         ISNULL(cm.consumo_mes, 0) AS consumo_mes
                     FROM @targetCodes tc
                     INNER JOIN dbo.producto p ON tc.codigo = RTRIM(p.producto)
                     LEFT JOIN dbo.alm_vol_prod avp ON p.producto_id = avp.producto_id AND avp.empresa_sede_id = @sedeSesion
                     LEFT JOIN dbo.unidad_medida um ON p.unidad_medida_id = um.unidad_medida_id
                     LEFT JOIN ConsolidadoSaldo cs ON p.producto_id = cs.producto_id
                     LEFT JOIN ConsumoMesActual cm ON p.producto_id = cm.producto_id
                     ORDER BY tc.orden_fijo ASC;";

                  cmd.Parameters.AddWithValue("@almacenId", (object)almacenId ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@sedeSesion", sedeFiltro);

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        decimal stock = reader["stock_actual"] != DBNull.Value ? Convert.ToDecimal(reader["stock_actual"]) : 0m;
                        decimal consumo = reader["consumo_mes"] != DBNull.Value ? Convert.ToDecimal(reader["consumo_mes"]) : 0m;
                        decimal rawStockMin = reader["stock_min"] != DBNull.Value ? Convert.ToDecimal(reader["stock_min"]) : 0m;
                        decimal stockMin = rawStockMin > 0 ? rawStockMin : 20m;

                        string estado;
                        string badge;
                        if (stock <= 0m)
                        {
                           estado = "Agotado";
                           badge = "danger";
                        }
                        else if (stock <= stockMin)
                        {
                           estado = "Stock Bajo";
                           badge = "warning";
                        }
                        else
                        {
                           estado = "Disponible";
                           badge = "success";
                        }

                        lista.Add(new ProductoStockDTO
                        {
                           Orden = Convert.ToInt32(reader["orden_fijo"]),
                           Codigo = reader["codigo"]?.ToString()?.Trim(),
                           ProductoId = reader["producto_id"] != DBNull.Value ? Convert.ToInt32(reader["producto_id"]) : 0,
                           Descripcion = reader["descripcion"]?.ToString()?.Trim(),
                           UnidadMedida = reader["unidad_medida"]?.ToString()?.Trim(),
                           StockActual = stock,
                           ConsumoMes = consumo,
                           StockActualFormateado = stock.ToString("N2"),
                           ConsumoMesFormateado = consumo.ToString("N2"),
                           EstadoStock = estado,
                           BadgeColor = badge
                        });
                     }
                  }
               }
            }
         }
         catch { }

         return lista;
      }

      [HttpGet]
      public JsonResult ObtenerStockInsumos(int? almacenId = null, string cid = "", int? sedeId = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            int? sedeSesionId = null;
            if (sedeId.HasValue && sedeId.Value > 0)
            {
               sedeSesionId = sedeId.Value;
            }
            else if (!string.IsNullOrEmpty(cid))
            {
               try
               {
                  var pDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
                  if (pDTO?.EmpresaSedeDTO != null && pDTO.EmpresaSedeDTO.EmpresaSedeId > 0)
                  {
                     sedeSesionId = pDTO.EmpresaSedeDTO.EmpresaSedeId;
                  }
               }
               catch { }
            }

            var data = ObtenerStockInsumosInterno(almacenId, sedeSesionId);
            string mesActual = DateTime.Today.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
            return Json(new { success = true, almacenId = almacenId ?? 0, mes = mesActual, data = data });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      private List<CategoriaFiltroItemDTO> ObtenerGruposActivos()
      {
         var grupos = new List<CategoriaFiltroItemDTO>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = "SELECT grupo_producto_id, nombre FROM dbo.grupo_producto WHERE estado = 'A' AND ISNULL(es_combo, 'N') <> 'S' AND ISNULL(tipo_producto_id, 1) <> 2 ORDER BY nombre";
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        grupos.Add(new CategoriaFiltroItemDTO
                        {
                           Id = Convert.ToInt32(reader["grupo_producto_id"]),
                           Nombre = reader["nombre"]?.ToString()?.Trim()
                        });
                     }
                  }
               }
            }
         }
         catch { }
         return grupos;
      }

      [HttpGet]
      public JsonResult ObtenerClasesPorGrupo(int? grupoId = null)
      {
         var clases = new List<CategoriaFiltroItemDTO>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT clase_producto_id, nombre 
                     FROM dbo.clase_producto 
                     WHERE estado = 'A' 
                       AND (@grupoId IS NULL OR @grupoId = 0 OR grupo_producto_id = @grupoId) 
                     ORDER BY nombre";
                  cmd.Parameters.AddWithValue("@grupoId", (object)grupoId ?? DBNull.Value);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        clases.Add(new CategoriaFiltroItemDTO
                        {
                           Id = Convert.ToInt32(reader["clase_producto_id"]),
                           Nombre = reader["nombre"]?.ToString()?.Trim()
                        });
                     }
                  }
               }
            }
            return Json(new { success = true, data = clases });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      [HttpGet]
      public JsonResult ObtenerFamiliasPorClase(int? claseId = null)
      {
         var familias = new List<CategoriaFiltroItemDTO>();
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT familia_producto_id, nombre 
                     FROM dbo.familia_producto 
                     WHERE estado = 'A' 
                       AND (@claseId IS NULL OR @claseId = 0 OR clase_producto_id = @claseId) 
                     ORDER BY nombre";
                  cmd.Parameters.AddWithValue("@claseId", (object)claseId ?? DBNull.Value);
                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        familias.Add(new CategoriaFiltroItemDTO
                        {
                           Id = Convert.ToInt32(reader["familia_producto_id"]),
                           Nombre = reader["nombre"]?.ToString()?.Trim()
                        });
                     }
                  }
               }
            }
            return Json(new { success = true, data = familias });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      private List<TopMenorStockDTO> ObtenerTopMenorStockInterno(int? grupoId = null, int? claseId = null, int? familiaId = null, int? sedeSesionId = null)
      {
         var lista = new List<TopMenorStockDTO>();
         int sedeFiltro = (sedeSesionId.HasValue && sedeSesionId.Value > 0) ? sedeSesionId.Value : 1;
         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     WITH SaldoAbierto AS (
                         SELECT 
                             mas.producto_id,
                             SUM((mas.cant_ant + mas.cant_ing) - (mas.cant_reserva + mas.cant_sal)) AS stock_total
                         FROM dbo.mov_alm_saldo mas
                         INNER JOIN dbo.mov_alm_cierre mac ON mac.periodo_emp_sede_id = mas.periodo_emp_sede_id
                             AND mac.almacen_id = mas.almacen_id
                             AND mac.mes_contable_id = mas.mes_contable_id
                         INNER JOIN dbo.estado_ape_cie eac ON eac.estado_ape_cie_id = mac.estado_ape_cie_id
                         LEFT JOIN dbo.periodo_emp_sede pes ON mac.periodo_emp_sede_id = pes.periodo_emp_sede_id
                         WHERE eac.estado_ape_cie = 'A'
                           AND pes.empresa_sede_id = @sedeId
                         GROUP BY mas.producto_id
                     )
                     SELECT TOP 10 
                         p.producto_id,
                         p.producto AS codigo,
                         p.descripcion AS nombre,
                         ISNULL(s.stock_total, 0) AS cantidad,
                         avp.stock_min,
                         ISNULL(um.abrev, 'UND') AS unidad_medida,
                         g.nombre AS grupo_nombre,
                         cl.nombre AS clase_nombre,
                         f.nombre AS familia_nombre
                     FROM dbo.alm_vol_prod avp
                     INNER JOIN dbo.producto p ON avp.producto_id = p.producto_id
                     LEFT JOIN SaldoAbierto s ON p.producto_id = s.producto_id
                     LEFT JOIN dbo.unidad_medida um ON p.unidad_medida_id = um.unidad_medida_id
                     LEFT JOIN dbo.familia_producto f ON p.familia_producto_id = f.familia_producto_id
                     LEFT JOIN dbo.clase_producto cl ON f.clase_producto_id = cl.clase_producto_id
                     LEFT JOIN dbo.grupo_producto g ON cl.grupo_producto_id = g.grupo_producto_id
                     WHERE avp.empresa_sede_id = @sedeId
                       AND avp.stock_min > 0
                       AND p.estado_producto_id = 1
                        AND ISNULL(g.es_combo, 'N') <> 'S'
                        AND ISNULL(g.tipo_producto_id, 1) <> 2
                       AND (@grupoId IS NULL OR @grupoId = 0 OR g.grupo_producto_id = @grupoId)
                       AND (@claseId IS NULL OR @claseId = 0 OR cl.clase_producto_id = @claseId)
                       AND (@familiaId IS NULL OR @familiaId = 0 OR f.familia_producto_id = @familiaId)
                     ORDER BY ISNULL(s.stock_total, 0) ASC, (ISNULL(s.stock_total, 0) - avp.stock_min) ASC, p.descripcion ASC;";

                  cmd.Parameters.AddWithValue("@sedeId", sedeFiltro);
                  cmd.Parameters.AddWithValue("@grupoId", (object)grupoId ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@claseId", (object)claseId ?? DBNull.Value);
                  cmd.Parameters.AddWithValue("@familiaId", (object)familiaId ?? DBNull.Value);

                  using (var reader = cmd.ExecuteReader())
                  {
                     int orden = 1;
                     while (reader.Read())
                     {
                        decimal cantidad = reader["cantidad"] != DBNull.Value ? Convert.ToDecimal(reader["cantidad"]) : 0m;
                        decimal stockMin = reader["stock_min"] != DBNull.Value ? Convert.ToDecimal(reader["stock_min"]) : 0m;
                        string estado;
                        string badge;
                        if (cantidad <= 0m)
                        {
                           estado = "Agotado";
                           badge = "danger";
                        }
                        else if (cantidad <= stockMin)
                        {
                           estado = "Stock Crítico";
                           badge = "warning";
                        }
                        else
                        {
                           estado = "Stock Bajo";
                           badge = "info";
                        }

                        lista.Add(new TopMenorStockDTO
                        {
                           Orden = orden++,
                           ProductoId = Convert.ToInt32(reader["producto_id"]),
                           Codigo = reader["codigo"]?.ToString()?.Trim(),
                           Nombre = reader["nombre"]?.ToString()?.Trim(),
                           Cantidad = cantidad,
                           CantidadFormateada = cantidad.ToString("N2"),
                           UnidadMedida = reader["unidad_medida"]?.ToString()?.Trim(),
                           GrupoNombre = reader["grupo_nombre"]?.ToString()?.Trim(),
                           ClaseNombre = reader["clase_nombre"]?.ToString()?.Trim(),
                           FamiliaNombre = reader["familia_nombre"]?.ToString()?.Trim(),
                           EstadoStock = estado,
                           BadgeColor = badge
                        });
                     }
                  }
               }
            }
         }
         catch { }

         return lista;
      }

      [HttpGet]
      public JsonResult ObtenerTopMenorStock(int? grupoId = null, int? claseId = null, int? familiaId = null, string cid = "", int? sedeId = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cid))
            {
               cid = Request.Query["cid"];
               if (string.IsNullOrEmpty(cid)) { cid = Request.Headers["x-app-cid"]; }
            }

            int? sedeSesionId = null;
            if (sedeId.HasValue && sedeId.Value > 0)
            {
               sedeSesionId = sedeId.Value;
            }
            else if (!string.IsNullOrEmpty(cid))
            {
               try
               {
                  var pDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
                  if (pDTO?.EmpresaSedeDTO != null && pDTO.EmpresaSedeDTO.EmpresaSedeId > 0)
                  {
                     sedeSesionId = pDTO.EmpresaSedeDTO.EmpresaSedeId;
                  }
               }
               catch { }
            }

            var data = ObtenerTopMenorStockInterno(grupoId, claseId, familiaId, sedeSesionId);
            return Json(new { success = true, data = data });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }

      private List<CalendarioAreaDTO> ObtenerAreasUsuarioAgenda(int usuarioId, bool esAdmin)
      {
         var areas = new List<CalendarioAreaDTO>();
         if (usuarioId <= 0) return areas;

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  if (esAdmin)
                  {
                     cmd.CommandText = @"
                        SELECT area_id, nombre, descripcion, color 
                        FROM dbo.calendario_area 
                        WHERE estado = 'A' 
                        ORDER BY orden ASC, nombre ASC";
                  }
                  else
                  {
                     cmd.CommandText = @"
                        INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal, fecha_creacion)
                        SELECT DISTINCT @usuarioId, m.area_id, 0, SYSDATETIME()
                        FROM dbo.rol_usuario ru
                        JOIN dbo.rol r ON ru.rol_id = r.rol_id
                        CROSS APPLY (
                            SELECT area_id FROM dbo.calendario_area
                            WHERE estado = 'A' AND (
                                (area_id = 2 AND (r.nombre LIKE '%Venta%' OR r.nombre LIKE '%Comercial%' OR r.rol IN ('00003','00004','00010','00017')))
                                OR (area_id = 3 AND (r.nombre LIKE '%Market%' OR r.rol IN ('00015')))
                                OR (area_id = 4 AND (r.nombre LIKE '%Producci%' OR r.rol IN ('00023')))
                                OR (area_id = 5 AND (r.nombre LIKE '%Almac%' OR r.nombre LIKE '%Log%stica%' OR r.nombre LIKE '%Despacho%' OR r.nombre LIKE '%Operacion%' OR r.rol IN ('00007','00011','00013','00014','00016','00018','00019','00021','00022')))
                                OR (area_id = 6 AND (r.nombre LIKE '%Contab%' OR r.nombre LIKE '%Tesor%' OR r.nombre LIKE '%Caja%' OR r.rol IN ('00005','00006','00008','00012')))
                                OR (area_id = 7 AND (r.nombre LIKE '%Capital Humano%' OR r.nombre LIKE '%RRHH%' OR r.rol IN ('00009')))
                                OR (area_id = 8 AND (r.rol = '00020' OR r.nombre = 'TI' OR r.nombre LIKE '%TI %' OR r.nombre LIKE '% TI' OR r.nombre LIKE '%Sistemas%'))
                                OR (area_id = 1 AND (r.rol = 'S0001' OR r.nombre LIKE '%Admin%'))
                            )
                        ) m
                        WHERE ru.usuario_id = @usuarioId
                          AND NOT EXISTS (
                              SELECT 1 FROM dbo.calendario_usuario_area cua 
                              WHERE cua.usuario_id = @usuarioId AND cua.area_id = m.area_id
                          );

                        IF EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @usuarioId)
                           AND NOT EXISTS (SELECT 1 FROM dbo.calendario_usuario_area WHERE usuario_id = @usuarioId AND es_principal = 1)
                        BEGIN
                            UPDATE TOP (1) dbo.calendario_usuario_area 
                            SET es_principal = 1 
                            WHERE usuario_id = @usuarioId;
                        END

                        SELECT a.area_id, a.nombre, a.descripcion, a.color 
                        FROM dbo.calendario_area a
                        INNER JOIN dbo.calendario_usuario_area cua ON a.area_id = cua.area_id
                        WHERE cua.usuario_id = @usuarioId AND a.estado = 'A'
                        ORDER BY cua.es_principal DESC, a.orden ASC, a.nombre ASC";
                     cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                  }

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        areas.Add(new CalendarioAreaDTO
                        {
                           AreaId = Convert.ToInt32(reader["area_id"]),
                           Nombre = reader["nombre"].ToString(),
                           Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "",
                           Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#0d6efd"
                        });
                     }
                  }

                  if (!esAdmin && areas.Count == 0)
                  {
                     cmd.Parameters.Clear();
                     cmd.CommandText = @"
                        SELECT TOP 1 area_id, nombre, descripcion, color 
                        FROM dbo.calendario_area 
                        WHERE estado = 'A' AND (area_id = 2 OR nombre LIKE '%Venta%') 
                        ORDER BY area_id ASC";
                     using (var reader = cmd.ExecuteReader())
                     {
                        if (reader.Read())
                        {
                           areas.Add(new CalendarioAreaDTO
                           {
                              AreaId = Convert.ToInt32(reader["area_id"]),
                              Nombre = reader["nombre"].ToString(),
                              Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : "",
                              Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : "#198754"
                           });
                        }
                     }
                  }
               }
            }
         }
         catch { }

         return areas;
      }

      [HttpGet]
      [HttpPost]
      public JsonResult ObtenerEventosAgenda(int? areaId = null)
      {
         var eventos = new List<object>();
         int usuarioActualId = ObtenerUsuarioId();
         if (usuarioActualId <= 0)
         {
            var uDto = ObtenerUsuarioDTO();
            if (uDto != null) usuarioActualId = uDto.UsuarioId;
         }

         bool esAdmin = EsAdministrador() || EsAdminOGerencia();
         var areasPermitidas = ObtenerAreasUsuarioAgenda(usuarioActualId, esAdmin);

         int idAreaFinal;
         if (esAdmin)
         {
            idAreaFinal = (areaId.HasValue && areaId.Value > 0)
               ? areaId.Value
               : (areasPermitidas.Count > 0 ? areasPermitidas[0].AreaId : 1);
         }
         else
         {
            if (areasPermitidas.Count == 0)
            {
               return Json(eventos);
            }
            idAreaFinal = (areaId.HasValue && areaId.Value > 0 && areasPermitidas.Any(a => a.AreaId == areaId.Value))
               ? areaId.Value
               : areasPermitidas[0].AreaId;
         }

         try
         {
            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT e.evento_id, e.area_id, e.usuario_id, e.usuario_logon, e.titulo, e.contexto,
                            e.fecha_inicio, e.fecha_fin, e.todo_el_dia, e.color, e.fecha_creacion,
                            ISNULL(a.nombre, 'General') AS area_nombre
                     FROM dbo.calendario_evento e
                     LEFT JOIN dbo.calendario_area a ON e.area_id = a.area_id
                     WHERE e.estado = 'A' AND e.area_id = @areaId
                     ORDER BY e.fecha_inicio ASC";
                  cmd.Parameters.AddWithValue("@areaId", idAreaFinal);

                  using (var reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        int id = Convert.ToInt32(reader["evento_id"]);
                        int propietarioId = Convert.ToInt32(reader["usuario_id"]);
                        string titulo = reader["titulo"].ToString();
                        string contexto = reader["contexto"] != DBNull.Value ? reader["contexto"].ToString() : "";
                        DateTime fInicio = Convert.ToDateTime(reader["fecha_inicio"]);
                        DateTime? fFin = reader["fecha_fin"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["fecha_fin"]) : null;
                        bool todoElDia = Convert.ToBoolean(reader["todo_el_dia"]);
                        string color = reader["color"].ToString();
                        string usuario = reader["usuario_logon"].ToString();
                        DateTime fCreacion = Convert.ToDateTime(reader["fecha_creacion"]);
                        string areaNombre = reader["area_nombre"].ToString();

                        eventos.Add(new
                        {
                           id = id.ToString(),
                           title = titulo,
                           start = todoElDia ? fInicio.ToString("yyyy-MM-dd") : fInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                           end = fFin.HasValue ? (todoElDia ? fFin.Value.ToString("yyyy-MM-dd") : fFin.Value.ToString("yyyy-MM-ddTHH:mm:ss")) : null,
                           allDay = todoElDia,
                           backgroundColor = color,
                           borderColor = color,
                           textColor = (color == "#ffc107") ? "#212529" : "#ffffff",
                           extendedProps = new
                           {
                              contexto = contexto,
                              usuario = usuario,
                              usuarioId = propietarioId,
                              areaId = idAreaFinal,
                              areaNombre = areaNombre,
                              fechaCreacion = fCreacion.ToString("dd/MM/yyyy HH:mm")
                           }
                        });
                     }
                  }
               }
            }
         }
         catch { }

         return Json(eventos);
      }

      [HttpPost]
      public JsonResult CambiarSesionCabecera(int? nuevoAnio, int? nuevaSedeId, string cidActual = null)
      {
         try
         {
            if (string.IsNullOrEmpty(cidActual))
            {
               cidActual = Request.Query["cid"];
               if (string.IsNullOrEmpty(cidActual)) { cidActual = Request.Headers["x-app-cid"]; }
            }

            int anioDestino = nuevoAnio ?? 2026;
            int sedeDestino = nuevaSedeId ?? 1; // 1 = Central, 2 = Ilo

            // EmpresaId: Por defecto 1 ("EMP. DE PRUEBA") o el del cid actual
            int empresaId = 1;
            if (!string.IsNullOrEmpty(cidActual))
            {
               int empIdExtraido = ObtenerEmpresaId(cidActual);
               if (empIdExtraido > 0)
               {
                  empresaId = empIdExtraido;
               }
            }

            // Validar directivas de usuario para sedes (Directiva 2) si no es administrador o gerencia
            int usuarioId = ObtenerUsuarioId();
            if (!EsAdminOGerencia() && usuarioId > 0)
            {
               using (var conn = new SqlConnection(GetDbConnectionString()))
               {
                  conn.Open();
                  using (var cmd = conn.CreateCommand())
                  {
                     cmd.CommandText = @"
                        SELECT valor 
                        FROM directiva_usuario 
                        WHERE usuario_id = @uid AND directiva_id = 2 AND estado = 'A'";
                     cmd.Parameters.AddWithValue("@uid", usuarioId);
                     var sedesPermitidas = new List<int>();
                     using (var r = cmd.ExecuteReader())
                     {
                        while (r.Read())
                        {
                           if (int.TryParse(r["valor"].ToString(), out int sVal))
                           {
                              sedesPermitidas.Add(sVal);
                           }
                        }
                     }

                     if (sedesPermitidas.Count > 0 && !sedesPermitidas.Contains(sedeDestino))
                     {
                        return Json(new { success = false, message = "No tiene permisos para acceder a la sede seleccionada." });
                     }
                  }
               }
            }

            int finalPeriodoId = 0;
            int finalPeriodoEmpSedeId = 0;

            using (var conn = new SqlConnection(GetDbConnectionString()))
            {
               conn.Open();
               using (var cmd = conn.CreateCommand())
               {
                  cmd.CommandText = @"
                     SELECT TOP 1 pes.periodo_emp_sede_id, pes.periodo_id
                     FROM dbo.periodo_emp_sede pes
                     INNER JOIN dbo.periodo p ON pes.periodo_id = p.periodo_id
                     INNER JOIN dbo.empresa_sede es ON pes.empresa_sede_id = es.empresa_sede_id
                     WHERE p.periodo = @anio
                       AND es.empresa_id = @empresaId
                       AND es.empresa_sede_id = @sedeId
                       AND pes.estado = 'A'";
                  cmd.Parameters.AddWithValue("@anio", anioDestino);
                  cmd.Parameters.AddWithValue("@empresaId", empresaId);
                  cmd.Parameters.AddWithValue("@sedeId", sedeDestino);

                  using (var reader = cmd.ExecuteReader())
                  {
                     if (reader.Read())
                     {
                        finalPeriodoEmpSedeId = Convert.ToInt32(reader["periodo_emp_sede_id"]);
                        finalPeriodoId = Convert.ToInt32(reader["periodo_id"]);
                     }
                  }
               }
            }

            if (finalPeriodoEmpSedeId == 0)
            {
               return Json(new { success = false, message = "No se encontró configuración para el año " + anioDestino + " y la sede seleccionada." });
            }

            // Generar nuevo CID: "{periodoId},{empresaId},{periodoEmpSedeId}"
            string nuevoCid = CreateCID(string.Concat(finalPeriodoId, ",", empresaId, ",", finalPeriodoEmpSedeId));

            string redirectUrl = Url.Action("Index", "Home", new { cid = nuevoCid });

            return Json(new
            {
               success = true,
               newCid = nuevoCid,
               redirectUrl = redirectUrl,
               anio = anioDestino,
               sedeId = sedeDestino
            });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = "Error al cambiar sesión: " + ex.Message });
         }
      }

      [HttpPost]
      public JsonResult GuardarTemaUsuario(string tema)
      {
         try
         {
            int usuarioId = ObtenerUsuarioId();
            if (usuarioId > 0 && !string.IsNullOrEmpty(tema))
            {
               UserPreferenceHelper.GuardarTemaUsuario(usuarioId, tema, HttpContext);
               return Json(new { success = true, tema = tema });
            }
            return Json(new { success = false, message = "Usuario no autenticado." });
         }
         catch (Exception ex)
         {
            return Json(new { success = false, message = ex.Message });
         }
      }
   }
}
