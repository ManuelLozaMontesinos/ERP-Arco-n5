-- ==============================================================================
-- Migración: Actualización de sp_tipo_remunera_eliminar
-- Propósito: Aceptar tanto @tipoRemuneracionId como @tipoRemuneraId para evitar
--            errores de parámetro no suministrado.
-- ==============================================================================

USE [esderp_v6]
GO

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
