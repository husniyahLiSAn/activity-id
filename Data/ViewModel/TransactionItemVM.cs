using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class TransactionItemVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Pay { get; set; }
        public int Total { get; set; }
        public int Stock { get; set; }
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Email { get; set; }
        public string SupplierName { get; set; }
    }
}
