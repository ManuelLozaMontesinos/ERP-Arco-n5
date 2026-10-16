namespace ESD.ERP.DTO
{
	public class TablaCorrelativoDTO
	{
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public string Tabla { get; set; }
		public bool UsarSedeCentral { get; set; }
		public int UsuarioAccionId { get; set; }
	}
}