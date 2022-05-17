using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstWebApi.Models {
    public class Customer {

        public int Id { get; set; } = 0;
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public int Passcode { get; set; } = 0;
        [Column(TypeName = "decimal(9,2)")]
        public decimal Sales { get; set; } = 0;
        public bool Active { get; set; } = true;

    }
}
