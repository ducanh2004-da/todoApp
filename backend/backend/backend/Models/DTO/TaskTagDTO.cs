using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace backend.Models.DTO
{
    public class TaskTagDTO
    {
        public int TaskTagId { get; set; }
        public int TaskId { get; set; }
        public int TagId { get; set; }

        public string TagName { get; set; }
        public string Tag { get; set; }
    }
}
