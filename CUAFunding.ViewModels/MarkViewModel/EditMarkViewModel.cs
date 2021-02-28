using System;
using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.Intefaces;


namespace CUAFunding.ViewModels.MarkViewModel
{
    public class EditMarkViewModel : BaseMarkViewModel, IBaseEditViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
    }
}
