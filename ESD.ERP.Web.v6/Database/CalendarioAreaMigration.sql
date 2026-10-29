-- =========================================================================================
-- Script: CalendarioAreaMigration.sql
-- Descripción: Módulo de Calendario por Áreas y Gestión de Permisos de Usuarios
-- Fecha: 23/10/2026 (Actualizado con soporte explícito Unicode N'...')
-- =========================================================================================

USE esderp_v6;
GO

-- 1. Tabla de Áreas para el Calendario
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
END
GO

-- 2. Tabla de Relación Usuario - Áreas (Permisos de acceso y visibilidad)
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
END
GO

-- 3. Vincular Eventos a un Área específica
IF COL_LENGTH(N'dbo.calendario_evento', N'area_id') IS NULL
BEGIN
    ALTER TABLE dbo.calendario_evento
        ADD area_id INT NULL;

    ALTER TABLE dbo.calendario_evento
        ADD CONSTRAINT FK_calendario_evento_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);
END
GO

-- 4. Vincular Etiquetas Arrastrables a un Área específica
IF COL_LENGTH(N'dbo.calendario_etiqueta', N'area_id') IS NULL
BEGIN
    ALTER TABLE dbo.calendario_etiqueta
        ADD area_id INT NULL;

    ALTER TABLE dbo.calendario_etiqueta
        ADD CONSTRAINT FK_calendario_etiqueta_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);
END
GO

-- 5. Poblar Áreas predeterminadas si está vacía
IF NOT EXISTS (
    SELECT 1
    FROM dbo.calendario_area
)
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
END
GO

-- 6. Actualizar y corregir encoding de tildes en áreas existentes
UPDATE dbo.calendario_area SET nombre = N'Administración', descripcion = N'Área administrativa y gerencial' WHERE orden = 1 OR nombre LIKE '%Administraci%';
UPDATE dbo.calendario_area SET nombre = N'Ventas', descripcion = N'Área comercial y de ventas' WHERE orden = 2;
UPDATE dbo.calendario_area SET nombre = N'Marketing', descripcion = N'Área de marketing y publicidad' WHERE orden = 3;
UPDATE dbo.calendario_area SET nombre = N'Producción', descripcion = N'Área operativa y talleres de producción' WHERE orden = 4 OR nombre LIKE '%Producci%';
UPDATE dbo.calendario_area SET nombre = N'Logística / Almacén', descripcion = N'Área de compras, almacenes e inventarios' WHERE orden = 5 OR nombre LIKE '%Log%stica%' OR nombre LIKE '%Almac%n%';
UPDATE dbo.calendario_area SET nombre = N'Contabilidad / Finanzas', descripcion = N'Área contable, tributaria y tesorería' WHERE orden = 6;
UPDATE dbo.calendario_area SET nombre = N'Capital Humano', descripcion = N'Área de recursos humanos y planillas' WHERE orden = 7;
UPDATE dbo.calendario_area SET nombre = N'Sistemas / TI', descripcion = N'Área de tecnología y soporte' WHERE orden = 8;
GO

-- 7. Actualizar eventos existentes para que apunten a la primera área (Administración)
UPDATE dbo.calendario_evento SET area_id = 1 WHERE area_id IS NULL;
GO

-- 8. Asegurar relación para el usuario 26 (ploaiza) y administradores en todas las áreas
INSERT INTO dbo.calendario_usuario_area (usuario_id, area_id, es_principal)
SELECT 26, a.area_id, CASE WHEN a.area_id = 1 THEN 1 ELSE 0 END
FROM dbo.calendario_area a
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.calendario_usuario_area cua
    WHERE cua.usuario_id = 26 AND cua.area_id = a.area_id
);
GO
