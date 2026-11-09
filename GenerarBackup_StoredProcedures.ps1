param (
    [string]$Server = "",
    [string]$Database = "esderp_v6",
    [string]$OutputFile = "Backup_SPs_Produccion_20261109.sql"
)

# Si no se pasó el servidor por parámetro, solicitarlo por consola
if ([string]::IsNullOrWhiteSpace($Server)) {
    Write-Host "==========================================================" -ForegroundColor Cyan
    Write-Host " BACKUP DE PROCEDIMIENTOS ALMACENADOS (PREVIO A PASE)    " -ForegroundColor Cyan
    Write-Host "==========================================================" -ForegroundColor Cyan
    $Server = Read-Host "Ingresa el nombre o IP del servidor SQL de Produccion (ej: localhost o IP del servidor)"
    if ([string]::IsNullOrWhiteSpace($Server)) {
        $Server = "localhost"
    }
}

$connStr = "Server=$Server;Database=$Database;Trusted_Connection=True;TrustServerCertificate=True;"

$procNames = @(
    "sp_pedido_co_buscarPor",
    "sp_orden_co_buscarPor",
    "sp_orden_c_comp_buscarPor",
    "sp_mov_almacen_buscarPorEmpresa",
    "sp_mov_almacen_buscarPor",
    "sp_guia_remision_buscarPor",
    "sp_mov_reclasi_buscarPor",
    "sp_cotiza_buscarPor",
    "sp_cotiza_autoriza_buscarPor",
    "sp_comprobante_buscarPor",
    "sp_comprobante_buscarPorEmpresaSede",
    "sp_comprob_autoriza_buscarPor",
    "sp_comprobante_buscarDespacho",
    "sp_orden_pro_buscarPor",
    "sp_nota_credito_buscarPor",
    "sp_perso_sol_acc_buscarPor"
)

Write-Host "Conectando a [$Server].[$Database]..." -ForegroundColor Yellow

Add-Type -AssemblyName System.Data
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)

try {
    $conn.Open()
    Write-Host "Conexion exitosa!" -ForegroundColor Green
} catch {
    Write-Error "No se pudo conectar a la base de datos: $_"
    exit 1
}

$sb = [System.Text.StringBuilder]::new()
$sb.AppendLine("-- =========================================================================================") | Out-Null
$sb.AppendLine("-- COPIA DE SEGURIDAD (BACKUP) DE PROCEDIMIENTOS ALMACENADOS - PRODUCCION") | Out-Null
$sb.AppendLine("-- Servidor: $Server") | Out-Null
$sb.AppendLine("-- Base de Datos: $Database") | Out-Null
$sb.AppendLine("-- Fecha de generacion: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')") | Out-Null
$sb.AppendLine("-- =========================================================================================") | Out-Null
$sb.AppendLine("USE [$Database];") | Out-Null
$sb.AppendLine("GO") | Out-Null

$i = 1
foreach ($p in $procNames) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT OBJECT_DEFINITION(OBJECT_ID('$p'))"
    $def = [string]$cmd.ExecuteScalar()
    
    if (-not [string]::IsNullOrWhiteSpace($def)) {
        # Si tiene CREATE PROCEDURE, asegurar ALTER PROCEDURE para facilitar un rollback
        $def = $def -replace '(?i)^\s*CREATE\s+PROCEDURE', 'ALTER PROCEDURE'
        
        $sb.AppendLine("-- -----------------------------------------------------------------------------------------") | Out-Null
        $sb.AppendLine("-- $i. Procedimiento: $p") | Out-Null
        $sb.AppendLine("-- -----------------------------------------------------------------------------------------") | Out-Null
        $sb.AppendLine($def.Trim()) | Out-Null
        $sb.AppendLine("GO") | Out-Null
        Write-Host "  [$i/16] Extraid: $p" -ForegroundColor Green
    } else {
        Write-Warning "  [$i/16] No se encontro definicion para: $p"
    }
    $i++
}

$conn.Close()

$fullOutPath = [System.IO.Path]::GetFullPath($OutputFile)
[System.IO.File]::WriteAllText($fullOutPath, $sb.ToString(), [System.Text.Encoding]::UTF8)

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " Backup completado exitosamente!" -ForegroundColor Green
Write-Host " Archivo generado en: $fullOutPath" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Cyan

