using CUAFunding.Common.Exceptions;
using CUAFunding.Common.Helpers;
using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.EquipmentVIewModel;
using CUAFunding.ViewModels.ProjectViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class EquipmentService : IEquipmentService
    {
        #region Fields
        private ApplicationDbContext _context;
        Type entityType = typeof(Equipment);

        public EquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<ApiResult<ShowEquipmentViewModel>> GetAllEquipment(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {

            var source = _context.Equipments.Select(item => new ShowEquipmentViewModel()
            {
                Id = item.Id,
                CreationTime = item.CreationDate,
                LastEditionTime = item.LastEditDate,
                Name = item.Name
            });

            if (!String.IsNullOrEmpty(filterColumn) && !String.IsNullOrEmpty(filterQuery) && entityType.IsValidProperty(filterColumn))
            {
                source = source.Where(String.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }

            if (!String.IsNullOrEmpty(sortColumn) && entityType.IsValidProperty(sortColumn))
            {
                var sortingString = String.Empty;

                if (!string.IsNullOrEmpty(sortColumn)
                       && entityType.IsValidProperty(sortColumn))
                {
                    sortOrder = !string.IsNullOrEmpty(sortOrder)
                        && sortOrder.ToUpper() == "ASC"
                        ? "ASC"
                        : "DESC";

                    if (string.IsNullOrEmpty(sortingString))
                    {
                        sortingString += $"{sortColumn} {sortOrder}";
                    }
                }

                source = source.OrderBy(sortingString);
            }

            var skipedSource = source.Skip(pageIndex * pageSize).Take(pageSize);
            var count = await source.CountAsync();
            var data = await skipedSource.ToListAsync();

            return new ApiResult<ShowEquipmentViewModel>(
                data,
                pageIndex,
                pageSize,
                count,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );
        }

        public async Task<ShowEquipmentViewModel> GetEquipmentById(string id)
        {
           var equipment = await _context.Equipments.FindAsync(id);
           
            if(equipment is null)
            {
                return null;
            }
            else
            {
                return new ShowEquipmentViewModel()
                {
                    Id = equipment.Id,
                    CreationTime = equipment.CreationDate,
                    LastEditionTime = equipment.LastEditDate,
                    Name = equipment.Name
                };
            }
        }

        public async Task<string> CreateEquipment(CreateEquipmentViewModel viewModel)
        {
            var equipment = new Equipment()
            {
                Name = viewModel.Name
            };

            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return equipment.Id;
        }

        public async Task<bool> EditEquipment(EditEquipmentViewModel viewModel)
        {
            var equipment = await _context.Equipments.FindAsync(viewModel.Id);
            var hasName = await _context.Equipments.AnyAsync(item => item.Name == viewModel.Name);

            if(equipment is null)
            {
                return false;
            }
            if (hasName && !(equipment.Name == viewModel.Name))
            {
                throw new Exception("Already has such name");
            }

             _context.Equipments.Update(equipment);
            await _context.SaveChangesAsync();

            return true;
        }
    }

    public class ProjectEquipmentService : IEquipmentService
    {
        #region Fields
        private ApplicationDbContext _context;
        Type entityType = typeof(Equipment);

        public ProjectEquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<ApiResult<ShowEquipmentViewModel>> GetAllEquipment(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {

            var source = _context.Equipments.Select(item => new ShowEquipmentViewModel()
            {
                Id = item.Id,
                CreationTime = item.CreationDate,
                LastEditionTime = item.LastEditDate,
                Name = item.Name
            });

            if (!String.IsNullOrEmpty(filterColumn) && !String.IsNullOrEmpty(filterQuery) && entityType.IsValidProperty(filterColumn))
            {
                source = source.Where(String.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }

            if (!String.IsNullOrEmpty(sortColumn) && entityType.IsValidProperty(sortColumn))
            {
                var sortingString = String.Empty;

                if (!string.IsNullOrEmpty(sortColumn)
                       && entityType.IsValidProperty(sortColumn))
                {
                    sortOrder = !string.IsNullOrEmpty(sortOrder)
                        && sortOrder.ToUpper() == "ASC"
                        ? "ASC"
                        : "DESC";

                    if (string.IsNullOrEmpty(sortingString))
                    {
                        sortingString += $"{sortColumn} {sortOrder}";
                    }
                }

                source = source.OrderBy(sortingString);
            }

            var skipedSource = source.Skip(pageIndex * pageSize).Take(pageSize);
            var count = await source.CountAsync();
            var data = await skipedSource.ToListAsync();

            return new ApiResult<ShowEquipmentViewModel>(
                data,
                pageIndex,
                pageSize,
                count,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );
        }

        public async Task<ShowEquipmentViewModel> GetEquipmentById(string id)
        {
            var equipment = await _context.Equipments.FindAsync(id);

            if (equipment is null)
            {
                return null;
            }
            else
            {
                return new ShowEquipmentViewModel()
                {
                    Id = equipment.Id,
                    CreationTime = equipment.CreationDate,
                    LastEditionTime = equipment.LastEditDate,
                    Name = equipment.Name
                };
            }
        }

        public async Task<string> CreateEquipment(CreateEquipmentViewModel viewModel)
        {
            var equipment = new Equipment()
            {
                Name = viewModel.Name
            };

            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return equipment.Id;
        }

        public async Task<bool> EditEquipment(EditEquipmentViewModel viewModel)
        {
            var equipment = await _context.Equipments.FindAsync(viewModel.Id);
            var hasName = await _context.Equipments.AnyAsync(item => item.Name == viewModel.Name);

            if (equipment is null)
            {
                return false;
            }
            if (hasName && !(equipment.Name == viewModel.Name))
            {
                throw new Exception("Already has such name");
            }

            _context.Equipments.Update(equipment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
