using System;

namespace backend.Models
{
    public class TaskCsvModel
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public bool IsDone { get; set; }
    }

}
