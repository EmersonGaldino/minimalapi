using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }

}