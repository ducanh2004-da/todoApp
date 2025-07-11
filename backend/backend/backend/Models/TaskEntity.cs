using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Tasks")]
    public class TaskEntity
    {
        [Key]
        public int TaskId { get; set; }

        [Required, MaxLength(200)]
        public string TaskTitle { get; set; }

        public string Description { get; set; }
        [Required]
        public string Priority { get; set; } = "Low";

        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }

        public bool IsDone { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
