using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("TaskTags")]
    public class TaskTag
    {
        [Key]
        public int TaskTagId { get; set; }

        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; }

        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
