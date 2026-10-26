-- ==============================================================================
-- Migración: Actualización de procedimientos almacenados para Reloj Marcador
-- Propósito: Soportar parámetro @esCentral con valor por defecto 'N' en
--            sp_reloj_insertar y sp_reloj_actualizar, evitando el error
--            "expects parameter '@esCentral', which was not supplied"
-- ==============================================================================

USE [esderp_v6]
GO

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
