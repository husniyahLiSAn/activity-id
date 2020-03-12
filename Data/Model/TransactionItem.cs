using Data.Base;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public class TransactionItem : BaseModel
    {
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Transaction Transaction { get; set; }
        public Item Item { get; set; }

        public TransactionItem() { }
        public TransactionItem(TransactionItemVM TransactionItemVM)
        {
            this.Quantity = TransactionItemVM.Quantity;
            this.CreateDate = DateTime.Now;
            this.IsDelete = false;
        }
    }
}
