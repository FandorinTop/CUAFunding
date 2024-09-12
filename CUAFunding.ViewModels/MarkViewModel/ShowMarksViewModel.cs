using System.Collections.Generic;
using CUAFunding.ViewModels.BaseViewModel.ViewItems;

namespace CUAFunding.ViewModels.MarkViewModel
{
    public class ShowMarksViewModel
    {
        public IEnumerable<MarkViewItem> Marks { get; set; }

        public ShowMarksViewModel()
        {
            Marks = new List<MarkViewItem>();
        }
    }
}
