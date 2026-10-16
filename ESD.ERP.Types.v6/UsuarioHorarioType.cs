namespace ESD.ERP.Types
{
	public class UsuarioHorarioType
	{
		public int UsuarioHorarioId { get; set; }
		public int UsuarioId { get; set; }
		public int DiaId { get; set; }
		public string HoraInicio { get; set; }
		public string HoraFin { get; set; }
		public string HoraExtension { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
	}
}
