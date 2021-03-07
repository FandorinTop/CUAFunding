using System;
using System.Collections.Generic;
using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.Intefaces;


namespace CUAFunding.ViewModels.ProjectViewModel
{
    public class EditProjectViewModel : BaseProjectViewModel, IBaseEditViewModel
    {
        public Guid Id { get; set; }
        public int PageVisitorsCount { get; set; }
    }
}
