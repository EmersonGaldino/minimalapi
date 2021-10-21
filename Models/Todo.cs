using TodoList.Models.Base;

namespace TodoList.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}