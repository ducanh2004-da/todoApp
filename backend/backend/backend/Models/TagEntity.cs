using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Tags")]
    public class TagEntity
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; }

        public string Color { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
