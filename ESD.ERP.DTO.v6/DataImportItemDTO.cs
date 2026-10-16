using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class DataImportItemDTO : DTOBase
	{
		public override int Id => DataImportItemId;
		public int DataImportItemId { get; set; }
		public int DataImportId { get; set; }
		public string Columna { get; set; }
		public string Descripcion { get; set; }
		public int Orden { get; set; }
		public DataImportDTO DataImportDTO { get; set; }
	}
}
