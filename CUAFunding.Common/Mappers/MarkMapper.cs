using System.Collections.Generic;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.BaseViewModel.ViewItems;
using System.Linq;
using CUAFunding.ViewModels.MarkViewModel;
using CUAFunding.Interfaces.Mappers;

namespace CUAFunding.Common.Mappers
{
    public class MarkMapper : IMarkMapper
    {
        public Mark Create(CreateMarkViewModel viewModel)
        {
            Mark mark = new Mark();
            mark.UserId = viewModel.UserId;
            mark.ProjectId = viewModel.ProjectId;
            mark.Value = viewModel.Value;

            return mark;
        }
        public Mark Edit(Mark mark, EditMarkViewModel viewModel)
        {
            mark.UserId = viewModel.UserId;
            mark.ProjectId = viewModel.ProjectId;
            mark.Value = viewModel.Value;

            return mark;
        }
        public EditMarkViewModel Edit(Mark mark)
        {
            EditMarkViewModel viewModel = new EditMarkViewModel();

            viewModel.UserId = mark.UserId;
            viewModel.ProjectId = mark.ProjectId;
            viewModel.Value = mark.Value;
            viewModel.ProjectName = mark.Project.Title;
            viewModel.UserName = mark.User.UserName;

            return viewModel;
        }
        public ShowMarksViewModel Show (IEnumerable<Mark> marks)
        {
            ShowMarksViewModel viewModel = new ShowMarksViewModel();

            viewModel.Marks = marks.Select(viewItem => new MarkViewItem()
            {
                Id = viewItem.Id,
                ProjectId = viewItem.ProjectId,
                UserId = viewItem.UserId,
                Value = viewItem.Value,
            });

            return viewModel;
        }
    }
}
