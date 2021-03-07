using System;

namespace CUAFunding.ViewModels.BaseViewModel.ViewItems
{
    public class MarkViewItem
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public int Value { get; set; }
    }
}
