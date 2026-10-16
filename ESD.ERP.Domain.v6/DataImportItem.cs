using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("data_imp_item")]
	public class DataImportItem : BaseEntity
	{
		public override int EntityId => DataImportItemId;
		[Key]
		[Column("data_imp_item_id", Order = 1)]
		public int DataImportItemId { get; set; }

		[Column("data_import_id")]
		public int DataImportId { get; set; }

		[Column("columna")]
		public string Columna { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("orden")]
		public int Orden { get; set; }

		public DataImport DataImport { get; set; }
	}
}
