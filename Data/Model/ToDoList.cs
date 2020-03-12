using Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class ToDoList : BaseModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public User User { get; set; }
    }
}
