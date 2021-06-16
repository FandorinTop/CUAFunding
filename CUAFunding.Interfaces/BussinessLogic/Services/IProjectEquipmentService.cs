using CUAFunding.ViewModels.ProjectEquipment;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IProjectEquipmentService
    {
        public Task UpdateProjectEquipment(CreateProjectEquipment viewModel);
    }
}
