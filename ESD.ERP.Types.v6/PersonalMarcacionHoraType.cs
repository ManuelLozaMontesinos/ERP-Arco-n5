namespace ESD.ERP.Types
{
   public class PersonalMarcacionHoraType
	{
		public int PersonalMarcacionHoraId { get; set; }
		public int PersonalMarcacionId { get; set; }
		public int HoraRemunerativaId { get; set; }
		public decimal Cantidad { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }

	}
}
