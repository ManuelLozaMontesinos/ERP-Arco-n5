-- ==============================================================================
-- Migración: Actualización de sp_tipo_cambio_insertar
-- Propósito: Permitir @tipoCambio opcional (NULL) y autogenerar el código
--            correlativo a 2 dígitos ('01', '02', '03', etc.) si no es suministrado
-- ==============================================================================

USE [esderp_v6]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_tipo_cambio_insertar]
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

	-- Si no se suministra el código o viene vacío, autogenerar correlativo a 2 dígitos ('01', '02', '03', ...)
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
