-- ==============================================================================
-- Migración: Actualización de sp_centro_costo_buscarPor
-- Propósito: Incluir columna sigla en el SELECT para mostrarla en la bandeja
-- ==============================================================================

USE [esderp_v6]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_centro_costo_buscarPor]
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

	--// Verificar valores nulos.
	SET @orderBy = NULLIF(@orderBy,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
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

	--// Parámetros de filtro.
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND' ) + ' cc.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@empresa_id INT,@estado CHAR(1)',
		@empresaId,@estado

	SET NOCOUNT OFF
END
GO
