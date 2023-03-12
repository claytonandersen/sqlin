using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Efcore.Sqlin.IntegrationTests
{
    [Table("TestIntegerModel")]
    public class TestIntegerKeyModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(2000)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }
    }

    [Table("TestGuidModel")]
    public class TestGuidKeyModel
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(2000)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }
    }

    [Table("TestBigIntegerModel")]
    public class TestBigIntegerKeyModel
    {
        [Key]
        public long Id { get; set; }

        [StringLength(2000)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }
    }
}