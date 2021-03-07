using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.BaseViewModel
{
    public class BaseMarkViewModel
    {
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public int Value { get; set; }
    }
}
