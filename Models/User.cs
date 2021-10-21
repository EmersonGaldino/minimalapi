using System;
using System.Collections.Generic;
using TodoList.Models.Base;

namespace TodoList.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Todo> Todo { get; set; }
    }
}