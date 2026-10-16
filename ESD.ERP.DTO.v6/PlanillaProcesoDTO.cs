using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PlanillaProcesoDTO : DTOBase
	{
		public override int Id => PlanillaProcesoId;
		public int PlanillaProcesoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int PlanillaId { get; set; }
		public short ProcesoAnual { get; set; }
		public int MesId { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoTipoCambio { get; set; }
		public int PeriodicidadRemunerativaId { get; set; }
		public DateTime FechaDesde { get; set; }
		public DateTime FechaHasta { get; set; }
		public DateTime? FechaProceso { get; set; }
		public int EstadoPlanillaProcesoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public string Nombre
      {
         get {
				if (PlanillaDTO == null)
					return string.Empty;

				return string.Concat(PlanillaDTO.Nombre, SeparadorConstante.ESPACIO,SeparadorConstante.GUION, SeparadorConstante.ESPACIO,
					MesDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO,
					PeriodicidadRemunerativaDTO.Nombre, SeparadorConstante.ESPACIO, SeparadorConstante.DOS_PUNTOS, SeparadorConstante.ESPACIO,
					"DEL ", string.Format(StringFormatConstant.DATE, FechaDesde)," AL ", string.Format(StringFormatConstant.DATE, FechaHasta));
			}
      }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public PlanillaDTO PlanillaDTO { get; set; }
		public MesDTO MesDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public EstadoPlanillaProcesoDTO EstadoPlanillaProcesoDTO { get; set; }
		public PeriodicidadRemunerativaDTO PeriodicidadRemunerativaDTO { get; set; }
	}
}
