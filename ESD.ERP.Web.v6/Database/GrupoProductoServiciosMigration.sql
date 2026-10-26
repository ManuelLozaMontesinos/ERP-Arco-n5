-- ==============================================================================
-- Migración: Actualización de grupo_producto para tipo de producto SERVICIO
-- Propósito: Garantizar que los grupos de tipo SERVICIO tengan es_combo = 'N'
--            y es_insumo = 'N' tal como en producción, para que sean listados
--            correctamente en Productos y Servicios.
-- ==============================================================================

USE [esderp_v6]
GO

UPDATE dbo.grupo_producto
SET es_combo = 'N',
    es_insumo = 'N'
WHERE tipo_producto_id = 2;
GO
