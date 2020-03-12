using Data.Model;
using Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class Paging
    {
        public IEnumerable<ToDoListVM> dataActivities { get; set; }
        public IEnumerable<ItemVM> dataItems { get; set; }
        public IEnumerable<TransactionItemVM> dataTransactionItems { get; set; }
        public int length { get; set; }
        public int filteredLength { get; set; }

    }
}
