using System;
using System.Collections.Generic;

namespace backend.Models.DTO
{
    public class TaskDTO
    {
        public int TaskId { get; set; }

        public string TaskTitle { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public bool IsDone { get; set; }
        public string TagName { get; set; }
        public string Color { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();

    }
}
