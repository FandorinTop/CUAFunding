using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.MarkViewModel;
using System.Collections.Generic;

namespace CUAFunding.Interfaces.Mappers
{
    public interface IMarkMapper
    {
        public Mark Create(CreateMarkViewModel viewModel);
        public Mark Edit(Mark mark, EditMarkViewModel viewModel);
        public EditMarkViewModel Edit(Mark mark);
        public ShowMarksViewModel Show(IEnumerable<Mark> marks);
    }
}
