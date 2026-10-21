-- Run once against the application database before enabling the calendar.
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
        fecha_modifica DATETIME2(0) NULL
    );
END;

IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_estado') IS NULL
BEGIN
    ALTER TABLE dbo.calendario_evento
        ADD recordatorio_estado CHAR(1) NOT NULL
            CONSTRAINT DF_calendario_evento_recordatorio_estado DEFAULT ('P') WITH VALUES;
END;

IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_reclamado_hasta') IS NULL
BEGIN
    ALTER TABLE dbo.calendario_evento
        ADD recordatorio_reclamado_hasta DATETIME NULL;
END;

IF COL_LENGTH(N'dbo.calendario_evento', N'recordatorio_enviado_en') IS NULL
BEGIN
    ALTER TABLE dbo.calendario_evento
        ADD recordatorio_enviado_en DATETIME NULL;
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.calendario_evento')
      AND name = N'IX_calendario_evento_recordatorio'
)
BEGIN
    CREATE INDEX IX_calendario_evento_recordatorio
        ON dbo.calendario_evento (recordatorio_estado, fecha_inicio)
        INCLUDE (evento_id, usuario_id, titulo, todo_el_dia, estado);
END;
