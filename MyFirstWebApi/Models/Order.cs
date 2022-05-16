using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstWebApi.Models {

    public class Order {

        public int Id { get; set; } = 0;
        [StringLength(80)]
        public string Description { get; set; } = String.Empty;
        [Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; } = 0;
        public string Status { get; set; } = string.Empty;

        public int CustomerId { get; set; } = 0;
        public virtual Customer? Customer { get; set; } = null!;

        public virtual List<Orderline> Orderlines { get; set; } = new List<Orderline>();

    }
}
