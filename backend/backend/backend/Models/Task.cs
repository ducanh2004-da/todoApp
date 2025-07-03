using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace backend.Models
{
    public class Task
    {
        
        public int TaskId { get; set; }  

        [MaxLength(100)]
        public string TaskTitle { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public string Priority { get; set; } = "Low";

        public string StartDay { get; set; }

        public string EndDay { get; set; }

        public bool IsDone { get; set; } = false;

        [Column(TypeName = "datetime")]
        public DateTime Created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Updated_at { get; set; }
    }
}
