-- =========================================================================================
-- SCRIPT DE MIGRACION PARA PRODUCCION - CORRECCION FILTRO RANGO DE FECHAS (DESDE - HASTA)
-- Base de Datos: esderp_v6
-- Fecha: 09/11/2026
-- Descripcion: 
--   1. Corrige la condicion AND/WHERE en 16 procedimientos almacenados para que el filtro
--      por rango de fechas 'Desde' y 'Hasta' funcione de manera desacoplada e inclusiva.
--   2. Corrige el error de tipos y comparacion con hora (CAST(fecha AS DATE)).
--   3. Corrige el bug critico de variable en sp_mov_reclasi_buscarPor (@fechaDesde / @fechaHasta).
--   4. Agrega los parametros y filtro de fechas a sp_orden_c_comp_buscarPor (Valorizacion de Compra).
-- =========================================================================================
USE [esderp_v6];
GO
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
PRINT 'Iniciando actualizacion de procedimientos almacenados...';
GO
-- -----------------------------------------------------------------------------------------
-- 1. Procedimiento: sp_pedido_co_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_pedido_co_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@periodoEmpresaSedeId      INT,
	@pedidoCompra              CHAR(5) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@tipoProductoId            INT = NULL,
	@tipoProducto              CHAR(1) = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@estadoAprobacion          CHAR(1) = NULL,
	@estadoPedidoCompra        CHAR(1) = NULL,
	@estado                    CHAR(1) = NULL
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
	SET @pedidoCompra = NULLIF(@pedidoCompra,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @tipoProductoId = NULLIF(@tipoProductoId,0)
	SET @tipoProducto = NULLIF(@tipoProducto,'')
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'pc.pedido_co_id,'
		+ 'pc.periodo_emp_sede_id,'
		+ 'pc.pedido_co,'
		+ 'pc.fecha,'
		+ 'pc.fecha_aproba,'
		+ 'pc.es_modifica,'
		+ 'pc.observa,'
		+ 'pc.estado_p_c_ant_id,'
		+ 'pc.estado,'
		+ 'tp.tipo_producto_id,'
		+ 'tp.nombre,'
		+ 'p.proveedor_id,'
		+ 'p.proveedor,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'p.cond_pago_id,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre,'
		+ 'epc.estado_pedido_co_id,'
		+ 'epc.estado_pedido_co,'
		+ 'epc.nombre'
		+ CHAR(13) + 'FROM pedido_co pc'
		+ CHAR(13) + 'INNER JOIN tipo_producto tp ON tp.tipo_producto_id = pc.tipo_producto_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = pc.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN estado_aproba ea ON ea.estado_aproba_id = pc.estado_aproba_id'
		+ CHAR(13) + 'INNER JOIN estado_pedido_co epc ON epc.estado_pedido_co_id = pc.estado_pedido_co_id'

	SET @whereStat = CHAR(13) + 'WHERE pc.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @pedidoCompra IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pc.pedido_co = @pedido_co'
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(pc.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(pc.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @tipoProductoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tp.tipo_producto_id = @tipo_producto_id'
	END
	IF @tipoProducto IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tp.tipo_producto = @tipo_producto'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estadoPedidoCompra IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' epc.estado_pedido_co = @estado_pedido_co'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pc.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@pedido_co CHAR(5),@fecha_desde DATETIME,@fecha_hasta DATETIME,@num_doc_iden VARCHAR(35),
		@tipo_producto_id INT,@tipo_producto CHAR(1),@estado_aproba CHAR(1),@estado_pedido_co CHAR(1),@estado CHAR(1)',
		@periodoEmpresaSedeId,@pedidoCompra,@fechaDesde,@fechaHasta,@numeroDocumentoIdentidad,
		@tipoProductoId,@tipoProducto,@estadoAprobacion,@estadoPedidoCompra,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_pedido_co_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 2. Procedimiento: sp_orden_co_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_orden_co_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@periodoEmpresaSedeId      INT,
	@ordenCompra               CHAR(5) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@tipoProductoId            INT = NULL,
	@tipoProducto              CHAR(1) = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@estadoAprobacion          CHAR(1) = NULL,
	@estadoOrdenCompraId       INT = NULL,
	@estadoOrdenCompraList     dbo.ListString READONLY,
	@estado                    CHAR(1) = NULL
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
	SET @ordenCompra = NULLIF(@ordenCompra,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @tipoProductoId = NULLIF(@tipoProductoId,0)
	SET @tipoProducto = NULLIF(@tipoProducto,'')
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estadoOrdenCompraId = NULLIF(@estadoOrdenCompraId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'oc.orden_co_id,'
		+ 'oc.periodo_emp_sede_id,'
		+ 'oc.orden_co,'
		+ 'oc.fecha,'
		+ 'oc.fecha_entrega,'
		+ 'oc.tipo_cambio,'
		+ 'oc.impuesto_tasa_id,'
		+ 'oc.monto_bruto,'
		+ 'oc.monto_impuesto,'
		+ 'oc.monto_total,'
		+ 'oc.plazo_entrega,'
		+ 'oc.lugar_entrega,'
		+ 'oc.cond_pago_id,'
		+ 'oc.observa,'
		+ 'oc.permite_pago_p,'
		+ 'oc.es_modifica,'
		+ 'oc.fecha_aproba,'
		+ 'oc.estado_oc_ant_id,'
		+ 'oc.estado,'
		+ 'tp.tipo_producto_id,'
		+ 'tp.nombre,'
		+ 'p.proveedor_id,'
		+ 'p.proveedor,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre,'
		+ 'eoc.estado_orden_co_id,'
		+ 'eoc.estado_orden_co,'
		+ 'eoc.nombre'
		+ CHAR(13) + 'FROM orden_co oc '
		+ CHAR(13) + 'INNER JOIN tipo_producto tp ON tp.tipo_producto_id = oc.tipo_producto_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = oc.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = oc.moneda_id'
		+ CHAR(13) + 'LEFT JOIN estado_aproba ea ON ea.estado_aproba_id = oc.estado_aproba_id'
		+ CHAR(13) + 'INNER JOIN estado_orden_co eoc ON eoc.estado_orden_co_id = oc.estado_orden_co_id'
		
	SET @whereStat = CHAR(13) + 'WHERE oc.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @ordenCompra IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' oc.orden_co = @orden_co'
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(oc.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(oc.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @tipoProductoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tp.tipo_producto_id = @tipo_producto_id'
	END
	IF @tipoProducto IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tp.tipo_producto = @tipo_producto'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estadoOrdenCompraId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' oc.estado_orden_co_id = @estado_orden_co_id'
	END
	IF EXISTS (SELECT 1 FROM @estadoOrdenCompraList)
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' eoc.estado_orden_co IN (SELECT [value] FROM @estado_orden_co_tvp)'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' oc.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	--print @selectStat
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@orden_co CHAR(5),@fecha_desde DATETIME,@fecha_hasta DATETIME,@num_doc_iden VARCHAR(35),
		@tipo_producto_id INT,@tipo_producto CHAR(1),@estado_aproba CHAR(1),@estado_orden_co_id INT,@estado_orden_co_tvp dbo.ListString READONLY,@estado CHAR(1)',
		@periodoEmpresaSedeId,@ordenCompra,@fechaDesde,@fechaHasta,@numeroDocumentoIdentidad,
		@tipoProductoId,@tipoProducto,@estadoAprobacion,@estadoOrdenCompraId,@estadoOrdenCompraList,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_orden_co_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 3. Procedimiento: sp_orden_c_comp_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_orden_c_comp_buscarPor]
(
	@pageSize                 INT,
	@orderBy                  VARCHAR(100) = NULL,
	@periodoId                INT = NULL,
	@ordenCompraId            INT = NULL,
	@ordenCompra              VARCHAR(5) = NULL,
	@containsRazonSocial      VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad VARCHAR(35) = NULL,
	@tipoDocumentoComercialId INT = NULL,
	@serie                    CHAR(4) = NULL,
	@numero                   CHAR(8) = NULL,
	@registroReferencialId    INT = NULL,
	@condicionPagoId          INT = NULL,
	@estado                   CHAR(1) = NULL,
	@fechaDesde               DATETIME = NULL,
	@fechaHasta               DATETIME = NULL
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
	SET @periodoId = NULLIF(@periodoId,0)
	SET @ordenCompraId = NULLIF(@ordenCompraId,0)
	SET @ordenCompra = NULLIF(@ordenCompra,0)
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @tipoDocumentoComercialId = NULLIF(@tipoDocumentoComercialId,0)
	SET @serie = NULLIF(@serie,'')
	SET @numero = NULLIF(@numero,'')
	SET @registroReferencialId = NULLIF(@registroReferencialId,0)
	SET @condicionPagoId = NULLIF(@condicionPagoId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'occ.orden_c_comp_id,'
		+ 'occ.serie,'
		+ 'occ.numero,'
		+ 'occ.fecha,'
		+ 'occ.fecha_vcto,'
		+ 'occ.monto_bruto,'
		+ 'occ.monto_impuesto,'
		+ 'occ.monto_total,'
		+ 'occ.reg_refe_id,'
		+ 'occ.estado,'
		+ 'oc.orden_co_id,'
		+ 'oc.orden_co,'
		+ 'oc.cond_pago_id,'
		+ 'p.proveedor_id,'
		+ 'p.proveedor,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'cp.cond_pago_id,'
		+ 'cp.cond_pago,'
		+ 'cp.nombre'
		+ CHAR(13) + 'FROM orden_c_comp occ'
		+ CHAR(13) + 'INNER JOIN orden_co oc ON oc.orden_co_id = occ.orden_co_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = oc.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = oc.moneda_id'
		+ CHAR(13) + 'INNER JOIN periodo_emp_sede pes ON pes.periodo_emp_sede_id = oc.periodo_emp_sede_id'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = occ.tipo_doc_com_id'
		+ CHAR(13) + 'LEFT JOIN cond_pago cp ON cp.cond_pago_id = occ.cond_pago_id'

	--// ParÃ¡metros de filtro.
	IF @periodoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pes.periodo_id = @periodo_id'
	END
	IF @ordenCompraId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' occ.orden_co_id = @orden_co_id'
	END
	IF @ordenCompra IS NOT NULL
	BEGIN 
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' oc.orden_co = @ordenCompra'
	END
	IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(occ.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(occ.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @tipoDocumentoComercialId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com_id = @tipo_doc_com_id'
	END
	IF @serie IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' occ.serie = @serie'
	END
	IF @numero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' occ.numero = @numero'
	END
	IF @registroReferencialId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' occ.reg_refe_id = @reg_refe_id'
	END
	IF @condicionPagoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' cp.cond_pago_id = @condicionPagoId'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' occ.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinÃ¡mica.
	EXEC sp_executesql @selectStat,
		N'@periodo_id INT,@orden_co_id INT,@ordenCompra VARCHAR(5),@num_doc_iden VARCHAR(35),@tipo_doc_com_id INT,@serie CHAR(4),
		@numero CHAR(10),@reg_refe_id INT,@condicionPagoId INT,@estado CHAR(1),@fecha_desde DATETIME,@fecha_hasta DATETIME',
		@periodoId,@ordenCompraId,@ordenCompra,@numeroDocumentoIdentidad,@tipoDocumentoComercialId,@serie,
		@numero,@registroReferencialId,@condicionPagoId,@estado,@fechaDesde,@fechaHasta

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_orden_c_comp_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 4. Procedimiento: sp_mov_almacen_buscarPorEmpresa
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_mov_almacen_buscarPorEmpresa]
(
	@pageSize                         INT,
	@orderBy                          VARCHAR(100) = NULL,
	@periodoId                        INT,
	@empresaId                        INT,
	@almacenId                        INT = NULL,
	@transaccionAlmacenId             INT = NULL,
	@transaccionAlmacen               CHAR(3) = NULL,
	@tipoMovimientoAlmacenId          INT = NULL,
	@tipoMovimientoAlmacen            CHAR(1) = NULL,
	@containsRazonSocial              VARCHAR(150) = NULL,
	@numeroRuc                        VARCHAR(35) = NULL,
	@containsMovimientoAlmacen        VARCHAR(10) = NULL,
	@containsNumeroGuiaRemision       VARCHAR(20) = NULL,
	@almacenDestinoId                 INT = NULL,
	@fechaDesde                       DATETIME = NULL,
	@fechaHasta                       DATETIME = NULL,
	@comprobanteId                    INT = NULL,
	@estadoMovimientoAlmacenId        INT = NULL,
	@estadoMovimientoAlmacen          CHAR(1) = NULL,
	@estadoMovimientoAlmacenList      dbo.ListString READONLY,
	@estado                           CHAR(1) = NULL
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
	SET @almacenId = NULLIF(@almacenId,0)
	SET @transaccionAlmacenId = NULLIF(@transaccionAlmacenId,0)
	SET @transaccionAlmacen = NULLIF(@transaccionAlmacen,'')
	SET @tipoMovimientoAlmacenId = NULLIF(@tipoMovimientoAlmacenId,0)
	SET @tipoMovimientoAlmacen = NULLIF(@tipoMovimientoAlmacen,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroRuc = NULLIF(@numeroRuc,'')
	SET @containsMovimientoAlmacen = NULLIF(@containsMovimientoAlmacen,'')
	SET @containsNumeroGuiaRemision = NULLIF(@containsNumeroGuiaRemision,'')
	SET @almacenDestinoId = NULLIF(@almacenDestinoId,0)
	SET @comprobanteId = NULLIF(@comprobanteId,0)
	SET @estadoMovimientoAlmacenId = NULLIF(@estadoMovimientoAlmacenId,0)
	SET @estadoMovimientoAlmacen = NULLIF(@estadoMovimientoAlmacen,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'ma.mov_almacen_id,'
		+ 'ma.periodo_emp_sede_id,'
		+ 'ma.mov_almacen,'
		+ 'ma.almacen_destino_id,'
		+ 'ma.fecha,'
		+ 'ma.orden_cs_id,'
		+ 'ma.orden_cs,'
		+ 'ma.nro_guia_rem,'
		+ 'ma.comprobante_id,'
		+ 'ma.serie_doc,'
		+ 'ma.numero_doc,'
		+ 'ma.tiene_guia_rem,'
		+ 'ma.g_r_en_modi,'
		+ 'ma.personal_id,'
		+ 'ma.centro_costo_id,'
		+ 'ma.perso_auto_id,'
		+ 'ma.afecta_saldo,'
		+ 'ma.estado,'
		+ 'a.almacen_id,'
		+ 'a.nombre,'
		+ 'ta.trans_alma_id,'
		+ 'ta.nombre,'
		+ 'tma.tipo_mov_alma_id,'
		+ 'tma.tipo_mov_alma,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'ad.almacen_id,'
		+ 'ad.nombre,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'tdc.codigo_sunat,'
		+ 'ema.estado_mov_alm_id,'
		+ 'ema.nombre,'
		+ 'u.usuario_id,'
		+ 'u.usuario_logon'
		+ CHAR(13) + 'FROM mov_almacen ma'
		+ CHAR(13) + 'INNER JOIN periodo_emp_sede pes ON pes.periodo_emp_sede_id = ma.periodo_emp_sede_id'
		+ CHAR(13) + 'INNER JOIN empresa_sede es ON es.empresa_sede_id = pes.empresa_sede_id'
		+ CHAR(13) + 'INNER JOIN almacen a ON a.almacen_id = ma.almacen_id'
		+ CHAR(13) + 'INNER JOIN trans_alma ta ON ta.trans_alma_id = ma.trans_alma_id'
		+ CHAR(13) + 'INNER JOIN tipo_mov_alma tma ON tma.tipo_mov_alma_id = ma.tipo_mov_alma_id'
		+ CHAR(13) + 'LEFT JOIN proveedor p ON p.proveedor_id = ma.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN almacen ad ON ad.almacen_id = ma.almacen_destino_id'
		+ CHAR(13) + 'LEFT JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = ma.tipo_doc_com_id'
		+ CHAR(13) + 'INNER JOIN estado_mov_alm ema ON ema.estado_mov_alm_id = ma.estado_mov_alm_id'
		+ CHAR(13) + 'INNER JOIN usuario u ON u.usuario_id = ma.usuario_creacion_id'

	SET @whereStat = CHAR(13) + 'WHERE pes.periodo_id = @periodo_id AND es.empresa_id = @empresa_id'

	--// Parámetros de filtro.
	IF @almacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.almacen_id = @almacen_id'
   END
	IF @transaccionAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.trans_alma_id = @trans_alma_id'
   END
	IF @transaccionAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.trans_alma = @trans_alma'
   END
	IF @tipoMovimientoAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tma.tipo_mov_alma_id = @tipo_mov_alma_id'
   END
	IF @tipoMovimientoAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tma.tipo_mov_alma = @tipo_mov_alma'
   END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroRuc IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.numero_ruc = @numero_ruc'
	END
	IF @containsMovimientoAlmacen IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.mov_almacen LIKE ''%' + @containsMovimientoAlmacen + '%'''
	END
	IF @almacenDestinoId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.almacen_destino_id = @almacen_destino_id'
   END
	IF @containsNumeroGuiaRemision IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.nro_guia_rem LIKE ''%' + @containsNumeroGuiaRemision + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(ma.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(ma.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @comprobanteId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.comprobante_id = @comprobante_id'
   END
	IF @estadoMovimientoAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm_id = @estado_mov_alm_id'
   END
	IF @estadoMovimientoAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm = @estado_mov_alm'
   END
	IF EXISTS (SELECT 1 FROM @estadoMovimientoAlmacenList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm IN (SELECT [value] FROM @estado_mov_alm_tvp)'
   END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_id INT,@empresa_id INT,@almacen_id INT,@trans_alma_id INT,@trans_alma CHAR(3),@tipo_mov_alma_id INT,
		@tipo_mov_alma CHAR(1),@numero_ruc VARCHAR(35),@almacen_destino_id INT,@fecha_desde DATETIME,@fecha_hasta DATETIME,@comprobante_id INT,
		@estado_mov_alma_id INT,@estado_mov_alma CHAR(1),@estado_mov_alm_tvp dbo.ListString READONLY,@estado CHAR(1)',
		@periodoId,@empresaId,@almacenId,@transaccionAlmacenId,@transaccionAlmacen,@tipoMovimientoAlmacenId,
		@tipoMovimientoAlmacen,@numeroRuc,@almacenDestinoId,@fechaDesde,@fechaHasta,@comprobanteId,
		@estadoMovimientoAlmacenId,@estadoMovimientoAlmacen,@estadoMovimientoAlmacenList,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_mov_almacen_buscarPorEmpresa actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 5. Procedimiento: sp_mov_almacen_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_mov_almacen_buscarPor]
(
	@pageSize                         INT,
	@orderBy                          VARCHAR(100) = NULL,
	@periodoEmpresaSedeId             INT,
	@almacenId                        INT = NULL,
	@transaccionAlmacenId             INT = NULL,
	@transaccionAlmacen               CHAR(3) = NULL,
	@transaccionAlmacenList           dbo.ListString READONLY,
	@notTransaccionAlmacenList        dbo.ListString READONLY,
	@tipoMovimientoAlmacenId          INT = NULL,
	@tipoMovimientoAlmacen            CHAR(1) = NULL,
	@containsRazonSocial              VARCHAR(150) = NULL,
	@numeroRuc                        VARCHAR(35) = NULL,
	@containsMovimientoAlmacen        VARCHAR(10) = NULL,
	@containsNumeroGuiaRemision       VARCHAR(20) = NULL,
	@containsNumeroDocumento          VARCHAR(20) = NULL,
	@almacenDestinoId                 INT = NULL,
	@movimientoAlmacenReferencialId   INT = NULL,
	@fechaDesde                       DATETIME = NULL,
	@fechaHasta                       DATETIME = NULL,
	@comprobanteId                    INT = NULL,
	@tieneGuiaRemision                CHAR(1) = NULL,
	@guiaRemisionEnModificacion       CHAR(1) = NULL,
	@containsPersonal                 VARCHAR(150) = NULL,
	@centroCostoId                    INT = NULL,
	@movimientoReclasificacionId      INT = NULL,
	@estadoAprobacionId               INT = NULL,
	@estadoAprobacion                 CHAR(1) = NULL,
	@estadoMovimientoAlmacenId        INT = NULL,
	@estadoMovimientoAlmacen          CHAR(1) = NULL,
	@estadoMovimientoAlmacenList      dbo.ListString READONLY,
	@esEditable												CHAR(1) = NULL,
	@estado                           CHAR(1) = NULL
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
	SET @almacenId = NULLIF(@almacenId,0)
	SET @transaccionAlmacenId = NULLIF(@transaccionAlmacenId,0)
	SET @transaccionAlmacen = NULLIF(@transaccionAlmacen,'')
	SET @tipoMovimientoAlmacenId = NULLIF(@tipoMovimientoAlmacenId,0)
	SET @tipoMovimientoAlmacen = NULLIF(@tipoMovimientoAlmacen,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroRuc = NULLIF(@numeroRuc,'')
	SET @containsMovimientoAlmacen = NULLIF(@containsMovimientoAlmacen,'')
	SET @containsNumeroGuiaRemision = NULLIF(@containsNumeroGuiaRemision,'')
	SET @containsNumeroDocumento = NULLIF(@containsNumeroDocumento,'')
	SET @almacenDestinoId = NULLIF(@almacenDestinoId,0)
	SET @movimientoAlmacenReferencialId = NULLIF(@movimientoAlmacenReferencialId,0)
	SET @comprobanteId = NULLIF(@comprobanteId,0)
	SET @tieneGuiaRemision = NULLIF(@tieneGuiaRemision,'')
	SET @guiaRemisionEnModificacion = NULLIF(@guiaRemisionEnModificacion,'')
	SET @containsPersonal = NULLIF(@containsPersonal,'')
	SET @centroCostoId = NULLIF(@centroCostoId,0)
	SET @estadoAprobacionId = NULLIF(@estadoAprobacionId,0)
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @movimientoReclasificacionId = NULLIF(@movimientoReclasificacionId,0)
	SET @estadoMovimientoAlmacenId = NULLIF(@estadoMovimientoAlmacenId,0)
	SET @estadoMovimientoAlmacen = NULLIF(@estadoMovimientoAlmacen,'')
	SET @esEditable = NULLIF(@esEditable,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'ma.mov_almacen_id,'
		+ 'ma.periodo_emp_sede_id,'
		+ 'ma.mov_almacen,'
		+ 'ma.almacen_destino_id,'
		+ 'ma.mov_alma_ref_id,'
		+ 'ma.fecha,'
		+ 'ma.orden_cs_id,'
		+ 'ma.orden_cs,'
		+ 'ma.nro_guia_rem,'
		+ 'ma.comprobante_id,'
		+ 'ma.serie_doc,'
		+ 'ma.numero_doc,'
		+ 'ma.tiene_guia_rem,'
		+ 'ma.g_r_en_modi,'
		+ 'ma.perso_auto_id,'
		+ 'ma.fecha_auto,'
		+ 'ma.afecta_saldo,'
		+ 'ma.es_editable,'
		+ 'ma.estado,'
		+ 'a.almacen_id,'
		+ 'a.nombre,'
		+ 'a.codigo_sunat,'
		+ 'ta.trans_alma_id,'
		+ 'ta.trans_alma,'
		+ 'ta.nombre,'
		+ 'tma.tipo_mov_alma_id,'
		+ 'tma.tipo_mov_alma,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'ad.almacen_id,'
		+ 'ad.nombre,'
		+ 'ad.codigo_sunat,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'tdc.codigo_sunat,'
		+ 'per.personal_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'cc.centro_costo_id,'
		+ 'cc.nombre,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre,'
		+ 'per3.personal_id,'
		+ 'p3.proveedor_id,'
		+ 'p3.razon_social,'
		+ 'ema.estado_mov_alm_id,'
		+ 'ema.nombre,'
		+ 'u.usuario_id,'
		+ 'u.usuario_logon'
		+ CHAR(13) + 'FROM mov_almacen ma'
		+ CHAR(13) + 'INNER JOIN almacen a ON a.almacen_id = ma.almacen_id'
		+ CHAR(13) + 'INNER JOIN trans_alma ta ON ta.trans_alma_id = ma.trans_alma_id'
		+ CHAR(13) + 'INNER JOIN tipo_mov_alma tma ON tma.tipo_mov_alma_id = ma.tipo_mov_alma_id'
		+ CHAR(13) + 'LEFT JOIN proveedor p ON p.proveedor_id = ma.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN almacen ad ON ad.almacen_id = ma.almacen_destino_id'
		+ CHAR(13) + 'LEFT JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = ma.tipo_doc_com_id'
		+ CHAR(13) + 'LEFT JOIN personal per ON per.personal_id = ma.personal_id'
		+ CHAR(13) + 'LEFT JOIN proveedor p2 ON p2.proveedor_id = per.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN centro_costo cc ON cc.centro_costo_id = ma.centro_costo_id'
		+ CHAR(13) + 'LEFT JOIN estado_aproba ea ON ea.estado_aproba_id = ma.estado_aproba_id'
		+ CHAR(13) + 'LEFT JOIN personal per3 ON per3.personal_id = ma.perso_auto_id'
		+ CHAR(13) + 'LEFT JOIN proveedor p3 ON p3.proveedor_id = per3.proveedor_id'
		+ CHAR(13) + 'INNER JOIN estado_mov_alm ema ON ema.estado_mov_alm_id = ma.estado_mov_alm_id'
		+ CHAR(13) + 'INNER JOIN usuario u ON u.usuario_id = ma.usuario_creacion_id'

	SET @whereStat = CHAR(13) + 'WHERE ma.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @almacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.almacen_id = @almacen_id'
   END
	IF @transaccionAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.trans_alma_id = @trans_alma_id'
   END
	IF @transaccionAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.trans_alma = @trans_alma'
   END
	IF EXISTS (SELECT 1 FROM @transaccionAlmacenList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.trans_alma IN (SELECT [value] FROM @trans_alma_tvp)'
   END
	IF EXISTS (SELECT 1 FROM @notTransaccionAlmacenList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' NOT ta.trans_alma IN (SELECT [value] FROM @not_trans_alma_tvp)'
   END
	IF @tipoMovimientoAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tma.tipo_mov_alma_id = @tipo_mov_alma_id'
   END
	IF @tipoMovimientoAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tma.tipo_mov_alma = @tipo_mov_alma'
   END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroRuc IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.numero_ruc = @numero_ruc'
	END
	IF @containsMovimientoAlmacen IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.mov_almacen LIKE ''%' + @containsMovimientoAlmacen + '%'''
	END
	IF @almacenDestinoId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.almacen_destino_id = @almacen_destino_id'
   END
	IF @movimientoAlmacenReferencialId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.mov_alma_ref_id = @mov_alma_ref_id'
   END
	IF @containsNumeroGuiaRemision IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.nro_guia_rem LIKE ''%' + @containsNumeroGuiaRemision + '%'''
	END
	IF @containsNumeroDocumento IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.numero_doc LIKE ''%' + @containsNumeroDocumento + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(ma.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(ma.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @comprobanteId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.comprobante_id = @comprobante_id'
   END
	IF @tieneGuiaRemision IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.tiene_guia_rem = @tiene_guia_rem'
   END
	IF @guiaRemisionEnModificacion IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.g_r_en_modi = @g_r_en_modi'
   END
	IF @containsPersonal IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsPersonal + '%'''
	END
	IF @centroCostoId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.centro_costo_id = @centro_costo_id'
   END
	IF @movimientoReclasificacionId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.mov_reclasi_id = @mov_reclasi_id'
   END
	IF @estadoAprobacionId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba_id = @estado_aproba_id'
   END
	IF @estadoAprobacion IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
   END
	IF @estadoMovimientoAlmacenId IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm_id = @estado_mov_alm_id'
   END
	IF @estadoMovimientoAlmacen IS NOT NULL
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm = @estado_mov_alm'
   END
	IF EXISTS (SELECT 1 FROM @estadoMovimientoAlmacenList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ema.estado_mov_alm IN (SELECT [value] FROM @estado_mov_alm_tvp)'
   END
	 IF @esEditable IS NOT NULL
	 BEGIN 
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.es_editable = @esEditable' 
	 END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@almacen_id INT,@trans_alma_id INT,@trans_alma CHAR(3),@trans_alma_tvp dbo.ListString READONLY,@not_trans_alma_tvp dbo.ListString READONLY,
		@tipo_mov_alma_id INT,
		@tipo_mov_alma CHAR(1),@numero_ruc VARCHAR(35),@almacen_destino_id INT,@mov_alma_ref_id INT,@fecha_desde DATETIME,@fecha_hasta DATETIME,@comprobante_id INT,
		@tiene_guia_rem CHAR(1),@g_r_en_modi CHAR(1),@centro_costo_id INT,@mov_reclasi_id INT,@estado_aproba_id INT,@estado_aproba CHAR(1),
		@estado_mov_alma_id INT,@estado_mov_alma CHAR(1),@estado_mov_alm_tvp dbo.ListString READONLY,@esEditable CHAR(1),@estado CHAR(1)',
		@periodoEmpresaSedeId,@almacenId,@transaccionAlmacenId,@transaccionAlmacen,@transaccionAlmacenList,@notTransaccionAlmacenList,
		@tipoMovimientoAlmacenId,
		@tipoMovimientoAlmacen,@numeroRuc,@almacenDestinoId,@movimientoAlmacenReferencialId,@fechaDesde,@fechaHasta,@comprobanteId,
		@tieneGuiaRemision,@guiaRemisionEnModificacion,@centroCostoId,@movimientoReclasificacionId,@estadoAprobacionId,@estadoAprobacion,
		@estadoMovimientoAlmacenId,@estadoMovimientoAlmacen,@estadoMovimientoAlmacenList,@esEditable,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_mov_almacen_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 6. Procedimiento: sp_guia_remision_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_guia_remision_buscarPor]
(
	@pageSize                          INT,
	@orderBy                           VARCHAR(100) = NULL,
	@periodoEmpresaSedeId              INT,
	@serie                             CHAR(4) = NULL,
	@containsNumero                    VARCHAR(8) = NULL,
	@containsRazonSocial               VARCHAR(150) = NULL,
	@razonSocialGR										 VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad          VARCHAR(35) = NULL,
	@fechaDesde                        DATETIME = NULL,
	@fechaHasta                        DATETIME = NULL,
	@EsInterno                         CHAR(1) = NULL,
	@esVenta                           CHAR(1) = NULL,
	@estadoAprobacion                  CHAR(1) = NULL,
	@estadoGuiaRemisionId              INT = NULL,
  @docRef                            VARCHAR(35) = NULL,
	@estado                            CHAR(1) = NULL
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
	SET @orderBy = NULLIF(@orderBy,'')
	SET @serie = NULLIF(@serie,'')
	SET @containsNumero = NULLIF(@containsNumero,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @esInterno = NULLIF(@esInterno,'')
	SET @esVenta = NULLIF(@esVenta,'')
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estadoGuiaRemisionId = NULLIF(@estadoGuiaRemisionId,0)
  SET @docRef = NULLIF(@docRef,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'gr.guia_remision_id,'
		+ 'gr.periodo_emp_sede_id,'
		+ 'gr.tipo_doc_com_id,'
		+ 'gr.serie,'
		+ 'gr.numero,'
		+ 'gr.fecha,'
		+ 'gr.fec_ini_tra,'
		+ 'gr.costo_flete,'
		+ 'gr.es_interno,'
		+ 'gr.distrito_id,'
		+ 'gr.punto_partida,'
		+ 'gr.cod_sunat_par,'
		+ 'gr.tipo_d_com_ref_id,'
		+ 'gr.serie_d_ref,'
		+ 'gr.numero_d_ref,'
		+ 'gr.distrito_llegada_id,'
		+ 'gr.punto_llegada,'
		+ 'gr.cod_sunat_lle,'
		+ 'gr.transp_unid_id,'
		+ 'gr.transp_condu_id,'
		+ 'gr.motivo_trasla_id,'
		+ 'gr.es_venta,'
		+ 'gr.impreso,'
		+ 'gr.peso_b_total,'
		+ 'gr.compro_elec_id,'
		+ 'gr.es_modifica,'
		+ 'gr.estado_g_r_ant_id,'
		+ 'gr.observa,'
		+ 'gr.estado,'
		+ 'ma.mov_almacen_id,'
		+ 'ma.mov_almacen,'
		+ 'ma.serie_doc,'
		+ 'ma.numero_doc,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'tdc.codigo_sunat,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'p2.num_doc_iden,'
		+ 'p2.numero_ruc,'
		+ 'u.usuario_id,'
		+ 'u.usuario_logon,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre,'
		+ 'egr.estado_guia_r_id,'
		+ 'egr.estado_guia_r,'
		+ 'egr.nombre,'
		+ 'es.estado_sunat_id,'
		+ 'es.estado_sunat,'
		+ 'es.nombre'
		+ CHAR(13) + 'FROM guia_remision gr'
		+ CHAR(13) + 'LEFT JOIN mov_almacen ma ON ma.mov_almacen_id = gr.mov_almacen_id'
		+ CHAR(13) + 'LEFT JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = ma.tipo_doc_com_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = gr.proveedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = gr.proveedor2_id'
		+ CHAR(13) + 'INNER JOIN usuario u ON u.usuario_id = gr.usuario_creacion_id'
		+ CHAR(13) + 'LEFT JOIN estado_aproba ea ON ea.estado_aproba_id = gr.estado_aproba_id'
		+ CHAR(13) + 'INNER JOIN estado_guia_r egr ON egr.estado_guia_r_id = gr.estado_guia_r_id'
		+ CHAR(13) + 'LEFT JOIN estado_sunat es ON es.estado_sunat_id = gr.estado_sunat_id'

	SET @whereStat = CHAR(13) + 'WHERE gr.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @serie IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.serie = @serie'
	END
	IF @containsNumero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.numero LIKE ''%' + @containsNumero + '%'''
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @razonSocialGR IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @razonSocialGR + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
  IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(gr.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(gr.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @esInterno IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.es_interno = @es_interno'
	END
	IF @esVenta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.es_venta = @es_venta'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estadoGuiaRemisionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.estado_guia_r_id = @estado_guia_r_id'
	END
  IF @docRef IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.numero_doc LIKE ''%' + @docRef + '%'''
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' gr.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@serie CHAR(4),@num_doc_iden VARCHAR(35),
		@fecha_desde DATETIME,@fecha_hasta DATETIME,@es_interno CHAR(1),@es_venta CHAR(1),@estado_aproba CHAR(1),@estado_guia_r_id INT,
    @docRef VARCHAR(35), @estado CHAR(1)',
		@periodoEmpresaSedeId,@serie,@numeroDocumentoIdentidad,
		@fechaDesde,@fechaHasta,@EsInterno,@esVenta,@estadoAprobacion,@estadoGuiaRemisionId,@docRef,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_guia_remision_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 7. Procedimiento: sp_mov_reclasi_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_mov_reclasi_buscarPor]
(
	@pageSize                          INT,
	@orderBy                           VARCHAR(100) = NULL,
	@periodoEmpresaSedeId              INT,
	@containsMovimientoReclasificacion VARCHAR(10) = NULL,
	@almacenId                         INT = NULL,
	@fechaDesde                        DATETIME = NULL,
	@fechaHasta                        DATETIME = NULL,
	@estado                            CHAR(1) = NULL,
	@estadomovRecla										 CHAR(1) = NULL
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
SET @containsMovimientoReclasificacion = NULLIF(@containsMovimientoReclasificacion,'')
SET @almacenId = NULLIF(@almacenId,0)
SET @estado = NULLIF(@estado,'')
SET @estadomovRecla = NULLIF(@estadomovRecla, '')

--// Construir expression SQL.
SET @selectStat = 'SELECT '
    + (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
    + CHAR(13) +
    + 'mr.mov_reclasi_id,'
    + 'mr.periodo_emp_sede_id,'
    + 'mr.mov_reclasi,'
    + 'mr.fecha,'
    + 'mr.mes_contable_id,'
    + 'mr.observacion,'
    + 'mr.estado,'
    + 'ma.mov_almacen_id,'
    + 'ma.mov_almacen,'
    + 'ma.serie_doc,'
    + 'ma.numero_doc,'
		+ 'u.usuario_id,'
    + 'u.nombre_completo,'
    + 'a.almacen_id,'
    + 'a.nombre,'
    + 'ta.trans_alma_id,'
    + 'ta.trans_alma,'
    + 'ta.nombre,'
    + 'tma.tipo_mov_alma_id,'
    + 'tma.tipo_mov_alma,'
    + 'tdc.tipo_doc_com_id,'
    + 'tdc.tipo_doc_com,'
    + 'tdc.abrev,'
    + 'p.proveedor_id,'
    + 'p.razon_social,'
    + 'emr.estado_mov_rec_id,'
    + 'emr.estado_mov_rec,'
    + 'emr.nombre'
    + CHAR(13) + 'FROM mov_reclasi mr'
    + CHAR(13) + 'INNER JOIN mov_almacen ma ON ma.mov_almacen_id = mr.mov_almacen_id'
    + CHAR(13) + 'INNER JOIN almacen a ON a.almacen_id = ma.almacen_id'
    + CHAR(13) + 'INNER JOIN trans_alma ta ON ta.trans_alma_id = ma.trans_alma_id'
    + CHAR(13) + 'INNER JOIN tipo_mov_alma tma ON tma.tipo_mov_alma_id = ma.tipo_mov_alma_id'
    + CHAR(13) + 'LEFT JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = ma.tipo_doc_com_id'
    + CHAR(13) + 'LEFT JOIN proveedor p ON p.proveedor_id = ma.proveedor_id'
    + CHAR(13) + 'INNER JOIN estado_mov_rec emr ON emr.estado_mov_rec_id = mr.estado_mov_rec_id'
    + CHAR(13) + 'LEFT JOIN usuario u on mr.usuario_autoriza_id = u.usuario_id'

SET @whereStat = CHAR(13) + 'WHERE mr.periodo_emp_sede_id = @periodo_emp_sede_id'

--// Parámetros de filtro.
IF @containsMovimientoReclasificacion IS NOT NULL
BEGIN
    SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' mr.mov_reclasi LIKE ''%' + @containsMovimientoReclasificacion + '%'''
END
IF @almacenId IS NOT NULL
BEGIN
    SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ma.almacen_id = @almacen_id'
END
IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(mr.fecha AS DATE) >= CAST(@fechaDesde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(mr.fecha AS DATE) <= CAST(@fechaHasta AS DATE)'
	END
IF @estado IS NOT NULL
BEGIN
    SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' mr.estado = @estado'
END
IF @estadomovRecla IS NOT NULL
BEGIN 
    SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' emr.estado_mov_rec = @estadomovRecla'
END

SET @selectStat = @selectStat + @whereStat

--// Agregar ORDER BY.
IF @orderBy IS NOT NULL
BEGIN
    SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
END

--// Ejecutar consulta dinámica.
EXEC sp_executesql @selectStat,
    N'@periodo_emp_sede_id INT,@almacen_id INT,@fechaDesde DATETIME,@fechaHasta DATETIME,
    @estado CHAR(1), @estadomovRecla CHAR(1)',
    @periodoEmpresaSedeId,@almacenId,@fechaDesde,@fechaHasta,
    @estado,@estadomovRecla

SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_mov_reclasi_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 8. Procedimiento: sp_cotiza_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_cotiza_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@periodoEmpresaSedeId      INT,
	@cotizacion                CHAR(7) = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@containsVendedor          VARCHAR(150) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@esPedido                  CHAR(1) = NULL,
	@estadoCotizacionId        INT = NULL,
	@estadoCotizacionList      dbo.ListString READONLY,
	@estado                    CHAR(1) = NULL
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
	SET @cotizacion = NULLIF(@cotizacion,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @esPedido = NULLIF(@esPedido,'')
	SET @estadoCotizacionId = NULLIF(@estadoCotizacionId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'c.cotiza_id,'
		+ 'c.periodo_emp_sede_id,'
		+ 'c.cotiza,'
		+ 'c.fecha,'
		+ 'c.monto_tc,'
		+ 'c.lista_precio_id,'
		+ 'c.tipo_c_pago_id,'
		+ 'c.observa,'
		+ 'c.impuesto_tasa_id,'
		+ 'c.monto_bruto,'
		+ 'c.porc_dcto,'
		+ 'c.monto_dcto,'
		+ 'c.monto_dcto_item,'
		+ 'c.monto_neto,'
		+ 'c.monto_igv,'
		+ 'c.monto_isc,'
		+ 'c.monto_anticipo,'
		+ 'c.monto_total,'
		+ 'c.almacen_id,'
		+ 'c.mes_contable_id,'
		+ 'c.es_pedido,'
		+ 'c.envia_prod,'
		+ 'c.estado,'
		+ 'p.proveedor_id,'
		+ 'p.proveedor,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'tnj.tipo_nat_juri_id,'
		+ 'tnj.tipo_nat_juri,'
		+ 'tdi.tipo_doc_iden_id,'
		+ 'tdi.tipo_doc_iden,'
		+ 'tdi.abrev,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'ec.estado_cotiza_id,'
		+ 'ec.estado_cotiza,'
		+ 'ec.nombre'
		+ CHAR(13) + 'FROM cotiza c'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN tipo_nat_juri tnj ON tnj.tipo_nat_juri_id = p.tipo_nat_juri_id'
		+ CHAR(13) + 'INNER JOIN tipo_doc_iden tdi ON tdi.tipo_doc_iden_id = p.tipo_doc_iden_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'INNER JOIN estado_cotiza ec ON ec.estado_cotiza_id = c.estado_cotiza_id'

	SET @whereStat = CHAR(13) + 'WHERE c.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @cotizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza = @cotiza'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @esPedido IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.es_pedido = @es_pedido'
	END
	IF EXISTS (SELECT 1 FROM @estadoCotizacionList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_cotiza IN (SELECT [value] FROM @estado_cotiza_tvp) '
   END
	IF @estadoCotizacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado_cotiza_id = @estado_cotiza_id'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@cotiza CHAR(7),@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@es_pedido CHAR(1),@estado_cotiza_tvp dbo.ListString READONLY,@estado_cotiza_id INT,@estado CHAR(1)',
		@periodoEmpresaSedeId,@cotizacion,@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@esPedido,@estadoCotizacionList,@estadoCotizacionId,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_cotiza_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 9. Procedimiento: sp_cotiza_autoriza_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_cotiza_autoriza_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@empresaId                 INT,
	@cotizacionId              INT = NULL,
	@cotizacion 			   VARCHAR(7) = NULL,
	@periodoEmpresaSedeId      INT = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@containsVendedor          VARCHAR(150) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@tipoAutorizacion          CHAR(2) = NULL,
	@estadoAprobacionId        INT = NULL,
	@estadoAprobacion          CHAR(1) = NULL,
	@estado                    CHAR(1) = NULL
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
	SET @cotizacionId = NULLIF(@cotizacionId,'')
	SET @cotizacion = NULLIF(@cotizacion, '')
	SET @periodoEmpresaSedeId = NULLIF(@periodoEmpresaSedeId,0)
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @tipoAutorizacion = NULLIF(@tipoAutorizacion,'')
	SET @estadoAprobacionId = NULLIF(@estadoAprobacionId,0)
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'ca.cotiza_autoriza_id,'
		+ 'ca.fecha_auto,'
		+ 'ca.estado,'
		+ 'ta.tipo_autoriza_id,'
		+ 'ta.tipo_autoriza,'
		+ 'ta.nombre,'
		+ 'c.cotiza_id,'
		+ 'c.cotiza,'
		+ 'c.fecha,'
		+ 'c.monto_total,'
		+ 'pes.periodo_emp_sede_id,'
		+ 'es.empresa_sede_id,'
		+ 'es.nombre,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'u.usuario_id,'
		+ 'u.usuario_logon,'
		+ 'u.nombre_completo,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre'
		+ CHAR(13) + 'FROM cotiza_autoriza ca'
		+ CHAR(13) + 'INNER JOIN tipo_autoriza ta ON ta.tipo_autoriza_id = ca.tipo_autoriza_id'
		+ CHAR(13) + 'INNER JOIN cotiza c ON c.cotiza_id = ca.cotiza_id'
		+ CHAR(13) + 'INNER JOIN periodo_emp_sede pes ON pes.periodo_emp_sede_id = c.periodo_emp_sede_id'
		+ CHAR(13) + 'INNER JOIN empresa_sede es ON es.empresa_sede_id = pes.empresa_sede_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'LEFT JOIN usuario u ON u.usuario_id = ca.usuario_id'
		+ CHAR(13) + 'INNER JOIN estado_aproba ea ON ea.estado_aproba_id = ca.estado_aproba_id'

	SET @whereStat = CHAR(13) + 'WHERE es.empresa_id = @empresa_id'

	--// Parámetros de filtro.
	IF @cotizacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza_id = @cotiza_id'
	END
	IF @cotizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza LIKE ''%' + @cotizacion + '%'''
	END
	IF @periodoEmpresaSedeId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pes.periodo_emp_sede_id = @periodo_emp_sede_id'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @tipoAutorizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ta.tipo_autoriza = @tipo_autoriza'
	END
	IF @estadoAprobacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba_id = @estado_aproba_id'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ca.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@empresa_id INT, @cotiza_id INT,@periodo_emp_sede_id INT,@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@tipo_autoriza CHAR(2),@estado_aproba_id INT,@estado_aproba CHAR(1),@estado CHAR(1)',
		@empresaId,@cotizacionId,@periodoEmpresaSedeId,@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@tipoAutorizacion,@estadoAprobacionId,@estadoAprobacion,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_cotiza_autoriza_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 10. Procedimiento: sp_comprobante_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_comprobante_buscarPor]
(
	@pageSize                    INT,
	@orderBy                     VARCHAR(100) = NULL,
	@periodoEmpresaSedeId        INT,
	@tipoDocumentoComercial      CHAR(2) = NULL,
	@tipoDocumentoComercialList  dbo.ListString READONLY,
	@containsRazonSocial         VARCHAR(150) = NULL,
	@numero                      CHAR(8) = NULL,
	@cotizacion                  CHAR(7) = NULL,
	@numeroDocumentoIdentidad    VARCHAR(35) = NULL,
	@containsVendedor            VARCHAR(150) = NULL,
	@fechaDesde                  DATETIME = NULL,
	@fechaHasta                  DATETIME = NULL,
	@cotizacionId                INT = NULL,
	@montoTotalMayorIgual        DECIMAL(19,4),
	@tipoComprobanteId           INT = NULL,
	@tipoComprobante             CHAR(1) = NULL,
	@tipoComprobanteList         dbo.ListString READONLY,
	@estadoSunat                 CHAR(1) = NULL,
	@tipoDetraccion              CHAR(3) = NULL,
	@notTipoDetraccion           CHAR(3) = NULL,
	@estadoComprobanteId         INT = NULL,
	@estadoComprobanteList       dbo.ListString READONLY,
	@bancarizado                 CHAR(1) = NULL,
	@estadoDespachoId            INT = NULL,
	@estado                      CHAR(1) = NULL
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
	SET @tipoDocumentoComercial = NULLIF(@tipoDocumentoComercial,'')
	SET @numero = NULLIF(@numero,'')
	SET @cotizacion = NULLIF(@cotizacion,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @estadoComprobanteId = NULLIF(@estadoComprobanteId,0)
	SET @cotizacionId = NULLIF(@cotizacionId,0)
	SET @montoTotalMayorIgual = NULLIF(@montoTotalMayorIgual,0)
	SET @tipoComprobanteId = NULLIF(@tipoComprobanteId,0)
	SET @tipoComprobante = NULLIF(@tipoComprobante,'')
	SET @estadoSunat = NULLIF(@estadoSunat,'')
	SET @tipoDetraccion = NULLIF(@tipoDetraccion,'')
	SET @notTipoDetraccion = NULLIF(@notTipoDetraccion,'')
	SET @bancarizado = NULLIF(@bancarizado,'')
	SET @estadoDespachoId = NULLIF(@estadoDespachoId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'c.comprobante_id,'
		+ 'c.periodo_emp_sede_id,'
		+ 'c.numero_opera,'
		+ 'c.serie,'
		+ 'c.numero,'
		+ 'c.fecha,'
		+ 'c.nombre_alte,'
		+ 'c.monto_tc,'
		+ 'c.lista_precio_id,'
		+ 'c.almacen_id,'
		+ 'c.observa,'
		+ 'c.porcen_tarj,'
		+ 'c.impuesto_tasa_id,'
		+ 'c.nro_guia_rem,'
		+ 'c.monto_bru_d,'
		+ 'c.monto_net_d,'
		+ 'c.monto_igv_d,'
		+ 'c.monto_tot_d,'
		+ 'c.monto_bruto,'
		+ 'c.monto_neto,'
		+ 'c.porc_dcto,'
		+ 'c.monto_dcto,'
		+ 'c.monto_dcto_item,'
		+ 'c.monto_neto,'
		+ 'c.monto_igv,'
		+ 'c.monto_isc,'
		+ 'c.monto_anticipo,'
		+ 'c.monto_total,'
		+ 'c.monto_ant_dcto,'
		+ 'c.canjeado,'
		+ 'c.tipo_comprob_id,'
		+ 'c.comp_canje_id,'
		+ 'c.estado_sunat_id,'
		+ 'c.porc_detrac,'
		+ 'c.monto_detrac,'
		+ 'c.num_opera_pago,'
		+ 'c.monto_pago,'
		+ 'c.fecha_pago,'
		+ 'c.monto_cargo,'
		+ 'c.es_contingencia,'
		+ 'c.emitido,'
		+ 'c.compro_elec_id,'
		+ 'c.codigo_hash,'
		+ 'c.bancariza,'
		+ 'c.estado_despacho_id,'
		+ 'c.personal_id,'
		+ 'c.fecha_desp,'
		+ 'c.observa_desp,'
		+ 'c.entrega_desp,'
		+ 'c.estado,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'ct.cotiza_id,'
		+ 'ct.cotiza,'
		+ 'ped.pedido_id,'
		+ 'ped.pedido,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'tcp.tipo_c_pago_id,'
		+ 'tcp.tipo_c_pago,'
		+ 'tcp.nombre,'
		+ 'mp.medio_pago_id,'
		+ 'mp.medio_pago,'
		+ 'mp.nombre,'
		+ 'tc.tipo_comprob_id,'
		+ 'tc.tipo_comprob,'
		+ 'tc.nombre,'
		+ 'td.tipo_detrac_id,'
		+ 'td.nombre,'
		+ 'td.tasa,'
		+ 'ec.estado_compro_id,'
		+ 'ec.estado_compro,'
		+ 'ec.nombre,'
		+ 'es.estado_sunat_id,'
		+ 'es.estado_sunat,'
		+ 'es.nombre'
		+ CHAR(13) + 'FROM comprobante c'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id'
		+ CHAR(13) + 'LEFT JOIN cotiza ct ON ct.cotiza_id = c.cotiza_id'
		+ CHAR(13) + 'LEFT JOIN pedido ped ON ped.pedido_id = c.pedido_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'INNER JOIN tipo_c_pago tcp ON tcp.tipo_c_pago_id = c.tipo_c_pago_id'
		+ CHAR(13) + 'INNER JOIN medio_pago mp ON mp.medio_pago_id = c.medio_pago_id'
		+ CHAR(13) + 'INNER JOIN tipo_comprob tc ON tc.tipo_comprob_id = c.tipo_comprob_id'
		+ CHAR(13) + 'INNER JOIN tipo_detrac td ON td.tipo_detrac_id = c.tipo_detrac_id'
		+ CHAR(13) + 'INNER JOIN estado_compro ec ON ec.estado_compro_id = c.estado_compro_id'
		+ CHAR(13) + 'LEFT JOIN estado_sunat es ON es.estado_sunat_id = c.estado_sunat_id'

	SET @whereStat = CHAR(13) + 'WHERE c.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @tipoDocumentoComercial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com = @tipo_doc_com'
	END
	IF EXISTS (SELECT 1 FROM @tipoDocumentoComercialList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com IN (SELECT [value] FROM @tipo_doc_com_tvp) '
   END
	IF @numero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.numero = @numero'
	END
	IF @cotizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ct.cotiza = @cotizacion'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @estadoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro_id = @estado_compro_id'
	END
	IF EXISTS (SELECT 1 FROM @estadoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro IN (SELECT [value] FROM @estado_compro_tvp) '
   END
	IF @cotizacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza_id = @cotiza_id'
	END
	IF @montoTotalMayorIgual IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ((CASE WHEN m.es_local=''N'' THEN c.monto_tc ELSE 1 END) * c.monto_total) >= @monto_total_mayor_igual'
	END
	IF @estadoSunat IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' es.estado_sunat = @estado_sunat'
	END
	IF @tipoDetraccion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' td.tipo_detrac = @tipo_detrac'
	END
	IF @notTipoDetraccion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' td.tipo_detrac <> @not_tipo_detrac'
	END
	IF @tipoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob_id = @tipo_comprob_id'
	END
	IF @tipoComprobante IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob = @tipo_comprob'
	END
	IF EXISTS (SELECT 1 FROM @tipoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob IN (SELECT [value] FROM @tipo_comprob_tvp) '
   END
	IF @bancarizado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.bancariza = @bancariza'
	END
	IF @estadoDespachoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado_despacho_id = @estado_despacho_id'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@tipo_doc_com CHAR(2),@tipo_doc_com_tvp dbo.ListString READONLY,@numero CHAR(8),@cotizacion CHAR(7),
		@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@cotiza_id INT,@monto_total_mayor_igual DECIMAL(19,4),@estado_sunat CHAR(1),@tipo_detrac CHAR(3),
		@not_tipo_detrac CHAR(3),@tipo_comprob_id INT,@tipo_comprob CHAR(1),@tipo_comprob_tvp dbo.ListString READONLY,@estado_compro_id INT,
		@estado_compro_tvp dbo.ListString READONLY,@bancariza CHAR(1),@estado_despacho_id INT,@estado CHAR(1)',
		@periodoEmpresaSedeId,@tipoDocumentoComercial,@tipoDocumentoComercialList,@numero,@cotizacion,
		@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@cotizacionId,@montoTotalMayorIgual,@estadoSunat,@tipoDetraccion,
		@notTipoDetraccion,@tipoComprobanteId,@tipoComprobante,@tipoComprobanteList,@estadoComprobanteId,
		@estadoComprobanteList,@bancarizado,@estadoDespachoId,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_comprobante_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 11. Procedimiento: sp_comprobante_buscarPorEmpresaSede
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_comprobante_buscarPorEmpresaSede]
(
	@pageSize                    INT,
	@orderBy                     VARCHAR(100) = NULL,
	@empresaSedeId               INT,
	@tipoDocumentoComercial      CHAR(2) = NULL,
	@tipoDocumentoComercialList  dbo.ListString READONLY,
	@containsRazonSocial         VARCHAR(150) = NULL,
	@numero                      CHAR(8) = NULL,
	@cotizacion                  CHAR(7) = NULL,
	@numeroDocumentoIdentidad    VARCHAR(35) = NULL,
	@containsVendedor            VARCHAR(150) = NULL,
	@fechaDesde                  DATETIME = NULL,
	@fechaHasta                  DATETIME = NULL,
	@cotizacionId                INT = NULL,
	@montoTotalMayorIgual        DECIMAL(19,4),
	@tipoComprobanteId           INT = NULL,
	@tipoComprobante             CHAR(1) = NULL,
	@tipoComprobanteList         dbo.ListString READONLY,
	@estadoSunat                 CHAR(1) = NULL,
	@tipoDetraccion              CHAR(3) = NULL,
	@notTipoDetraccion           CHAR(3) = NULL,
	@estadoComprobanteId         INT = NULL,
	@estadoComprobanteList       dbo.ListString READONLY,
	@bancarizado                 CHAR(1) = NULL,
	@estado                      CHAR(1) = NULL
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
	SET @tipoDocumentoComercial = NULLIF(@tipoDocumentoComercial,'')
	SET @numero = NULLIF(@numero,'')
	SET @cotizacion = NULLIF(@cotizacion,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @estadoComprobanteId = NULLIF(@estadoComprobanteId,0)
	SET @cotizacionId = NULLIF(@cotizacionId,0)
	SET @montoTotalMayorIgual = NULLIF(@montoTotalMayorIgual,0)
	SET @tipoComprobanteId = NULLIF(@tipoComprobanteId,0)
	SET @tipoComprobante = NULLIF(@tipoComprobante,'')
	SET @estadoSunat = NULLIF(@estadoSunat,'')
	SET @tipoDetraccion = NULLIF(@tipoDetraccion,'')
	SET @notTipoDetraccion = NULLIF(@notTipoDetraccion,'')
	SET @bancarizado = NULLIF(@bancarizado,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'c.comprobante_id,'
		+ 'c.periodo_emp_sede_id,'
		+ 'c.numero_opera,'
		+ 'c.serie,'
		+ 'c.numero,'
		+ 'c.fecha,'
		+ 'c.nombre_alte,'
		+ 'c.monto_tc,'
		+ 'c.lista_precio_id,'
		+ 'c.almacen_id,'
		+ 'c.observa,'
		+ 'c.porcen_tarj,'
		+ 'c.impuesto_tasa_id,'
		+ 'c.nro_guia_rem,'
		+ 'c.monto_bru_d,'
		+ 'c.monto_net_d,'
		+ 'c.monto_igv_d,'
		+ 'c.monto_tot_d,'
		+ 'c.monto_bruto,'
		+ 'c.porc_dcto,'
		+ 'c.monto_dcto,'
		+ 'c.monto_dcto_item,'
		+ 'c.monto_neto,'
		+ 'c.monto_igv,'
		+ 'c.monto_isc,'
		+ 'c.monto_anticipo,'
		+ 'c.monto_total,'
		+ 'c.monto_ant_dcto,'
		+ 'c.canjeado,'
		+ 'c.tipo_comprob_id,'
		+ 'c.comp_canje_id,'
		+ 'c.estado_sunat_id,'
		+ 'c.porc_detrac,'
		+ 'c.monto_detrac,'
		+ 'c.num_opera_pago,'
		+ 'c.monto_pago,'
		+ 'c.fecha_pago,'
		+ 'c.monto_cargo,'
		+ 'c.es_contingencia,'
		+ 'c.emitido,'
		+ 'c.compro_elec_id,'
		+ 'c.codigo_hash,'
		+ 'c.bancariza,'
		+ 'c.estado_despacho_id,'
		+ 'c.personal_id,'
		+ 'c.fecha_desp,'
		+ 'c.observa_desp,'
		+ 'c.entrega_desp,'
		+ 'c.estado,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.abrev,'
		+ 'ct.cotiza_id,'
		+ 'ct.cotiza,'
		+ 'ped.pedido_id,'
		+ 'ped.pedido,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'tcp.tipo_c_pago_id,'
		+ 'tcp.tipo_c_pago,'
		+ 'tcp.nombre,'
		+ 'mp.medio_pago_id,'
		+ 'mp.medio_pago,'
		+ 'mp.nombre,'
		+ 'tc.tipo_comprob_id,'
		+ 'tc.tipo_comprob,'
		+ 'tc.nombre,'
		+ 'td.tipo_detrac_id,'
		+ 'td.nombre,'
		+ 'td.tasa,'
		+ 'ec.estado_compro_id,'
		+ 'ec.estado_compro,'
		+ 'ec.nombre,'
		+ 'es.estado_sunat_id,'
		+ 'es.estado_sunat,'
		+ 'es.nombre'
		+ CHAR(13) + 'FROM comprobante c'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id'
		+ CHAR(13) + 'LEFT JOIN cotiza ct ON ct.cotiza_id = c.cotiza_id'
		+ CHAR(13) + 'LEFT JOIN pedido ped ON ped.pedido_id = c.pedido_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'INNER JOIN tipo_c_pago tcp ON tcp.tipo_c_pago_id = c.tipo_c_pago_id'
		+ CHAR(13) + 'INNER JOIN medio_pago mp ON mp.medio_pago_id = c.medio_pago_id'
		+ CHAR(13) + 'INNER JOIN tipo_comprob tc ON tc.tipo_comprob_id = c.tipo_comprob_id'
		+ CHAR(13) + 'INNER JOIN tipo_detrac td ON td.tipo_detrac_id = c.tipo_detrac_id'
		+ CHAR(13) + 'INNER JOIN estado_compro ec ON ec.estado_compro_id = c.estado_compro_id'
		+ CHAR(13) + 'LEFT JOIN estado_sunat es ON es.estado_sunat_id = c.estado_sunat_id'
		+ CHAR(13) + 'INNER JOIN periodo_emp_sede pes ON pes.periodo_emp_sede_id = c.periodo_emp_sede_id'

	SET @whereStat = CHAR(13) + 'WHERE pes.empresa_sede_id = @empresa_sede_id'

	--// Parámetros de filtro.
	IF @tipoDocumentoComercial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com = @tipo_doc_com'
	END
	IF EXISTS (SELECT 1 FROM @tipoDocumentoComercialList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com IN (SELECT [value] FROM @tipo_doc_com_tvp) '
   END
	IF @numero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.numero = @numero'
	END
	IF @cotizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ct.cotiza = @cotizacion'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @estadoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro_id = @estado_compro_id'
	END
	IF EXISTS (SELECT 1 FROM @estadoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro IN (SELECT [value] FROM @estado_compro_tvp) '
   END
	IF @cotizacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza_id = @cotiza_id'
	END
	IF @montoTotalMayorIgual IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ((CASE WHEN m.es_local=''N'' THEN c.monto_tc ELSE 1 END) * c.monto_total) >= @monto_total_mayor_igual'
	END
	IF @estadoSunat IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' es.estado_sunat = @estado_sunat'
	END
	IF @tipoDetraccion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' td.tipo_detrac = @tipo_detrac'
	END
	IF @notTipoDetraccion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' td.tipo_detrac <> @not_tipo_detrac'
	END
	IF @tipoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob_id = @tipo_comprob_id'
	END
	IF @tipoComprobante IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob = @tipo_comprob'
	END
	IF EXISTS (SELECT 1 FROM @tipoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob IN (SELECT [value] FROM @tipo_comprob_tvp) '
   END
	IF @bancarizado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.bancariza = @bancariza'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@empresa_sede_id INT,@tipo_doc_com CHAR(2),@tipo_doc_com_tvp dbo.ListString READONLY,@numero CHAR(8),@cotizacion CHAR(7),
		@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@cotiza_id INT,@monto_total_mayor_igual DECIMAL(19,4),@estado_sunat CHAR(1),@tipo_detrac CHAR(3),
		@not_tipo_detrac CHAR(3),@tipo_comprob_id INT,@tipo_comprob CHAR(1),@tipo_comprob_tvp dbo.ListString READONLY,@estado_compro_id INT,
		@estado_compro_tvp dbo.ListString READONLY,@bancariza CHAR(1),@estado CHAR(1)',
		@empresaSedeId,@tipoDocumentoComercial,@tipoDocumentoComercialList,@numero,@cotizacion,
		@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@cotizacionId,@montoTotalMayorIgual,@estadoSunat,@tipoDetraccion,
		@notTipoDetraccion,@tipoComprobanteId,@tipoComprobante,@tipoComprobanteList,@estadoComprobanteId,
		@estadoComprobanteList,@bancarizado,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_comprobante_buscarPorEmpresaSede actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 12. Procedimiento: sp_comprob_autoriza_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_comprob_autoriza_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@empresaId                 INT,
	@comprobanteId             INT = NULL,
	@periodoEmpresaSedeId      INT = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@containsVendedor          VARCHAR(150) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@estadoAprobacionId        INT = NULL,
	@estadoAprobacion          CHAR(1) = NULL,
	@estado                    CHAR(1) = NULL
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
	SET @comprobanteId = NULLIF(@comprobanteId,'')
	SET @periodoEmpresaSedeId = NULLIF(@periodoEmpresaSedeId,0)
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @estadoAprobacionId = NULLIF(@estadoAprobacionId,0)
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'ca.comprob_autoriza_id,'
		+ 'ca.fecha_auto,'
		+ 'ca.estado,'
		+ 'ta.tipo_autoriza_id,'
		+ 'ta.tipo_autoriza,'
		+ 'ta.nombre,'
		+ 'c.comprobante_id,'
		+ 'c.serie,'
		+ 'c.fecha,'
		+ 'c.monto_total,'
		+ 'pes.periodo_emp_sede_id,'
		+ 'es.empresa_sede_id,'
		+ 'es.nombre,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'u.usuario_id,'
		+ 'u.usuario_logon,'
		+ 'u.nombre_completo,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre'
		+ CHAR(13) + 'FROM comprob_autoriza ca'
		+ CHAR(13) + 'INNER JOIN tipo_autoriza ta ON ta.tipo_autoriza_id = ca.tipo_autoriza_id'
		+ CHAR(13) + 'INNER JOIN comprobante c ON c.comprobante_id = ca.comprobante_id'
		+ CHAR(13) + 'INNER JOIN periodo_emp_sede pes ON pes.periodo_emp_sede_id = c.periodo_emp_sede_id'
		+ CHAR(13) + 'INNER JOIN empresa_sede es ON es.empresa_sede_id = pes.empresa_sede_id'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'LEFT JOIN usuario u ON u.usuario_id = ca.usuario_id'
		+ CHAR(13) + 'INNER JOIN estado_aproba ea ON ea.estado_aproba_id = ca.estado_aproba_id'

	SET @whereStat = CHAR(13) + 'WHERE es.empresa_id = @empresa_id'

	--// Parámetros de filtro.
	IF @comprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.comprobante_id = @comprobante_id'
	END
	IF @periodoEmpresaSedeId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pes.periodo_emp_sede_id = @periodo_emp_sede_id'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @estadoAprobacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba_id = @estado_aproba_id'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ca.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@empresa_id INT, @comprobante_id INT,@periodo_emp_sede_id INT,@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@estado_aproba_id INT,@estado_aproba CHAR(1),@estado CHAR(1)',
		@empresaId,@comprobanteId,@periodoEmpresaSedeId,@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@estadoAprobacionId,@estadoAprobacion,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_comprob_autoriza_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 13. Procedimiento: sp_comprobante_buscarDespacho
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_comprobante_buscarDespacho]
(
	@pageSize                    INT,
	@orderBy                     VARCHAR(100) = NULL,
	@periodoEmpresaSedeId        INT,
	@tipoDocumentoComercial      CHAR(2) = NULL,
	@tipoDocumentoComercialList  dbo.ListString READONLY,
	@containsRazonSocial         VARCHAR(150) = NULL,
	@numero                      CHAR(8) = NULL,
	@cotizacion                  CHAR(7) = NULL,
	@numeroDocumentoIdentidad    VARCHAR(35) = NULL,
	@containsVendedor            VARCHAR(150) = NULL,
	@fechaDesde                  DATETIME = NULL,
	@fechaHasta                  DATETIME = NULL,
	@tipoComprobanteId           INT = NULL,
	@tipoComprobante             CHAR(1) = NULL,
	@tipoComprobanteList         dbo.ListString READONLY,
	@estadoComprobanteId         INT = NULL,
	@estadoComprobanteList       dbo.ListString READONLY,
	@estadoDespachoId            INT = NULL,
	@estado                      CHAR(1) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@selectStat NVARCHAR(4000),
		@whereStat NVARCHAR(4000)

	SET @whereStat = ''

   CREATE TABLE #result_tmp(
      id         INT,
		rowNumber  INT
   )

	--// Verificar valores nulos.
	SET @orderBy = NULLIF(@orderBy,'')
	SET @tipoDocumentoComercial = NULLIF(@tipoDocumentoComercial,'')
	SET @numero = NULLIF(@numero,'')
	SET @cotizacion = NULLIF(@cotizacion,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @estadoComprobanteId = NULLIF(@estadoComprobanteId,0)
	SET @tipoComprobanteId = NULLIF(@tipoComprobanteId,0)
	SET @tipoComprobante = NULLIF(@tipoComprobante,'')
	SET @estadoDespachoId = NULLIF(@estadoDespachoId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'INSERT INTO #result_tmp(id,rowNumber)' 
		+ CHAR(13) +
		+ 'SELECT ' + (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) + 'c.comprobante_id,'
		+ CHAR(13) + 'ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ')' +
		+ CHAR(13) + 'FROM comprobante c'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id'
		+ CHAR(13) + 'LEFT JOIN cotiza ct ON ct.cotiza_id = c.cotiza_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN tipo_comprob tc ON tc.tipo_comprob_id = c.tipo_comprob_id'
		+ CHAR(13) + 'INNER JOIN estado_compro ec ON ec.estado_compro_id = c.estado_compro_id'

	SET @whereStat = CHAR(13) + 'WHERE c.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @tipoDocumentoComercial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com = @tipo_doc_com'
	END
	IF EXISTS (SELECT 1 FROM @tipoDocumentoComercialList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tdc.tipo_doc_com IN (SELECT [value] FROM @tipo_doc_com_tvp) '
   END
	IF @numero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.numero = @numero'
	END
	IF @cotizacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ct.cotiza = @cotizacion'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
   IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(c.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @estadoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro_id = @estado_compro_id'
	END
	IF EXISTS (SELECT 1 FROM @estadoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro IN (SELECT [value] FROM @estado_compro_tvp) '
   END
	IF @tipoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob_id = @tipo_comprob_id'
	END
	IF @tipoComprobante IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob = @tipo_comprob'
	END
	IF EXISTS (SELECT 1 FROM @tipoComprobanteList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tc.tipo_comprob IN (SELECT [value] FROM @tipo_comprob_tvp) '
   END
	IF @estadoDespachoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado_despacho_id = @estado_despacho_id'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@tipo_doc_com CHAR(2),@tipo_doc_com_tvp dbo.ListString READONLY,@numero CHAR(8),@cotizacion CHAR(7),
		@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@tipo_comprob_id INT,@tipo_comprob CHAR(1),@tipo_comprob_tvp dbo.ListString READONLY,@estado_compro_id INT,
		@estado_compro_tvp dbo.ListString READONLY,@estado_despacho_id INT,@estado CHAR(1)',
		@periodoEmpresaSedeId,@tipoDocumentoComercial,@tipoDocumentoComercialList,@numero,@cotizacion,
		@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@tipoComprobanteId,@tipoComprobante,@tipoComprobanteList,@estadoComprobanteId,
		@estadoComprobanteList,@estadoDespachoId,@estado

	SELECT
		c.comprobante_id,
		c.periodo_emp_sede_id,
		c.numero_opera,
		c.serie,
		c.numero,
		c.fecha,
		c.nombre_alte,
		c.monto_tc,
		c.lista_precio_id,
		c.almacen_id,
		c.observa,
		c.porcen_tarj,
		c.impuesto_tasa_id,
		c.nro_guia_rem,
		c.monto_bru_d,
		c.monto_net_d,
		c.monto_igv_d,
		c.monto_tot_d,
		c.monto_bruto,
		c.monto_neto,
		c.monto_igv,
		c.monto_isc,
		c.monto_anticipo,
		c.monto_total,
		c.monto_ant_dcto,
		c.canjeado,
		c.tipo_comprob_id,
		c.comp_canje_id,
		c.estado_sunat_id,
		c.porc_detrac,
		c.monto_detrac,
		c.num_opera_pago,
		c.monto_pago,
		c.fecha_pago,
		c.monto_cargo,
		c.es_contingencia,
		c.emitido,
		c.compro_elec_id,
		c.codigo_hash,
		c.bancariza,
		c.fecha_desp,
		c.observa_desp,
		c.entrega_desp,
		c.estado,
		cp.color_lista,
		tdc.tipo_doc_com_id,
		tdc.abrev,
		ct.cotiza_id,
		ct.cotiza,
		p.proveedor_id,
		p.razon_social,
		p.num_doc_iden,
		p.numero_ruc,
		v.vendedor_id,
		p2.proveedor_id,
		p2.razon_social,
		tc.tipo_comprob_id,
		tc.tipo_comprob,
		tc.nombre,
		ec.estado_compro_id,
		ec.estado_compro,
		ec.nombre,
		ed.estado_despacho_id,
		ed.estado_despacho,
		ed.nombre,
		per.personal_id,
		p3.proveedor_id,
		p3.razon_social
	FROM comprobante c
	INNER JOIN #result_tmp tmp ON c.comprobante_id = tmp.id
	INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id
	LEFT JOIN cotiza ct ON ct.cotiza_id = c.cotiza_id
	INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id
	INNER JOIN vendedor v ON v.vendedor_id = c.vendedor_id
	INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id
	INNER JOIN tipo_comprob tc ON tc.tipo_comprob_id = c.tipo_comprob_id
	INNER JOIN estado_compro ec ON ec.estado_compro_id = c.estado_compro_id
	LEFT JOIN estado_despacho ed ON ed.estado_despacho_id = c.estado_despacho_id
	LEFT JOIN personal per ON per.personal_id = c.personal_id
	LEFT JOIN proveedor p3 ON p3.proveedor_id = per.proveedor_id
	LEFT JOIN 
	(
		SELECT 
			comprobante_id,
			STRING_AGG(nombre,',') WITHIN GROUP(ORDER BY comprobante_id ASC) AS color_lista
		FROM
		(
			SELECT
				cp.comprobante_id,
				c.nombre
			FROM comprob_prod cp
			INNER JOIN #result_tmp tmp ON cp.comprobante_id = tmp.id
			INNER JOIN producto p ON p.producto_id = cp.producto_id
			INNER JOIN color c ON c.color_id = p.color_id
			WHERE cp.estado = @estado 
			GROUP BY cp.comprobante_id,c.nombre
		) cp
		GROUP BY cp.comprobante_id
	) cp ON cp.comprobante_id = c.comprobante_id
	ORDER BY tmp.rowNumber

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_comprobante_buscarDespacho actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 14. Procedimiento: sp_orden_pro_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_orden_pro_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@periodoEmpresaSedeId      INT = NULL,
	@ordenProduccion           VARCHAR(7) = NULL,
	@cotizacionId              INT = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@estadoOrdenProduccionList dbo.ListString READONLY,
	@incluyeInsumo             CHAR(1) = NULL,
	@estadoDespachoId          INT = NULL,
	@cotiza										 VARCHAR(8) = NULL,
	@containsVendedor					 VARCHAR(150) = NULL,
	@estado                    CHAR(1) = NULL
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
	SET @periodoEmpresaSedeId = NULLIF(@periodoEmpresaSedeId,0)
	SET @ordenProduccion = NULLIF(@ordenProduccion,'')
	SET @cotizacionId = NULLIF(@cotizacionId,0)
	SET @cotiza = NULLIF(@cotiza, '')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @incluyeInsumo = NULLIF(@incluyeInsumo,'')
	SET @estadoDespachoId = NULLIF(@estadoDespachoId,0)
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'op.orden_pro_id,'
		+ 'op.periodo_emp_sede_id,'
		+ 'op.orden_pro,'
		+ 'op.almacen_id,'
		+ 'op.fecha,'
		+ 'op.observa,'
		+ 'op.ruta_prod_id,'
		+ 'op.fecha_ent,'
		+ 'op.incluye_insu,'
		+ 'op.fecha_desp,'
		+ 'op.modificaciones,'
		+ 'op.observa_desp,'
		+ 'op.estado_plano_mel_id,'
		+ 'op.estado_plano_mdf_id,'
		+ 'op.comentarios,'
		+ 'op.estado,'
		+ 'd.colores,'
		+ 'c.cotiza_id,'
		+ 'c.cotiza,'
		+ 'c.vendedor_id,'
		+ 'p.proveedor_id,'
		+ 'p.proveedor,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'eop.estado_orden_p_id,'
		+ 'eop.estado_orden_p,'
		+ 'eop.nombre,'
		+ 'ed.estado_despacho_id,'
		+ 'ed.estado_despacho,'
		+ 'ed.nombre,'
		+ 'per.personal_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'v.vendedor_id,'
		+ 'v.proveedor_id,'
		+ 'v.abrev,'
		+ 'pvd.proveedor_id,'
		+ 'pvd.razon_social,'
		+ 'mel.estado_orden_p_id,'
		+ 'mel.estado_orden_p,'
		+ 'mel.nombre,'
		+ 'mdf.estado_orden_p_id,'
		+ 'mdf.estado_orden_p,'
		+ 'mdf.nombre'
		+ CHAR(13) + 'FROM orden_pro op'
		+ CHAR(13) + 'LEFT JOIN cotiza c ON c.cotiza_id = op.cotiza_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = op.proveedor_id'
		+ CHAR(13) + 'INNER JOIN estado_orden_p eop ON eop.estado_orden_p_id = op.estado_orden_p_id'
		+ CHAR(13) + 'LEFT JOIN estado_despacho ed ON ed.estado_despacho_id = op.estado_despacho_id'
		+ CHAR(13) + 'LEFT JOIN personal per ON per.personal_id = op.personal_id'
		+ CHAR(13) + 'LEFT JOIN proveedor p2 ON p2.proveedor_id = per.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN vendedor v ON v.vendedor_id = c.vendedor_id'
		+ CHAR(13) + 'LEFT JOIN proveedor pvd ON pvd.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN estado_orden_p mel ON mel.estado_orden_p_id = op.estado_plano_mel_id'
		+ CHAR(13) + 'LEFT JOIN estado_orden_p mdf ON mdf.estado_orden_p_id = op.estado_plano_mdf_id'
		+ CHAR(13) + 'LEFT JOIN (SELECT orden_pro_id, colores FROM'
		+ CHAR(13) + '(SELECT a.orden_pro_id, STRING_AGG(a.colores, '' - '') as colores FROM'
		+ CHAR(13) + '(SELECT c.orden_pro_id, CONCAT(c.colores, '', '', FORMAT(c.cantidad, ''N2'')) as colores FROM'
		+ CHAR(13) + '(SELECT opp.orden_pro_id, fp.nombre AS colores, SUM(opp.cantidad) AS cantidad FROM orden_p_prod opp'
		+ CHAR(13) + 'INNER JOIN producto p on p.producto_id = opp.producto_id'
		+ CHAR(13) + 'INNER JOIN familia_producto fp ON fp.familia_producto_id = p.familia_producto_id'
		+ CHAR(13) + 'INNER JOIN clase_producto cp ON cp.clase_producto_id = fp.clase_producto_id' 
		+ CHAR(13) + 'WHERE cp.grupo_producto_id IN (5, 6) GROUP BY opp.orden_pro_id, fp.nombre) c) a GROUP BY a.orden_pro_id)'
		+ CHAR(13) + 'c) d ON d.orden_pro_id = op.orden_pro_id'
		
		
	--// Parámetros de filtro.
	IF @periodoEmpresaSedeId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' op.periodo_emp_sede_id = @periodo_emp_sede_id'
	END
	IF @ordenProduccion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' op.orden_pro LIKE ''%' + @ordenProduccion + '%'''
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' pvd.razon_social LIKE ''%' +@containsVendedor + '%'''
	END
	IF @cotizacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' op.cotiza_id = @cotiza_id'
	END
	IF @cotiza IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.cotiza LIKE ''%' + @cotiza + '%'''
	END
  IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(op.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(op.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF EXISTS (SELECT 1 FROM @estadoOrdenProduccionList)
   BEGIN
      SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' eop.estado_orden_p IN (SELECT [value] FROM @estado_orden_p_tvp)'
   END
	IF @incluyeInsumo IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' op.incluye_insu = @incluye_insu'
	END
	IF @estadoDespachoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ed.estado_despacho_id = @estado_despacho_id'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' op.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@orden_po CHAR(7),@cotiza_id INT,@fecha_desde DATETIME,@fecha_hasta DATETIME,@num_doc_iden VARCHAR(35),
		@estado_orden_p_tvp dbo.ListString READONLY,@incluye_insu CHAR(1),@estado_despacho_id INT,@cotiza CHAR(8),@estado CHAR(1)',
		@periodoEmpresaSedeId,@ordenProduccion,@cotizacionId,@fechaDesde,@fechaHasta,@numeroDocumentoIdentidad,
		@estadoOrdenProduccionList,@incluyeInsumo,@estadoDespachoId,@cotiza,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_orden_pro_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 15. Procedimiento: sp_nota_credito_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_nota_credito_buscarPor]
(
	@pageSize                  INT,
	@orderBy                   VARCHAR(100) = NULL,
	@periodoEmpresaSedeId      INT,
	@numero                    CHAR(8) = NULL,
	@containsRazonSocial       VARCHAR(150) = NULL,
	@numeroDocumentoIdentidad  VARCHAR(35) = NULL,
	@comprobanteId             INT = NULL,
	@containsVendedor          VARCHAR(150) = NULL,
	@fechaDesde                DATETIME = NULL,
	@fechaHasta                DATETIME = NULL,
	@tipoNotaCreditoId         INT = NULL,
	@tipoNotaCreditoList       dbo.ListString READONLY,
	@estadoComprobanteId       INT = NULL,
	@tipoDocComprobanteId			 INT = NULL,
	@comprobante							 VARCHAR(8) = NULL,
	@estado                    CHAR(1) = NULL
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
	SET @numero = NULLIF(@numero,'')
	SET @containsRazonSocial = NULLIF(@containsRazonSocial,'')
	SET @numeroDocumentoIdentidad = NULLIF(@numeroDocumentoIdentidad,'')
	SET @comprobanteId = NULLIF(@comprobanteId,0)
	SET @containsVendedor = NULLIF(@containsVendedor,'')
	SET @tipoNotaCreditoId = NULLIF(@tipoNotaCreditoId,0)
	SET @estadoComprobanteId = NULLIF(@estadoComprobanteId,0)
	SET @tipoDocComprobanteId = NULLIF(@tipoDocComprobanteId, 0)
	SET @comprobante = NULLIF(@comprobante,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'nc.nota_credito_id,'
		+ 'nc.periodo_emp_sede_id,'
		+ 'nc.numero_opera,'
		+ 'nc.serie,'
		+ 'nc.numero,'
		+ 'nc.fecha,'
		+ 'nc.observa,'
		+ 'nc.impuesto_tasa_id,'
		+ 'nc.monto_bruto,'
		+ 'nc.monto_neto,'
		+ 'nc.monto_igv,'
		+ 'nc.monto_isc,'
		+ 'nc.monto_total,'
		+ 'nc.es_contingencia,'
		+ 'nc.emitido,'
		+ 'nc.compro_elec_id,'
		+ 'nc.codigo_hash,'
		+ 'nc.estado_compro_id,'
		+ 'nc.estado_sunat_id,'
		+ 'nc.caja_id,'
		+ 'nc.estado,'
		+ 'tnc.tipo_nota_c_id,'
		+ 'tnc.tipo_nota_c,'
		+ 'tnc.nombre,'
		+ 'tnc.permite_edit_item,'
		+ 'tnc.permite_edit_cant,'
		+ 'c.comprobante_id,'
		+ 'c.serie,'
		+ 'c.numero,'
		+ 'c.fecha,'
		+ 'c.tipo_doc_com_id,'
		+ 'c.lista_precio_id,'
		+ 'tdc.tipo_doc_com_id,'
		+ 'tdc.tipo_doc_com,'
		+ 'tdc.abrev,'
		+ 'p.proveedor_id,'
		+ 'p.razon_social,'
		+ 'p.num_doc_iden,'
		+ 'p.numero_ruc,'
		+ 'm.moneda_id,'
		+ 'm.moneda,'
		+ 'm.simbolo,'
		+ 'v.vendedor_id,'
		+ 'p2.proveedor_id,'
		+ 'p2.razon_social,'
		+ 'ec.estado_compro_id,'
		+ 'ec.estado_compro,'
		+ 'ec.nombre,'
		+ 'es.estado_sunat_id,'
		+ 'es.estado_sunat,'
		+ 'es.nombre'
		+ CHAR(13) + 'FROM nota_credito nc'
		+ CHAR(13) + 'INNER JOIN tipo_nota_c tnc ON tnc.tipo_nota_c_id = nc.tipo_nota_c_id'
		+ CHAR(13) + 'INNER JOIN comprobante c ON c.comprobante_id = nc.comprobante_id'
		+ CHAR(13) + 'INNER JOIN tipo_doc_com tdc ON tdc.tipo_doc_com_id = c.tipo_doc_com_id'
		+ CHAR(13) + 'INNER JOIN proveedor p ON p.proveedor_id = c.proveedor_id'
		+ CHAR(13) + 'INNER JOIN moneda m ON m.moneda_id = c.moneda_id'
		+ CHAR(13) + 'INNER JOIN vendedor v ON v.vendedor_id = nc.vendedor_id'
		+ CHAR(13) + 'INNER JOIN proveedor p2 ON p2.proveedor_id = v.proveedor_id'
		+ CHAR(13) + 'INNER JOIN estado_compro ec ON ec.estado_compro_id = nc.estado_compro_id'
		+ CHAR(13) + 'LEFT JOIN estado_sunat es ON es.estado_sunat_id = nc.estado_sunat_id'

	SET @whereStat = CHAR(13) + 'WHERE nc.periodo_emp_sede_id = @periodo_emp_sede_id'

	--// Parámetros de filtro.
	IF @numero IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' nc.numero = @numero'
	END
	IF @containsRazonSocial IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.razon_social LIKE ''%' + @containsRazonSocial + '%'''
	END
	IF @numeroDocumentoIdentidad IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p.num_doc_iden = @num_doc_iden'
	END
	IF @comprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' nc.comprobante_id = @comprobante_id'
	END
	IF @containsVendedor IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' p2.razon_social LIKE ''%' + @containsVendedor + '%'''
	END
	IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(nc.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(nc.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @tipoNotaCreditoId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' nc.tipo_nota_c_id = @tipo_nota_c_id'
	END
	IF EXISTS (SELECT 1 FROM @tipoNotaCreditoList)
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' tnc.tipo_nota_c IN (SELECT [value] FROM @tipo_nota_c_tvp) '
	END
	IF @estadoComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ec.estado_compro_id = @estado_compro_id'
	END
	IF @tipoDocComprobanteId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.tipo_doc_com_id = @tipoDocComprobanteId' 
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' nc.estado = @estado'
	END
	IF @comprobante IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' c.numero = @comprobante'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@periodo_emp_sede_id INT,@numero CHAR(8),@num_doc_iden VARCHAR(35),@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@comprobante_id INT,@tipo_nota_c_id INT,@tipo_nota_c_tvp dbo.ListString READONLY,@estado_compro_id INT,@tipoDocComprobanteId INT,@comprobante CHAR(8),@estado CHAR(1)',
		@periodoEmpresaSedeId,@numero,@numeroDocumentoIdentidad,@fechaDesde,@fechaHasta,
		@comprobanteId,@tipoNotaCreditoId,@tipoNotaCreditoList,@estadoComprobanteId,@tipoDocComprobanteId,@comprobante,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_nota_credito_buscarPor actualizado correctamente.';
GO
-- -----------------------------------------------------------------------------------------
-- 16. Procedimiento: sp_perso_sol_acc_buscarPor
-- -----------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[sp_perso_sol_acc_buscarPor]
(
	@pageSize              INT,
	@orderBy               VARCHAR(100) = NULL,
	@fechaDesde            DATETIME = NULL,
	@fechaHasta            DATETIME = NULL,
	@usuarioId             INT = NULL,
	@estadoAprobacionId    INT = NULL,
	@estadoAprobacion      CHAR(1) = NULL,
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
	SET @usuarioId = NULLIF(@usuarioId,0)
	SET @estadoAprobacionId = NULLIF(@estadoAprobacionId,0)
	SET @estadoAprobacion = NULLIF(@estadoAprobacion,'')
	SET @estado = NULLIF(@estado,'')

	--// Construir expression SQL.
	SET @selectStat = 'SELECT '
		+ (CASE WHEN @pageSize > 0 THEN CONCAT('TOP(',CONVERT(NVARCHAR(10),@pageSize),') ') ELSE '' END)
		+ CHAR(13) +
		+ 'psa.perso_sol_acc_id,'
		+ 'psa.fecha,'
		+ 'psa.fecha_registro,'
		+ 'psa.hora_sol,'
		+ 'psa.hora_auto,'
		+ 'psa.fecha_auto,'
		+ 'psa.estado,'
		+ 'p.personal_id,'
		+ 'prv.proveedor_id,'
		+ 'prv.razon_social,'
		+ 'pa.personal_id,'
		+ 'prv2.proveedor_id,'
		+ 'prv2.razon_social,'
		+ 'ea.estado_aproba_id,'
		+ 'ea.estado_aproba,'
		+ 'ea.nombre'
		+ CHAR(13) + 'FROM perso_sol_acc psa'
		+ CHAR(13) + 'INNER JOIN personal p ON p.personal_id = psa.personal_id'
		+ CHAR(13) + 'INNER JOIN proveedor prv ON prv.proveedor_id = p.proveedor_id'
		+ CHAR(13) + 'LEFT JOIN personal pa ON pa.personal_id = psa.perso_auto_id'
		+ CHAR(13) + 'LEFT JOIN proveedor prv2 ON prv2.proveedor_id = pa.proveedor_id'
		+ CHAR(13) + 'INNER JOIN estado_aproba ea ON ea.estado_aproba_id = psa.estado_aproba_id'

	--// Parámetros de filtro.
	IF @fechaDesde IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(psa.fecha AS DATE) >= CAST(@fecha_desde AS DATE)'
	END
	IF @fechaHasta IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' CAST(psa.fecha AS DATE) <= CAST(@fecha_hasta AS DATE)'
	END
	IF @usuarioId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' prv.usuario_id = @usuario_id'
	END
	IF @estadoAprobacionId IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba_id = @estado_aproba_id'
	END
	IF @estadoAprobacion IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' ea.estado_aproba = @estado_aproba'
	END
	IF @estado IS NOT NULL
	BEGIN
		SET @whereStat = @whereStat + CHAR(13) + IIF(LEN(@whereStat) = 0, 'WHERE', 'AND') + ' psa.estado = @estado'
	END

	SET @selectStat = @selectStat + @whereStat

	--// Agregar ORDER BY.
	IF @orderBy IS NOT NULL
	BEGIN
		SET @selectStat = @selectStat + CHAR(13)+'ORDER BY ' + @orderBy
	END

	--// Ejecutar consulta dinámica.
	EXEC sp_executesql @selectStat,
		N'@usuario_id INT,@fecha_desde DATETIME,@fecha_hasta DATETIME,
		@estado_aproba_id INT,@estado_aproba CHAR(1),@estado CHAR(1)',
		@usuarioId,@fechaDesde,@fechaHasta,
		@estadoAprobacionId,@estadoAprobacion,@estado

	SET NOCOUNT OFF
END
GO
PRINT 'Procedimiento sp_perso_sol_acc_buscarPor actualizado correctamente.';
GO
PRINT '=======================================================';
PRINT 'Migracion de procedimientos completada exitosamente.';
PRINT '=======================================================';
GO
