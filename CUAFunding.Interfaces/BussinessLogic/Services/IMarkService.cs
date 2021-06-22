using CUAFunding.ViewModels.MarkViewModel;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IMarkService
    {
        public Task<int> GetMark(string projectId, string userId);

        public Task<bool> AddMark(CreateMarkViewModel viewModel);

        public Task<bool> UpdateMark(EditMarkViewModel viewModel);
    }
}
