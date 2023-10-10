using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FarmaciaGeneration.Model
{
    public class Produto
    {

        [Key] // Primary Key Id
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Identity 1,1
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(250)]
        public string Nome { get; set; } = string.Empty;

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,2)")]
        public decimal Preco { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(450)]
        public string Foto { get; set; } = string.Empty;

    }
}
