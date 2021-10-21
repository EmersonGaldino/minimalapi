using System;

namespace TodoList.Models.Base
{
    public class BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Update { get; set; }
        public bool Active { get; set; } = true;
    }
}