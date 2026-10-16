namespace ESD.ERP.Common
{
   public class SiNoConstante
   {
      public const string S = "S";
      public const string N = "N";
   }
   public class SiNoTextoConstante
   {
      public const string S = "SI";
      public const string N = "NO";
   }
   public class EstadoConstante
   {
      public const string ACTIVO = "A";
      public const string INACTIVO = "I";
   }
   public class EstadoNombreConstante
   {
      public const string ACTIVO = "ACTIVO";
      public const string INACTIVO = "INACTIVO";
   }
   public class OrigenRegistroConstante
   {
      public const string SISTEMA = "01";
      public const string USUARIO = "02";
   }
   public class PaisConstante
   {
      public const string DEFAULT = "9589";
   }
   public class DepartamentoConstante
   {
      public const string DEFAULT = "15";
   }
   public class ProvinciaConstante
   {
      public const string DEFAULT = "01";
   }
   public class DistritoConstante
   {
      public const string DEFAULT = "01";
   }
   public class TipoViaConstante
   {
      public const string DEFAULT = "12";
   }
   public class TipoZonaConstante
   {
      public const string DEFAULT = "12";
   }
   public class RegimenSaludConstante
   {
      public const string DEFAULT = "00";
      public const string ESSALUD_REGULAR_Y_EPS = "01";
   }
   public class RegimenPensionarioConstante
   {
      public const string DEFAULT = "99";
      public const string SIN_REG_PENSIONARIO = "99";
      public const string ONP = "02";
   }
   public class SctrSaludConstante
   {
      public const string DEFAULT = "1";
   }
   public class TipoContratoConstante
   {
      public const string DEFAULT = "99";
   }
   public class PeriodicidadRemunerativaConstante
   {
      public const string DEFAULT = "1";
   }
   public class EstadoCivilConstante
   {
      public const string DEFAULT = "S";
   }
   public class TipoPagoRemuneracionConstante
   {
      public const string DEFAULT = "1";
      public const string DEPOSITO_EN_CUENTA = "2";
   }
   public class SituacionEspecialTrabajadorConstante
   {
      public const string DEFAULT = "0";
   }
   public class CargoPersonalConstante
   {
      public const string DEFAULT = "00000";
   }
   public class OcupacionConstante
   {
      public const string DEFAULT = "999001";
   }
   public class ConvenioDobleTributacionConstante
   {
      public const string DEFAULT = "0";
   }
   public class TipoTrabajadorConstante
   {
      public const string DEFAULT = "21";
   }
   public class TipoNaturalezaJuridicaConstante
   {
      public const string NATURAL = "1";
      public const string JURIDICO = "2";
   }
   public class RegimenLaboralConstante
   {
      public const string DEFAULT = "99";
   }
   public class TablaConstante
   {
      public const string EMPRESA_SEDE = "0001";
      public const string PERSONAL = "0002";
      public const string PROVEEDOR = "0003";
      public const string HORA_REMUNERATIVA = "0004";
      public const string CONCEPTO_REMUNERATIVO = "0005";
      public const string CARGO_PERSONAL = "0006";
      public const string PRESTAMO = "0007";
      public const string ROL = "0008";
      public const string FAMILIA_PRODUCTO = "0011";
      public const string PRODUCTO = "0012";
      public const string VENDEDOR = "0013";
      public const string TRANSACCION_ALMACEN = "0014";
      public const string COTIZACION = "0015";
      public const string MEDIO_PAGO = "0016";
      public const string COMPROBANTE_PAGO = "0017";
      public const string RECIBO_INGRESO = "0018";
      public const string NOTA_CREDITO = "0019";
      public const string ORDEN_COMPRA = "0020";
      public const string RECLASIFICACION_ITEM = "0021";
      public const string CONDICION_PAGO = "0022";
      public const string ORDEN_SERVICIO = "0023";
      public const string PEDIDO_COMPRA = "0024";
      public const string ORDEN_PRODUCCION = "0025";
      public const string CLIENTE_MK = "0026";
   }
   public class TipoDocumentoIdentidadConstante
   {
      public const string DEFAULT = "01";
      public const string DNI = "01";
      public const string RUC = "06";
      public const string PASAPORTE = "07";
      public const string DOC_ID_EXTRANJERO = "24";
   }
   public class GrupoParametroSistemaConstante
   {
      public const string GENERAL = "01";
      public const string SSRS_CONFIGURATION = "02";
      public const string EMAIL_CONFIGURATION = "03";
      public const string CAPITAL_HUMANO = "05";
      public const string VENTAS = "06";
      public const string VENTAS_DETRACCION = "07";
      public const string FACTURACION_ELECTRONICA = "08";
   }
   public class TipoDatoConstante
   {
      public const string CADENA = "01";
      public const string ENTERO = "02";
      public const string DECIMAL = "03";
      public const string CADENA_OCULTA = "07";
      public const string RANGO_HORA = "08";
   }
   public class TipoVariableCalculoConstante
   {
      public const string VARIABLE = "VA";
      public const string CAMPO_HORA = "CH";
      public const string CAMPO_GENERAL= "CA";
      public const string VARIABLE_TABLA= "VT";
   }
   public class EstadoPlanillaProcesoConstante
   {
      public const string ABIERTO = "01";
      public const string CERRADA = "02";
   }
   public class ParametroSistemaConstante
   {
      public static readonly string SSRS_BASE_PATH = "0001";
      public static readonly string SSRS_USE_CREDENTIAL = "0002";
      public static readonly string SSRS_USER = "0003";
      public static readonly string SSRS_PASSWORD = "0004";

      public static readonly string HABILITAR_ENVIO_EMAIL = "0005";
      public static readonly string SERVIDOR_EMAIL = "0006";
      public static readonly string PUERTO_SMTP = "0007";
      public static readonly string USUARIO_EMAIL = "0008";
      public static readonly string CONTRASENA_EMAIL = "0009";
      public static readonly string USAR_SSL = "0010";

      public const string PERSONAL_PAIS_EMISOR_X_TIPO_DOC = "0013";
      public const string PERSONAL_SITUACION_EDUCATIVA_EXCLU_X_TIPO_TRABAJADOR = "0014";
      public const string PERSONAL_CATEG_OCUPACIONAL_EXCLU_X_TIPO_TRABAJADOR = "0015";

      public const string VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION = "0016";
      public const string VENTAS_BANCARIZACION = "0024";

      public const string DETRACCION_MONTO_MAX_SERVICIOS = "0017";
      public const string DETRACCION_MONTO_MAX_PRODUCTOS = "0018";

      public const string FACTURACION_E_URL = "0019";

      public const string DATOS_MATRICIAL_URL = "0023";

      public const string ALERTA_STOCK_MINIMO = "0025";

      public const string HORARIO_ACCESO_LV = "0026";
      public const string HORARIO_ACCESO_DIARIO = "0027";
   }
   public class SituacionTrabajadorConstante
   {
      public const string DEFAULT = "1";
      public const string BAJA = "0";
      public const string ACTIVO = "1";
   }
   public class CategoriaOcupacionalConstante
   {
      public const string EJECUTIVO = "01";
      public const string OBRERO = "02";
      public const string EMPLEADO = "03";
   }
   public class TipoCancelacionCuotaConstante
   {
      public const string DESCUENTO = "D";
   }
   public class TipoPrestamoConstante
   {
      public const string ADELANTO = "A";
   }
   public class EstadoCuotaConstante
   {
      public const string PENDIENTE = "P";
      public const string SUSPENDIDO = "S";
      public const string PAGADO = "G";
   }
   public class EstadoPrestamoConstante
   {
      public const string PENDIENTE = "P";
      public const string PAGADO = "G";
      public const string ANULADO = "A";
   }
   public class ModuloConstante
   {
      public const string CAPITAL_HUMANO = "RH";
      public const string VENTA = "VT";
      public const string INVENTARIOS = "IV";
   }
   public class TipoProcesoIntegracionConstante
   {
      public const string SERVICIO_REST = "SR";
      public const string JOB_BASE_DATOS = "JB";
   }
   public class TipoIgvConstante
   {
      public const string DEFAULT = "01";
      public const string GRAVADO = "01";
      public const string INAFECTO = "03";
      public const string GRATUITA = "05";
   }
   public class EstadoProductoConstante
   {
      public const string ACTIVO = "A";
      public const string INACTIVO = "I";
      public const string DESCONTINUADO = "D";
   }
   public class EstadoComercialProductoConstante
   {
      public const string STOCK = "S";
   }
   public class UnidadMedidaConstante
   {
      public const string DEFAULT = "008";
      public const string UNIDAD = "008";
   }
   public class MonedaConstante
   {
      public const string DEFAULT = "001";
      public const string SOLES = "001";
      public const string DOLARES = "002";
   }
   public class DataImportConstant
   {
      public const string LISTA_PRECIO_GENERAL = "0001";
      public const string VOLUMEN_EXISTENCIAS = "0002";
      public const string PROVEEDOR_LISTA_PRECIO_PRODUCTO = "0003";
   }
   public class EstadoMovimientoAlmacenConstante
   {
      public const string MOVIMIENTO = "M";
      public const string TRANSITO = "T";
      public const string ANULADO = "9";
   }
   public class TipoProductoConstante
   {
      public const string PRODUCTO = "P";
      public const string SERVICIO = "S";
   }
   public class GrupoProductoConstante
   {
      public const string ACABADOS = "ACB";
      public const string DRYWALL = "DRY";
      public const string FERRETERIA = "FER";
      public const string MADERA = "MAD";
      public const string MELAMINA = "MEL";
      public const string TABLEROS = "TAB";
      public const string TAPACANTOS = "TAP";
      public const string SERVICIOS = "SVC";
      public const string COMBOS = "CMB";
      public const string MERCHANDISING = "MER";
   }
   public class TipoMovimientoAlmacenConstante
   {
      public const string INGRESO = "I";
      public const string SALIDA = "S";
   }
   public class EstadoCotizacionConstante
   {
      public const string COTIZACION = "01";
      public const string PEDIDO = "02";
      public const string FACTURADO = "03";
      public const string FACTURADO_PARCIAL = "04";
      public const string ANULADO = "99";
   }
   public class TipoImpuestoConstante
   {
      public const string IGV = "01";
   }
   public class TipoAfectacionIgvConstante
   {
      public const string GRAVADO_OPERACION_ONEROSA = "10";
      public const string INAFECTO_RETIRO_POR_BONIFICACION = "32";
   }
   public class EstadoComprobanteConstante
   {
      public const string PENDIENTE = "1";
      public const string CERRADO = "2";
      public const string EMITIDO = "3";
      public const string ANULADO = "9";
   }
   public class TipoCondicionPagoConstante
   {
      public const string CONTADO = "1";
      public const string CREDITO = "2";
      public const string OBSEQUIO = "3";
   }
   public class CondicionPagoConstante
   {
      public const string CONTADO = "001";
      public const string CREDITO = "002";
   }
   public class MedioPagoConstante
   {
      public const string EFECTIVO = "01";
      public const string DEPOSITO_BANCARIO = "04";
   }
   public class TipoComprobanteConstante
   {
      public const string NORMAL = "1";
      public const string CANJE_DOCUMENTO = "2";
      public const string CANJE_SELECTIVO = "3";
      public const string TEMPORAL = "4";
      public const string ANTICIPO = "5";
   }
   public class TipoDetraccionConstante
   {
      public const string SIN_DETRACCION = "000";
      public const string MADERA = "001";
      public const string SERVICIOS = "999";
   }
   public class TipoDocumentoComercialConstante
   {
      public const string FACTURA = "01";
      public const string BOLETA_VENTA = "03";
      public const string GUIA_REMISION_REMITENTE = "09";
      public const string NOTA_CREDITO = "07";
      public const string GUIA_REMISION_TRANSPORTE = "31";
      public const string INTERNO = "30";
      public const string NOTA_CREDITO_INTERNO = "99";
   }
   public class TransaccionAlmacenConstante
   {
      public const string INGRESO_ORDEN_COMPRA_MANUAL = "S01";
      public const string SALIDA_POR_VENTAS = "S02";
      public const string RECLASIFICACION_DE_ITEMS_INGRESO = "S05";
      public const string RECLASIFICACION_DE_ITEMS_SALIDA = "S06";
      public const string INGRESO_MERMA = "S07";
      public const string SALIDA_MERMA = "S08";
      public const string SALIDA_CONSUMO_INTERNO = "S09";
      public const string DEVOLUCION_DE_VENTA = "S10";
      public const string TRANSFERENCIA_ENTRE_ALMACENES = "S11";
      public const string INGRESO_ORDEN_COMPRA = "S13";
   }
   public class EstadoSunatConstante
   {
      public const string PENDIENTE_GENERAR = "1";
      public const string PENDIENTE_ENVIO = "2";
      public const string VERIFICACION = "3";
      public const string APROBADO = "4";
      public const string APROBADO_CON_OBS = "5";
      public const string RECHAZADO = "6";
      public const string ARCHIVO_CON_ERROR = "E";
   }
   public class TipoAutorizacionModuloConstante
   {
      public const string COTIZACION = "0001";
      public const string PEDIDO = "0002";
      public const string COMPROBANTE = "0003";
   }
   public class TipoAutorizacionConstante
   {
      public const string DSCTO_EN_PRECIO_UNITARIO = "01";
      public const string NO_REQUERIR_ADELANTO = "02";
      public const string PEDIDO_CON_OP_MODIFICAR_ITEMS = "03";
    }
   public class EstadoAprobacionConstante
   {
      public const string PENDIENTE_APROBACION = "P";
      public const string APROBADO = "A";
      public const string DESAPROBADO = "D";
      public const string CANCELADO = "C";
      public const string EN_EDICION = "E";
   }
   public class MotivoTrasladoConstante
   {
      public const string VENTA = "01";
   }
   public class TipoCargoAbonoConstante
   {
      public const string CARGO = "C";
      public const string ABONO = "A";
   }
   public class TipoOperacionAnticipoConstante
   {
      public const string ANTICIPO = "01";
      public const string APLICACION = "02";
      public const string ANULACION = "03";
      public const string EXTORNO = "EX";
   }
   public class ProveedorConstante
   {
      public const string GENERICO = "0000000";
   }
   public class RolConstante
   {
      public const string SYS_ADMIN = "S0001";
      public const string LOCAL_ADMIN = "S0002";
      public const string ADMIN_VENTAS = "S0003";
      public const string TI = "00020";
      public const string PRICING = "00017";
      public const string CAJA_OPERADOR = "00005";
    }
    public class DirectivaReglaConstante
   {
      public const string ASIGNACION_DE_SEDES = "002";
   }
   public class DataFormatoImpresionConstante
   {
      public const string ALMACEN_GUIA_REMISION = "01";
      public const string PEDIDO_ADELANTO = "04";
      public const string COMPROBANTE_ADELANTO = "06";
      public const string PEDIDO_TAPACANTO = "05";
      public const string COMPROBANTE_TAPACANTO = "07";
      public const string CONSUMO_INTERNO = "10";
      public const string CONTROL_MERMA = "11";
   }
   public class MesContableConstante
   {
      public const string APERTURA = "00";
      public const string CIERRE = "13";
   }
   public class EstadoAperturaCierreConstante
   {
      public const string APERTURA = "A";
      public const string CIERRE = "C";
   }
   public class EstadoPedidoCompraConstante
   {
      public const string EN_EDICION = "1";
      public const string PEDIDO = "2";
      public const string CON_ORDEN_DE_COMPRA = "3";
      public const string ANULADA = "4";
      public const string MODIFICACION = "5";
   }
   public class EstadoOrdenCompraConstante
   {
      public const string EN_EDICION = "1";
      public const string ORDEN_COMPRA = "2";
      public const string INGRESO_PARCIAL = "3";
      public const string ATENDIDA = "4";
      public const string ANULADA = "5";
      public const string MODIFICACION = "6";
   }
   public class EstadoMovimientoReclasificacionConstante
   {
      public const string MOVIMIENTO = "1";
      public const string APROBADO = "2";
      public const string ANULADO = "9";
      public const string DESAPROBADO = "3";
      public const string PENDIENTE = "4";
   }
   public class TipoCambioConstante
   {
      public const string OFICIAL = "01";
      public const string VENTAS = "02";
   }
   public class TipoProveedorConstante
   {
      public const string CLIENTE = "C";
      public const string PROVEEDOR = "P";
   }
   public class ProductoConstante
   {
      public const string ANTICIPO = "ANTICIPO0001";
   }
   public class EstadoDespachoConstante
   {
      public const string PENDIENTE = "1";
      public const string ATENDIDO_PARCIAL = "2";
      public const string ATENDIDO = "3";
   }
   public class EstadoOrdenProduccionConstante
   {
      public const string PENDIENTE = "1";
      public const string EN_PROCESO = "2";
      public const string CULMINADA = "3";
      public const string ANULADA = "9";
   }
   public class EstadoOrdenProduccionSubProcesoConstante
   {
      public const string PENDIENTE = "1";
      public const string EN_PROCESO = "2";
      public const string CULMINADA = "3";
      public const string ANULADA = "9";
   }
   public class TipoDescuentoConstante
   {
      public const string SIN_DESCUENTO = "0";
      public const string CANTIDAD = "1";
   }
   public class TipoAccionAuditoriaConstante
   {
      public const string ELIMINACION = "1";
   }
   public class EstadoGuiaRemisionConstante
   {
      public const string EN_EDICION = "1";
      public const string EMITIDO = "2";
      public const string MODIFICACION = "3";
      public const string ANULADO = "9";
   }
   public class PersonalConstante
   {
      public const string VARIOS_TEXTO = "VARIOS";
   }
   public class TipoNotaCreditoConstante
   {
      public const string ANULACION_OPERACION = "01";
      public const string ANULACION_ERROR_RUC = "02";
   }
   public class DiaConstante
   {
      public const string SABADO = "6";
      public const string DOMINGO = "7";
   }
}