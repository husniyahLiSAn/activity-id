using Data.Base;
using System;

namespace Data.Model
{
    public class Transaction : BaseModel
    {
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int GrandTotal { get; set; }
        public User User { get; set; }

        public Transaction()
        {
            this.OrderDate = DateTime.Now;
            this.IsDelete = false;
        }
    }
}
