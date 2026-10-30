-- =========================================================================================
-- MASTER DEPLOYMENT SCRIPT PARA PRODUCCION
-- Base de Datos: esderp_v6
-- Fecha de Pase: 30/10/2026
-- Proyecto: ERP ARCO (ESD.ERP.Web.v6)
--
-- Contenido consolidado de todas las migraciones acumuladas desde 'erp-develop':
--   1. Tabla dbo.dashboard_rol_config (Persistencia de visibilidad del dashboard por rol)
--   2. Tabla dbo.usuario_preferencia (Preferencia de tema Dark/Light por usuario)
--   3. Módulo de Calendario: dbo.calendario_area y dbo.calendario_usuario_area
--   4. Módulo de Calendario: dbo.calendario_evento (recordatorios y áreas)
--   5. Módulo de Calendario: dbo.calendario_etiqueta (etiquetas arrastrables por área)
--   6. Procedimiento dbo.sp_centro_costo_buscarPor (Inclusión de columna 'sigla')
--   7. Tabla dbo.grupo_producto (Corrección de tipo SERVICIO: es_combo='N', es_insumo='N')
--   8. Procedimientos dbo.sp_reloj_insertar y sp_reloj_actualizar (Parámetro @esCentral='N')
--   9. Tabla dbo.reporte_expo (Registro de VT_LiquidacionCaja y VT_PedidoPorProducto)
--  10. Procedimiento dbo.sp_tipo_cambio_insertar (Autogeneración correlativa de código)
--  11. Procedimiento dbo.sp_tipo_remunera_eliminar (Soporte de @tipoRemuneraId)
-- =========================================================================================

USE [esderp_v6];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

PRINT '======================================================================';
PRINT '>>> INICIANDO MASTER DEPLOYMENT SCRIPT PARA PRODUCCION (esderp_v6)';
PRINT '======================================================================';
GO

-- -----------------------------------------------------------------------------------------
-- 1. TABLA: dbo.dashboard_rol_config
-- -----------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dashboard_rol_config')
BEGIN
    CREATE TABLE dbo.dashboard_rol_config (
        rol_codigo VARCHAR(50) NOT NULL,
        config_json NVARCHAR(MAX) NOT NULL,
        fecha_modifica DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT PK_dashboard_rol_config PRIMARY KEY CLUSTERED (rol_codigo)
    );
    PRINT '[1/11] OK: Tabla dbo.dashboard_rol_config creada exitosamente.';
END
ELSE
BEGIN
    PRINT '[1/11] OK: Tabla dbo.dashboard_rol_config ya existia.';
END
GO

-- -----------------------------------------------------------------------------------------
-- 2. TABLA: dbo.usuario_preferencia
-- -----------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'usuario_preferencia')
BEGIN
    CREATE TABLE dbo.usuario_preferencia (
        usuario_id INT NOT NULL,
        tema VARCHAR(20) NOT NULL CONSTRAINT DF_usuario_preferencia_tema DEFAULT ('light'),
        fecha_modificacion DATETIME NOT NULL CONSTRAINT DF_usuario_preferencia_fecha DEFAULT GETDATE(),
        CONSTRAINT PK_usuario_preferencia PRIMARY KEY CLUSTERED (usuario_id)
    );
    PRINT '[2/11] OK: Tabla dbo.usuario_preferencia creada exitosamente.';
END
ELSE
BEGIN
    PRINT '[2/11] OK: Tabla dbo.usuario_preferencia ya existia.';
END
GO

-- -----------------------------------------------------------------------------------------
-- 3. TABLAS DE CALENDARIO: dbo.calendario_area y dbo.calendario_usuario_area
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.calendario_area', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.calendario_area
    (
        area_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_calendario_area PRIMARY KEY,
        nombre NVARCHAR(200) NOT NULL,
        descripcion NVARCHAR(500) NULL,
        color NVARCHAR(40) NOT NULL CONSTRAINT DF_calendario_area_color DEFAULT (N'#0d6efd'),
        orden INT NOT NULL CONSTRAINT DF_calendario_area_orden DEFAULT (1),
        estado CHAR(1) NOT NULL CONSTRAINT DF_calendario_area_estado DEFAULT ('A'),
        fecha_creacion DATETIME2(0) NOT NULL CONSTRAINT DF_calendario_area_fecha_creacion DEFAULT (SYSDATETIME())
    );
    PRINT '[3/11] OK: Tabla dbo.calendario_area creada exitosamente.';
END
ELSE
BEGIN
    PRINT '[3/11] OK: Tabla dbo.calendario_area ya existia.';
END
GO

-- Poblar las 8 areas base si la tabla esta vacia
IF NOT EXISTS (SELECT 1 FROM dbo.calendario_area)
BEGIN
    INSERT INTO dbo.calendario_area (nombre, descripcion, color, orden)
    VALUES 
        (N'Administración', N'Área administrativa y gerencial', '#0d6efd', 1),
        (N'Ventas', N'Área comercial y de ventas', '#198754', 2),
        (N'Marketing', N'Área de marketing y publicidad', '#6f42c1', 3),
        (N'Producción', N'Área operativa y talleres de producción', '#d63384', 4),
        (N'Logística / Almacén', N'Área de compras, almacenes e inventarios', '#fd7e14', 5),
        (N'Contabilidad / Finanzas', N'Área contable, tributaria y tesorería', '#20c997', 6),
        (N'Capital Humano', N'Área de recursos humanos y planillas', '#0dcaf0', 7),
        (N'Sistemas / TI', N'Área de tecnología y soporte', '#6610f2', 8);
    PRINT '       Areas iniciales de calendario insertadas.';
END
GO

-- Normalizar encoding de nombres de areas existentes
UPDATE dbo.calendario_area SET nombre = N'Administración', descripcion = N'Área administrativa y gerencial' WHERE orden = 1 OR nombre LIKE '%Administraci%';
UPDATE dbo.calendario_area SET nombre = N'Producción', descripcion = N'Área operativa y talleres de producción' WHERE orden = 4 OR nombre LIKE '%Producci%';
UPDATE dbo.calendario_area SET nombre = N'Logística / Almacén', descripcion = N'Área de compras, almacenes e inventarios' WHERE orden = 5 OR nombre LIKE '%Log%stica%' OR nombre LIKE '%Almac%n%';
GO

IF OBJECT_ID(N'dbo.calendario_usuario_area', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.calendario_usuario_area
    (
        usuario_id INT NOT NULL,
        area_id INT NOT NULL,
        es_principal BIT NOT NULL CONSTRAINT DF_calendario_usuario_area_principal DEFAULT (0),
        fecha_creacion DATETIME2(0) NOT NULL CONSTRAINT DF_calendario_usuario_area_fecha DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_calendario_usuario_area PRIMARY KEY (usuario_id, area_id),
        CONSTRAINT FK_calendario_usuario_area_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id) ON DELETE CASCADE
    );
    PRINT '[3/11] OK: Tabla dbo.calendario_usuario_area creada exitosamente.';
END
ELSE
BEGIN
    PRINT '[3/11] OK: Tabla dbo.calendario_usuario_area ya existia.';
END
GO

-- Asignar todas las áreas iniciales a los administradores del sistema (roles S0001, S0002, 00020) y al usuario 26
INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal)
SELECT DISTINCT ru.usuario_id, a.area_id, CASE WHEN a.area_id = 1 THEN 1 ELSE 0 END
FROM dbo.rol_usuario ru
INNER JOIN dbo.rol r ON ru.rol_id = r.rol_id
CROSS JOIN dbo.calendario_area a
WHERE r.estado = 'A'
  AND (r.rol IN ('S0001', 'S0002', '00020') OR ru.usuario_id = 26)
  AND NOT EXISTS (
      SELECT 1 FROM dbo.calendario_usuario_area cua
      WHERE cua.usuario_id = ru.usuario_id AND cua.area_id = a.area_id
  );
GO

-- -----------------------------------------------------------------------------------------
-- 4. TABLA: dbo.calendario_evento
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.calendario_evento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.calendario_evento
    (
        evento_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_calendario_evento PRIMARY KEY,
        usuario_id INT NOT NULL,
        usuario_logon NVARCHAR(256) NOT NULL,
        titulo NVARCHAR(150) NOT NULL,
        contexto NVARCHAR(MAX) NULL,
        fecha_inicio DATETIME2(0) NOT NULL,
        fecha_fin DATETIME2(0) NULL,
        todo_el_dia BIT NOT NULL CONSTRAINT DF_calendario_evento_todo_el_dia DEFAULT (0),
        color NVARCHAR(20) NOT NULL CONSTRAINT DF_calendario_evento_color DEFAULT (N'#0d6efd'),
        estado CHAR(1) NOT NULL CONSTRAINT DF_calendario_evento_estado DEFAULT ('A'),
        fecha_creacion DATETIME2(0) NOT NULL CONSTRAINT DF_calendario_evento_fecha_creacion DEFAULT (SYSDATETIME()),
        fecha_modifica DATETIME2(0) NULL,
        recordatorio_estado CHAR(1) NOT NULL CONSTRAINT DF_calendario_evento_recordatorio_estado DEFAULT ('P'),
        recordatorio_reclamado_hasta DATETIME NULL,
        recordatorio_enviado_en DATETIME NULL,
        area_id INT NULL CONSTRAINT FK_calendario_evento_area FOREIGN KEY REFERENCES dbo.calendario_area(area_id)
    );
    PRINT '[4/11] OK: Tabla dbo.calendario_evento creada exitosamente.';
END
ELSE
BEGIN
    -- Asegurar columnas si la tabla ya existia
    IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_estado') IS NULL
        ALTER TABLE dbo.calendario_evento ADD recordatorio_estado CHAR(1) NOT NULL CONSTRAINT DF_calendario_evento_recordatorio_estado DEFAULT ('P') WITH VALUES;
    IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_reclamado_hasta') IS NULL
        ALTER TABLE dbo.calendario_evento ADD recordatorio_reclamado_hasta DATETIME NULL;
    IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_enviado_en') IS NULL
        ALTER TABLE dbo.calendario_evento ADD recordatorio_enviado_en DATETIME NULL;
    IF COL_LENGTH(N'dbo.calendario_evento', N'area_id') IS NULL
    BEGIN
        ALTER TABLE dbo.calendario_evento ADD area_id INT NULL;
        ALTER TABLE dbo.calendario_evento ADD CONSTRAINT FK_calendario_evento_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);
    END
    PRINT '[4/11] OK: Columnas de dbo.calendario_evento verificadas.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.calendario_evento') AND name = N'IX_calendario_evento_recordatorio')
BEGIN
    CREATE INDEX IX_calendario_evento_recordatorio
        ON dbo.calendario_evento (recordatorio_estado, fecha_inicio)
        INCLUDE (evento_id, usuario_id, titulo, todo_el_dia, estado);
END
GO

-- -----------------------------------------------------------------------------------------
-- 5. TABLA: dbo.calendario_etiqueta
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.calendario_etiqueta', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.calendario_etiqueta
    (
        etiqueta_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_calendario_etiqueta PRIMARY KEY,
        usuario_id INT NOT NULL,
        slot_index INT NOT NULL,
        titulo NVARCHAR(100) NULL,
        color NVARCHAR(20) NOT NULL CONSTRAINT DF_calendario_etiqueta_color DEFAULT ('#0d6efd'),
        icono NVARCHAR(50) NULL,
        fecha_modifica DATETIME2(0) NOT NULL CONSTRAINT DF_calendario_etiqueta_fecha DEFAULT (GETDATE()),
        area_id INT NOT NULL CONSTRAINT DF_calendario_etiqueta_area DEFAULT (1),
        CONSTRAINT UQ_calendario_etiqueta_area_slot UNIQUE (area_id, slot_index),
        CONSTRAINT FK_calendario_etiqueta_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id)
    );
    PRINT '[5/11] OK: Tabla dbo.calendario_etiqueta creada exitosamente.';
END
ELSE
BEGIN
    IF COL_LENGTH(N'dbo.calendario_etiqueta', N'area_id') IS NULL
    BEGIN
        ALTER TABLE dbo.calendario_etiqueta ADD area_id INT NOT NULL CONSTRAINT DF_calendario_etiqueta_area DEFAULT (1);
        ALTER TABLE dbo.calendario_etiqueta ADD CONSTRAINT FK_calendario_etiqueta_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);
    END
    PRINT '[5/11] OK: Tabla dbo.calendario_etiqueta verificada.';
END
GO

-- -----------------------------------------------------------------------------------------
-- 6. PROCEDIMIENTO: dbo.sp_centro_costo_buscarPor (Columna sigla)
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_centro_costo_buscarPor', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_centro_costo_buscarPor;
GO

CREATE PROCEDURE [dbo].[sp_centro_costo_buscarPor]
(
	@pageSize              INT,
	@orderBy               VARCHAR(100) = NULL,
	@empresaId             INT,
	@estado                CHAR(1) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@selectStat NVARCHAR(4000),
		@whereStat NVARCHAR(4000)

	SET @whereStat = ''

	SET @orderBy = NULLIF(@orderBy,'')
	SET @estado = NULLIF(@estado,'')

	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'cc.centro_costo_id,'
		+ 'cc.empresa_id,'
		+ 'cc.nombre,'
		+ 'cc.sigla,'
		+ 'cc.estado'
		+ CHAR(13) + 'FROM centro_costo cc'

	SET @whereStat = CHAR(13) + 'WHERE cc.empresa_id = @empresa_id'

	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND' ) + ' cc.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	EXEC sp_executesql @selectStat,
		N'@empresa_id INT,@estado CHAR(1)',
		@empresaId,@estado

	SET NOCOUNT OFF
END
GO
PRINT '[6/11] OK: Procedimiento dbo.sp_centro_costo_buscarPor actualizado (columna sigla).';
GO

-- -----------------------------------------------------------------------------------------
-- 7. TABLA: dbo.grupo_producto (Servicios: es_combo='N', es_insumo='N')
-- -----------------------------------------------------------------------------------------
UPDATE dbo.grupo_producto
SET es_combo = 'N',
    es_insumo = 'N'
WHERE tipo_producto_id = 2;
GO
PRINT '[7/11] OK: Registros de dbo.grupo_producto para tipo SERVICIO normalizados.';
GO

-- -----------------------------------------------------------------------------------------
-- 8. PROCEDIMIENTOS: dbo.sp_reloj_insertar y sp_reloj_actualizar (@esCentral = 'N')
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_reloj_insertar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_reloj_insertar;
GO

CREATE PROCEDURE [dbo].[sp_reloj_insertar]
(
	@empresaId             INT,
	@nombre                VARCHAR(100),
	@rutaArchivo           VARCHAR(100),
	@esCentral             CHAR(1) = 'N',
	@usuarioAccionId       INT,
	@newID                 INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@fechaAccion DATETIME,
		@estado CHAR(1)

	SET @fechaAccion = GETDATE()
	SET @estado = 'A'
	SET @esCentral = ISNULL(@esCentral, 'N')

	IF @esCentral = 'S'
	BEGIN
		UPDATE dbo.reloj
		SET es_central = 'N'
		WHERE empresa_id = @empresaId AND es_central = 'S'
	END

	SET @newID = NEXT VALUE FOR reloj_seq

	INSERT INTO dbo.reloj
	(
		reloj_id,empresa_id,nombre,ruta_archivo,es_central,
		estado,usuario_creacion_id,fecha_creacion
	)
	VALUES
	(
		@newID,@empresaId,@nombre,@rutaArchivo,@esCentral,
		@estado,@usuarioAccionId,@fechaAccion
	)

	SET NOCOUNT OFF
END
GO

IF OBJECT_ID('dbo.sp_reloj_actualizar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_reloj_actualizar;
GO

CREATE PROCEDURE [dbo].[sp_reloj_actualizar]
(
	@relojId               INT,
	@nombre                VARCHAR(100),
	@rutaArchivo           VARCHAR(100),
	@esCentral             CHAR(1) = 'N',
	@usuarioAccionId       INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@fechaAccion DATETIME,
		@empresaId INT

	SET @fechaAccion = GETDATE()
	SET @esCentral = ISNULL(@esCentral, 'N')

	SELECT @empresaId = empresa_id FROM dbo.reloj WHERE reloj_id = @relojId

	IF @esCentral = 'S' AND @empresaId IS NOT NULL
	BEGIN
		UPDATE dbo.reloj
		SET es_central = 'N'
		WHERE empresa_id = @empresaId AND reloj_id <> @relojId AND es_central = 'S'
	END

	UPDATE dbo.reloj
		SET
			nombre = @nombre,
			ruta_archivo = @rutaArchivo,
			es_central = @esCentral,
			usuario_modifica_id = @usuarioAccionId,
			fecha_modifica = @fechaAccion
	WHERE reloj_id = @relojId

	SET NOCOUNT OFF
END
GO
PRINT '[8/11] OK: Procedimientos dbo.sp_reloj_insertar y sp_reloj_actualizar actualizados (@esCentral).';
GO

-- -----------------------------------------------------------------------------------------
-- 9. TABLA: dbo.reporte_expo (Registro de procedimientos de fallback)
-- -----------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.reporte_expo WHERE reporte = 'VT_LiquidacionCaja')
BEGIN
    DECLARE @nextId_liq INT;
    SELECT @nextId_liq = ISNULL(MAX(reporte_expo_id), 0) + 1 FROM dbo.reporte_expo;
    INSERT INTO dbo.reporte_expo (reporte_expo_id, reporte, objeto_sql)
    VALUES (@nextId_liq, 'VT_LiquidacionCaja', 'sp_rptvt_liquidacionCaja');
    PRINT '[9/11] OK: Insertado VT_LiquidacionCaja en dbo.reporte_expo.';
END
ELSE
BEGIN
    PRINT '[9/11] OK: VT_LiquidacionCaja ya existia en dbo.reporte_expo.';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.reporte_expo WHERE reporte = 'VT_PedidoPorProducto')
BEGIN
    DECLARE @nextId_ped INT;
    SELECT @nextId_ped = ISNULL(MAX(reporte_expo_id), 0) + 1 FROM dbo.reporte_expo;
    INSERT INTO dbo.reporte_expo (reporte_expo_id, reporte, objeto_sql)
    VALUES (@nextId_ped, 'VT_PedidoPorProducto', 'sp_rptvt_registroPedidoProducto');
    PRINT '[9/11] OK: Insertado VT_PedidoPorProducto en dbo.reporte_expo.';
END
ELSE
BEGIN
    PRINT '[9/11] OK: VT_PedidoPorProducto ya existia en dbo.reporte_expo.';
END
GO

-- -----------------------------------------------------------------------------------------
-- 10. PROCEDIMIENTO: dbo.sp_tipo_cambio_insertar (Autogenerar correlativo)
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_tipo_cambio_insertar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_tipo_cambio_insertar;
GO

CREATE PROCEDURE [dbo].[sp_tipo_cambio_insertar]
(
	@tipoCambio            CHAR(2) = NULL,
	@nombre                VARCHAR(50),
	@origenRegistroId      INT,
	@usuarioAccionId       INT,
	@newID                 INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@fechaAccion DATETIME,
		@estado CHAR(1)

	SET @fechaAccion = GETDATE()
	SET @estado = 'A'

	-- Si no se suministra el codigo o viene vacio, autogenerar correlativo a 2 digitos ('01', '02', '03', ...)
	IF @tipoCambio IS NULL OR LTRIM(RTRIM(@tipoCambio)) = ''
	BEGIN
		SELECT @tipoCambio = RIGHT('0' + CAST(ISNULL(MAX(CASE WHEN ISNUMERIC(tipo_cambio) = 1 THEN CAST(tipo_cambio AS INT) ELSE 0 END), 0) + 1 AS VARCHAR(2)), 2)
		FROM dbo.tipo_cambio
	END

	SET @newID = NEXT VALUE FOR tipo_cambio_seq

	INSERT INTO dbo.tipo_cambio
	(
		tipo_cambio_id,tipo_cambio,nombre,origen_registro_id,estado,
		usuario_creacion_id,fecha_creacion
	)
	VALUES
	(
		@newID,@tipoCambio,@nombre,@origenRegistroId,@estado,
		@usuarioAccionId,@fechaAccion
	)

	SET NOCOUNT OFF
END
GO
PRINT '[10/11] OK: Procedimiento dbo.sp_tipo_cambio_insertar actualizado (autogeneracion de codigo).';
GO

-- -----------------------------------------------------------------------------------------
-- 11. PROCEDIMIENTO: dbo.sp_tipo_remunera_eliminar (@tipoRemuneraId / @tipoRemuneracionId)
-- -----------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_tipo_remunera_eliminar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_tipo_remunera_eliminar;
GO

CREATE PROCEDURE [dbo].[sp_tipo_remunera_eliminar]
(
	@tipoRemuneracionId    INT = NULL,
	@usuarioAccionId       INT = NULL,
	@tipoRemuneraId        INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@fechaAccion DATETIME,
		@estado CHAR(1)

	SET @fechaAccion = GETDATE()
	SET @estado = 'I'
	SET @tipoRemuneracionId = COALESCE(@tipoRemuneracionId, @tipoRemuneraId)

	UPDATE dbo.tipo_remunera
		SET
			estado = @estado,
			usuario_modifica_id = @usuarioAccionId,
			fecha_modifica = @fechaAccion
	WHERE tipo_remunera_id = @tipoRemuneracionId

	SET NOCOUNT OFF
END
GO
PRINT '[11/11] OK: Procedimiento dbo.sp_tipo_remunera_eliminar actualizado (compatibilidad de alias).';
GO

PRINT '======================================================================';
PRINT '>>> MASTER DEPLOYMENT SCRIPT COMPLETADO CON EXITO!';
PRINT '======================================================================';
GO

