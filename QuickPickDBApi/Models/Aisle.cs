using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickPickDBApi.Models
{
    public class Aisle
    {
        [Key]
        [Column("Aisle_Id")]
        public int Id { get; set; }
        public string? Aisle_Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
