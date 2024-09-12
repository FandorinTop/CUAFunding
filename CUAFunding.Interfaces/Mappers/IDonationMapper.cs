using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.ProjectViewModel;

namespace CUAFunding.Interfaces.Mappers
{
    public interface IProjectMapper
    {
        public Project Create(CreateProjectViewModel viewModel);
        public EditProjectViewModel Edit(Project project, decimal collected = 0, double rating = 0);
        public Project Edit(Project project, EditProjectViewModel viewModel);
        public ShowProjectViewModel Show(Project project, decimal collected = 0, double rating = 0);
    }
}
