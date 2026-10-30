-- =========================================================================================
-- Script de Migración: Creación de tabla para persistencia de configuración del Dashboard por Rol
-- Base de datos: esderp_v6
-- Fecha: 2026-10-30
-- Descripción:
--   Permite que los administradores configuren la visibilidad de los widgets y KPIs
--   del dashboard por rol y que dicha configuración persista entre equipos y sesiones.
--   En producción, por defecto todos los widgets están desactivados para usuarios no administradores.
-- =========================================================================================

IF NOT EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.TABLES
    WHERE
        TABLE_SCHEMA = 'dbo'
        AND TABLE_NAME = 'dashboard_rol_config'
) BEGIN
CREATE TABLE dbo.dashboard_rol_config (
    rol_codigo VARCHAR(50) NOT NULL,
    config_json NVARCHAR (MAX) NOT NULL,
    fecha_modifica DATETIME NOT NULL DEFAULT GETDATE (),
    CONSTRAINT PK_dashboard_rol_config PRIMARY KEY CLUSTERED (rol_codigo)
);

PRINT 'Tabla dbo.dashboard_rol_config creada exitosamente.';

END ELSE BEGIN PRINT 'La tabla dbo.dashboard_rol_config ya existe.';

END