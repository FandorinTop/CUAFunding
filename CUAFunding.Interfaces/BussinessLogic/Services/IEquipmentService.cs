using CUAFunding.ViewModels;
using CUAFunding.ViewModels.EquipmentVIewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IEquipmentService
    {
        public Task<ApiResult<ShowEquipmentViewModel>> GetAllEquipment(
            int pageIndex,
            int pageSiez,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null);

        public Task<ShowEquipmentViewModel> GetEquipmentById(string id);

        public Task CreateEquipments(IEnumerable<CreateEquipmentViewModel> viewModels);

        public Task<string> CreateEquipment(CreateEquipmentViewModel viewModel);

        public Task<bool> EditEquipment(EditEquipmentViewModel viewModel);
    }
}
