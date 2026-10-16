using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
    [Table("catalogos_prod")]
    public class CatalogosProd : BaseEntity
    {
        public override int EntityId => CatalogosProdId;
        [Key]
        [Column("catalogos_prod_id", Order = 1)]
        public int CatalogosProdId { get; set; }
    }
}