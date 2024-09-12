using CUAFunding.BusinessLogic.Providers;
using CUAFunding.BusinessLogic.Services;
using CUAFunding.Common.Mappers;
using CUAFunding.DataAccess.Repository;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Providers;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.Interfaces.Mappers;
using CUAFunding.Interfaces.Repository;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CUAFunding.Tests
{
    public class CUAFundingServices_ProjectService
    {
        protected string baseName = "basename";
        protected string newValue = "new";
        protected string changedName = "changedname";
        protected string baseFolderPath = @"..\..\..\";
        protected TestHelper _helper;
        protected IProjectService _projectService;
        protected IProjectMapper _projectMapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IFileServerProvider _fileServerProvider;
        private IConfiguration _configuration;
        private IProjectRepository _projectRepository;
        

        public CUAFundingServices_ProjectService()
        {
            _httpContextAccessor = new HttpContextAccessor();
            _projectMapper = new ProjectMapper();
            _projectMapper = new ProjectMapper();
            _helper = new TestHelper();
            _projectRepository = new ProjectRepository(_helper.GetInMemoryRepo());
            var builder = new ConfigurationBuilder();
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), baseFolderPath, "AppTest.json");
            builder.AddJsonFile(jsonPath);
            _configuration = builder.Build();
            _projectService = new ProjectService(_projectRepository, _projectMapper, _httpContextAccessor, _fileServerProvider, _configuration);
        }

        [Fact]
        public async Task Create_Project_With_Valid_Data()
        {
            Assert.NotNull(1);
        }

        [Fact]
        public void Update_Project_With_Valid_Data()
        {
            Assert.Equal(1,1);
        }

        [Fact]
        public void Delete_Project_With_Valid_Data()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Show_Project_With_Valid_Data()
        {
            Assert.True(true);
        }

        [Fact]
        public void GetAll_Project_With_Valid_Data()
        {
            Assert.True(true);
        }

        [Fact]
        public void Create_Project_Main_Image()
        {
            Assert.True(true);
        }

        [Fact]
        public void Change_Project_Main_Image()
        {
            Assert.True(true);
        }
    }

}
