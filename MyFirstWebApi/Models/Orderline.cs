using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyFirstWebApi.Models {

    public class Orderline {

        public int Id { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        [StringLength(30)]
        public string Product { get; set; } = string.Empty;
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; } = 0;

        public int OrderId { get; set; } = 0;
        [JsonIgnore]
        public virtual Order? Order { get; set; } = null;

    }
}
