using System;
using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.ViewModels.BaseViewModel.Intefaces;


namespace CUAFunding.ViewModels.MarkViewModel
{
    public class EditMarkViewModel : BaseMarkViewModel, IBaseEditViewModel
    {
        public string Id { get; set; }
    }
}
