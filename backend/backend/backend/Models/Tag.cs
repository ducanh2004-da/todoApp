using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [MaxLength(100)]
        public string TagName { get; set; }

        [MaxLength(100)]
        public string Color { get; set; }
    }
}
