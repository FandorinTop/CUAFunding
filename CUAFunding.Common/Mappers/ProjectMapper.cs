using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels.ProjectViewModel;
using CUAFunding.ViewModels.BaseViewModel;
using CUAFunding.Interfaces.Mappers;
using System.Collections.Generic;
using System;
using CUAFunding.Common.Exceptions;

namespace CUAFunding.Common.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        public BaseProjectViewModel BaseMapper(Project project, BaseProjectViewModel viewModel)
        {
            viewModel.OwnerId = project.OwnerId;
            viewModel.Title = project.Title;
            viewModel.Description = project.Description;
            viewModel.Goal = project.Goal;
            viewModel.ImagePath = project.ImagePath;
            viewModel.ExpirationDate = project.ExpirationDate;

            return viewModel;
        }

        public Project BaseMapper(BaseProjectViewModel viewModel)
        {
            var project = new Project();

            project.OwnerId = viewModel.OwnerId;
            project.Title = viewModel.Title;
            project.Description = viewModel.Description;
            project.Goal = viewModel.Goal;
            project.ImagePath = viewModel.ImagePath;
            project.ExpirationDate = viewModel.ExpirationDate;

            return project;
        }

        public Project Create(CreateProjectViewModel viewModel)
        {
            Project project = BaseMapper(viewModel);
            ProjectValidation(project, "Create");

            return project;
        }

        public EditProjectViewModel Edit(Project project, decimal collected=0, double rating=0)
        {
            EditProjectViewModel viewModel = new EditProjectViewModel();
            viewModel = BaseMapper(project, viewModel) as EditProjectViewModel;
            viewModel.PageVisitorsCount = project.PageVisitorsCount;

            return viewModel;
        }

        public Project Edit(Project project, EditProjectViewModel viewModel)
        {        
            project.OwnerId = viewModel.OwnerId;
            project.Title = viewModel.Title;
            project.Description = viewModel.Description;
            project.Goal = viewModel.Goal;
            project.ImagePath = viewModel.ImagePath;
            project.ExpirationDate = viewModel.ExpirationDate;
            ProjectValidation(project, nameof(Edit));

            return project;
        }

        public ShowProjectViewModel Show(Project project, decimal collected = 0, double rating = 0)
        {
            ShowProjectViewModel viewModel = new ShowProjectViewModel();
            viewModel = BaseMapper(project, viewModel) as ShowProjectViewModel;
            viewModel.Id = project.Id;
            viewModel.PageVisitorsCount = project.PageVisitorsCount;
            viewModel.Сollected = collected;
            viewModel.AvgRating = rating;

            return viewModel;
        }

        protected virtual void ProjectValidation(Project project, string nameOfFunction)
        {
            MapperException exception = new MapperException("Project mapper Error at:" + nameOfFunction);

            if (String.IsNullOrWhiteSpace(project.Title))
            {
                exception.AddValidationMistake(new KeyValuePair<string, string>($"{nameof(project.Title)}", "Project Title can`t be Void Or WhiteSpace"));
            }
            if (String.IsNullOrWhiteSpace(project.Description))
            {
                exception.AddValidationMistake(new KeyValuePair<string, string>($"{nameof(project.Description)}", "Project Description can`t be Void Or WhiteSpace"));
            }
            if (project.Goal < 0)
            {
                exception.AddValidationMistake(new KeyValuePair<string, string>($"{nameof(project.Goal)}", "Project goal can`t be less than 0"));
            }
            if (DateTime.Compare(project.ExpirationDate.ToUniversalTime(), DateTime.UtcNow) <= 0)
            {
                exception.AddValidationMistake(new KeyValuePair<string, string>($"{nameof(ProjectMapper)}", "Project ExpirationDate can`t be less now"));
            }
            if (exception.ValidationMistakeCount >= 1)
            {
                throw exception;
            }
        }
    }
}
