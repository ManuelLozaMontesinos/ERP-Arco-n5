-- ==============================================================================
-- Migración: Estructura de Áreas y Etiquetas para Calendario de Eventos
-- ==============================================================================

USE [esderp_v6]

-- 1. Tabla de Áreas del Calendario
IF OBJECT_ID ('dbo.calendario_area', 'U') IS NULL BEGIN
CREATE TABLE dbo.calendario_area (
    area_id INT IDENTITY (1, 1) PRIMARY KEY,
    nombre NVARCHAR (100) NOT NULL,
    descripcion NVARCHAR (250) NULL,
    color NVARCHAR (20) NOT NULL DEFAULT('#0d6efd'),
    orden INT NOT NULL DEFAULT(0),
    estado CHAR(1) NOT NULL DEFAULT('A'),
    fecha_creacion DATETIME2 NOT NULL DEFAULT(GETDATE ())
);

END

-- 2. Tabla de Relación Usuario - Área
IF OBJECT_ID (
    'dbo.calendario_usuario_area',
    'U'
) IS NULL BEGIN
CREATE TABLE dbo.calendario_usuario_area (
    usuario_id INT NOT NULL,
    area_id INT NOT NULL,
    es_principal BIT NOT NULL DEFAULT(0),
    fecha_creacion DATETIME2 NOT NULL DEFAULT(GETDATE ()),
    CONSTRAINT PK_calendario_usuario_area PRIMARY KEY (usuario_id, area_id),
    CONSTRAINT FK_calendario_usuario_area_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id)
);

END

-- 3. Tabla de Etiquetas Arrastrables por Usuario y Área
IF OBJECT_ID (
    'dbo.calendario_etiqueta',
    'U'
) IS NULL BEGIN
CREATE TABLE dbo.calendario_etiqueta (
    etiqueta_id INT IDENTITY (1, 1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    area_id INT NOT NULL DEFAULT(1),
    slot_index INT NOT NULL,
    titulo NVARCHAR (100) NULL,
    color NVARCHAR (20) NOT NULL DEFAULT('#0d6efd'),
    icono NVARCHAR (50) NULL,
    fecha_modifica DATETIME2 NOT NULL DEFAULT(GETDATE ()),
    CONSTRAINT FK_calendario_etiqueta_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id),
    CONSTRAINT UQ_calendario_etiqueta_area_slot UNIQUE (area_id, slot_index)
);
END
ELSE BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = 'calendario_etiqueta' AND COLUMN_NAME = 'area_id'
    ) BEGIN
        ALTER TABLE dbo.calendario_etiqueta ADD area_id INT NOT NULL DEFAULT(1);
    END

    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_calendario_etiqueta_user_slot') BEGIN
        ALTER TABLE dbo.calendario_etiqueta DROP CONSTRAINT UQ_calendario_etiqueta_user_slot;
    END

    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_calendario_etiqueta_user_area_slot') BEGIN
        ALTER TABLE dbo.calendario_etiqueta DROP CONSTRAINT UQ_calendario_etiqueta_user_area_slot;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_calendario_etiqueta_area') BEGIN
        ALTER TABLE dbo.calendario_etiqueta ADD CONSTRAINT FK_calendario_etiqueta_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);
    END

    IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'UQ_calendario_etiqueta_area_slot') BEGIN
        ALTER TABLE dbo.calendario_etiqueta ADD CONSTRAINT UQ_calendario_etiqueta_area_slot UNIQUE (area_id, slot_index);
    END
END

-- 4. Modificar dbo.calendario_evento para incluir area_id
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
        TABLE_NAME = 'calendario_evento'
        AND COLUMN_NAME = 'area_id'
) BEGIN
ALTER TABLE dbo.calendario_evento
ADD area_id INT NULL;

ALTER TABLE dbo.calendario_evento
ADD CONSTRAINT FK_calendario_evento_area FOREIGN KEY (area_id) REFERENCES dbo.calendario_area (area_id);

END

-- 5. Poblar Áreas predeterminadas si está vacía
IF NOT EXISTS (
    SELECT 1
    FROM dbo.calendario_area
) BEGIN
INSERT INTO
    dbo.calendario_area (
        nombre,
        descripcion,
        color,
        orden
    )
VALUES (
        'Administración',
        'Área administrativa y gerencial',
        '#0d6efd',
        1
    ),
    (
        'Ventas',
        'Área comercial y de ventas',
        '#198754',
        2
    ),
    (
        'Marketing',
        'Área de marketing y publicidad',
        '#6f42c1',
        3
    ),
    (
        'Producción',
        'Área operativa y talleres de producción',
        '#d63384',
        4
    ),
    (
        'Logística / Almacén',
        'Área de compras, almacenes e inventarios',
        '#fd7e14',
        5
    ),
    (
        'Contabilidad / Finanzas',
        'Área contable, tributaria y tesorería',
        '#20c997',
        6
    ),
    (
        'Capital Humano',
        'Área de recursos humanos y planillas',
        '#0dcaf0',
        7
    ),
    (
        'Sistemas / TI',
        'Área de tecnología y soporte',
        '#6610f2',
        8
    );

END

-- 6. Actualizar eventos existentes para que apunten a la primera área (Administración)
UPDATE dbo.calendario_evento SET area_id = 1 WHERE area_id IS NULL;
GO

-- 7. Asegurar relación para el usuario 26 (ploaiza) y administradores en todas las áreas
INSERT INTO
    dbo.calendario_usuario_area (
        usuario_id,
        area_id,
        es_principal
    )
SELECT
    26,
    a.area_id,
    CASE
        WHEN a.area_id = 1 THEN 1
        ELSE 0
    END
FROM dbo.calendario_area a
WHERE
    NOT EXISTS (
        SELECT 1
        FROM dbo.calendario_usuario_area cua
        WHERE
            cua.usuario_id = 26
            AND cua.area_id = a.area_id
    );
GO