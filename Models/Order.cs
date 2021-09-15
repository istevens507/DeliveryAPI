using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        [Column(TypeName = "int")]
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CreatedBy { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [AllowNull]
        public string UpdateBy { get; set; }

        [Column(TypeName = "datetime")]
        [AllowNull]
        public DateTime? UpdatedOn { get; set; }

    }
}
