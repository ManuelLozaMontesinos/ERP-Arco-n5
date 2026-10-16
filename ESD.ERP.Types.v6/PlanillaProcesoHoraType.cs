namespace ESD.ERP.Types
{
   public class PlanillaProcesoHoraType
   {
		public int PlanillaProcesoHoraId { get; set; }
		public int PlanillaProcesoId { get; set; }
		public int HoraRemunerativaId { get; set; }
		public string Personal { get; set; }
		public int PersonalId { get; set; }
		public decimal Cantidad { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }

	}
}
