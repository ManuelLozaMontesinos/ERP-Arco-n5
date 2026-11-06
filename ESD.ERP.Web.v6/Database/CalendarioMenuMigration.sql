-- =========================================================================================
-- Script de Migracion: Alta de 'Calendario' en la tabla dbo.menu y permisos en dbo.rol_menu
-- Fecha: 02/11/2026
-- ERP SIGANET ARCO
-- Permite que 'Calendario' forme parte del catalogo de menus y sea gestionable por Roles (Lectura/Escritura).
-- =========================================================================================
USE [esderp_v6];
GO

-- 1. Insertar Calendario en dbo.menu si no existe
IF NOT EXISTS (SELECT 1 FROM dbo.menu WHERE url = 'calendario/index' OR nombre = 'Calendario')
BEGIN
    DECLARE @newMenuId INT;
    SELECT @newMenuId = ISNULL(MAX(menu_id), 0) + 1 FROM dbo.menu;

    INSERT INTO dbo.menu (menu_id, nombre, area, url, icono, menu_padre_id, orden, estado)
    VALUES (@newMenuId, 'Calendario', '', 'calendario/index', 'far fa-calendar-alt', NULL, 100, 'A');

    IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'menu_seq')
    BEGIN
        DECLARE @seqSql NVARCHAR(MAX);
        SET @seqSql = N'ALTER SEQUENCE dbo.menu_seq RESTART WITH ' + CAST((@newMenuId + 1) AS NVARCHAR(20));
        EXEC sp_executesql @seqSql;
    END

    PRINT '[OK] Menu Calendario insertado en dbo.menu con ID ' + CAST(@newMenuId AS VARCHAR(10));
END
ELSE
BEGIN
    PRINT '[OK] El menu Calendario ya existe en dbo.menu.';
END
GO

-- 2. Asignar permiso inicial completo (Lectura='S', Escritura='S') al rol Administradores (rol_id = 1)
DECLARE @calMenuId INT;
SELECT @calMenuId = menu_id FROM dbo.menu WHERE url = 'calendario/index' OR nombre = 'Calendario';

IF @calMenuId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.rol_menu WHERE rol_id = 1 AND menu_id = @calMenuId)
    BEGIN
        DECLARE @newRolMenuId INT;
        SELECT @newRolMenuId = ISNULL(MAX(rol_menu_id), 0) + 1 FROM dbo.rol_menu;

        INSERT INTO dbo.rol_menu (rol_menu_id, rol_id, menu_id, lectura, escritura)
        VALUES (@newRolMenuId, 1, @calMenuId, 'S', 'S');

        IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'rol_menu_seq')
        BEGIN
            DECLARE @seqRolSql NVARCHAR(MAX);
            SET @seqRolSql = N'ALTER SEQUENCE dbo.rol_menu_seq RESTART WITH ' + CAST((@newRolMenuId + 1) AS NVARCHAR(20));
            EXEC sp_executesql @seqRolSql;
        END

        PRINT '[OK] Permiso de Calendario (L/E) asignado a Administradores (rol_id = 1).';
    END
    ELSE
    BEGIN
        UPDATE dbo.rol_menu
        SET lectura = 'S', escritura = 'S'
        WHERE rol_id = 1 AND menu_id = @calMenuId;

        PRINT '[OK] Permisos de Calendario verificados para Administradores.';
    END
END
GO

