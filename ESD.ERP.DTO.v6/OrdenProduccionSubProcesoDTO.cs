using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenProduccionSubProcesoDTO : DTOBase
	{
		public override int Id => OrdenProduccionSubProcesoId;
		public int OrdenProduccionSubProcesoId { get; set; }
		public int OrdenProduccionId { get; set; }
		public int SubProcesoProduccionId { get; set; }
		public DateTime? FechaInicio { get; set; }
		public DateTime? FechaFin { get; set; }
		public int Orden { get; set; }
		public int? PersonalId { get; set; }
		public int? MaquinaId { get; set; }
		public int? SubProcesoProduccionAnteriorId { get; set; }
		public string SubProcesoProduccionAnteriorConcluido { get; set; }
		public string Observacion { get; set; }
		public int EstadoOrdenProduccionSubProcesoId { get; set; }
		public string Estado { get; set; } 
		public int AlmacenId { get; set; }
		public int MaquinaRealId { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
		public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; }
		public EstadoOrdenProduccionSubProcesoDTO EstadoOrdenProduccionSubProcesoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public MaquinaDTO MaquinaDTO { get; set; }
		public MaquinaDTO MaquinaRealDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }

    }
}
