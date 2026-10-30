-- =========================================================================================
-- Script: ReportesExpoFallbackMigration.sql
-- Descripción: Registro de procedimientos de reporte en 'reporte_expo' para soporte
--              de exportación de datos y generación PDF fallback en Vista Previa
-- Fecha: 29/10/2026
-- =========================================================================================

USE esderp_v6;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.reporte_expo WHERE reporte = 'VT_LiquidacionCaja')
BEGIN
    DECLARE @nextId_liq INT;
    SELECT @nextId_liq = ISNULL(MAX(reporte_expo_id), 0) + 1 FROM dbo.reporte_expo;
    INSERT INTO dbo.reporte_expo (reporte_expo_id, reporte, objeto_sql)
    VALUES (@nextId_liq, 'VT_LiquidacionCaja', 'sp_rptvt_liquidacionCaja');
    PRINT 'Insertado VT_LiquidacionCaja en reporte_expo';
END
ELSE
BEGIN
    PRINT 'VT_LiquidacionCaja ya existe en reporte_expo';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.reporte_expo WHERE reporte = 'VT_PedidoPorProducto')
BEGIN
    DECLARE @nextId_ped INT;
    SELECT @nextId_ped = ISNULL(MAX(reporte_expo_id), 0) + 1 FROM dbo.reporte_expo;
    INSERT INTO dbo.reporte_expo (reporte_expo_id, reporte, objeto_sql)
    VALUES (@nextId_ped, 'VT_PedidoPorProducto', 'sp_rptvt_registroPedidoProducto');
    PRINT 'Insertado VT_PedidoPorProducto en reporte_expo';
END
ELSE
BEGIN
    PRINT 'VT_PedidoPorProducto ya existe en reporte_expo';
END
GO

