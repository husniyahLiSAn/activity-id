using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class ToDoListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int StatusActivity { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime CompletedTime { get; set; }
    }
}
