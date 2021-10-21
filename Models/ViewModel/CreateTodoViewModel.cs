using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.ViewModel
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}