using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels.MarkViewModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class MarkService : IMarkService
    {
        private readonly ApplicationDbContext _context;

        public MarkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMark(CreateMarkViewModel viewModel)
        {
            var project = await _context.Projects.FindAsync(viewModel.ProjectId);
            var user = await _context.ApplicationUsers.FindAsync(viewModel.UserId);

            var mark = new Mark()
            {
                Project = project,
                User = user,
                Value = viewModel.Value
            };

            await _context.Marks.AddAsync(mark);
            var result = await _context.SaveChangesAsync();

            return result >= 0 ? true : false;
        }

        public async Task<bool> UpdateMark(EditMarkViewModel viewModel)
        {
            var mark = await _context.Marks
                .FirstOrDefaultAsync(item => item.ProjectId == viewModel.ProjectId && item.UserId == viewModel.UserId);

            if(mark != null)
            {
                mark.Value = viewModel.Value;
                var result = await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return await AddMark(new CreateMarkViewModel()
                {
                    ProjectId = viewModel.ProjectId,
                    UserId = viewModel.UserId,
                    Value = viewModel.Value
                });
            }
        }
    }
}
