using CUAFunding.DataAccess.Repository;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.Repository;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CUAFunding.Tests
{
    public class CUAFundingRepositories_ProjectRepository
    {
        private TestHelper _helper;
        private IProjectRepository _projectRepository;

        public CUAFundingRepositories_ProjectRepository()
        {
            _helper = new TestHelper();
            _projectRepository = new ProjectRepository(_helper.GetInMemoryRepo());
        }

        [Fact]
        public async Task Create_Project_With_Valid_Data()
        {

            var project = new Project()
            {
                Description = "desc",
                Title = "desc",
                Goal = 500
            };

            await _projectRepository.Insert(project);

            var res = await _projectRepository.Find(project.Id);

            Assert.NotNull(res);
        }

        [Fact]
        public async Task GetApiProject_With_NameFilter()
        {
            //Arrange
            var projectCount = 15;

            for (int i = 0; i < projectCount; i++)
            {
                var project = new Project()
                {
                    OwnerId = null,
                    Description = i.ToString(),
                    Title = "desc" + i.ToString(),
                    Goal = 500 + i,
                    ExpirationDate = DateTime.Now.AddDays(10),
                    Location = new Point(1+i, 1+i) { SRID = 4326 },
                };

                project.Marks = new List<Mark>() { 
                    new Mark() { ProjectId = project.Id, Value = i % 5 },
                    new Mark() { ProjectId = project.Id, Value = 3 } };
                await _projectRepository.Insert(project);
            }

            //Act
            var res = await _projectRepository.GetApiResult(0, 10, filterColumn: "Title", filterQuery: "desc1");

            //Assert
            Assert.NotNull(res);
            Assert.Equal(6, res.TotalCount);

            foreach (var item in res.Data)
            {
                Assert.Contains("desc1", item.Title);
            }
        }
    }
}
