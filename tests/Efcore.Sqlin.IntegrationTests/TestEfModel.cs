using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Efcore.Sqlin.IntegrationTests
{
    [Table("TestModel")]
    public class TestEfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(2000)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }
    }
}