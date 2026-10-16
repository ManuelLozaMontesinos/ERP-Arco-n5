using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("data_import")]
	public class DataImport : BaseEntity
	{
		public override int EntityId => DataImportId;
		[Key]
		[Column("data_import_id", Order = 1)]
		public int DataImportId { get; set; }

		[Column("data_import")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }
	}
}
