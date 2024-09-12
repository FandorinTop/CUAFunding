using CUAFunding.Common.Helpers;
using CUAFunding.DataAccess;
using CUAFunding.DomainEntities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels;
using CUAFunding.ViewModels.EquipmentVIewModel;
using CUAFunding.ViewModels.ProjectEquipment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class ProjectEquipmentService : IProjectEquipmentService
    {
        #region Fields
        private IEquipmentService _equipmentService;
        private ApplicationDbContext _context;
        Type entityType = typeof(NeededEquipment);

        public ProjectEquipmentService(ApplicationDbContext context, IEquipmentService equipmentService)
        {
            _context = context;
            _equipmentService = equipmentService;
        }
        #endregion

        public async Task UpdateProjectEquipment(CreateProjectEquipment viewModel)
        {
            var projectEquipmentsToCreate = new List<NeededEquipment>();
            var projectEquipmentsToRemove = new List<NeededEquipment>();
            var project = await _context.Projects.FindAsync(viewModel.Id);
            var equipmentNameViewModels = viewModel.EquipmentIds.Select(item => new CreateEquipmentViewModel() { Name = item.Name });
            var equipmentNames = viewModel.EquipmentIds.Select(item => item.Name);

            if (project is null)
            {
                throw new Exception("No project with such Id");
            }

            await _equipmentService.CreateEquipments(equipmentNameViewModels);
            var eqipments = await _context.Equipments.Where(item => equipmentNames.Contains(item.Name)).ToListAsync();
            var projectEquipments = await _context.NeededEquipments
                .Include(item => item.Equipment)
                .Where(item => item.ProjectId == project.Id && equipmentNames.Contains(item.Equipment.Name)).ToListAsync();

            foreach (var item in eqipments)
            {
                var equipmentItem = viewModel.EquipmentIds.FirstOrDefault(item => item.Name == item.Name);
                var createdItem = projectEquipments.FirstOrDefault(item => item.Equipment.Name == equipmentItem.Name);

                if (createdItem is null)
                {
                    if (equipmentItem.EntityState == EntityStateViewModel.Created || equipmentItem.EntityState == EntityStateViewModel.Modified)
                    {
                        projectEquipmentsToCreate.Add(new NeededEquipment() { Project = project, Equipment = item, IsRequired = equipmentItem.IsRequired });
                    }
                }
                else
                {
                    if (equipmentItem.EntityState == EntityStateViewModel.Removed)
                    {
                        projectEquipmentsToRemove.Add(createdItem);
                    }
                    else if(equipmentItem.EntityState == EntityStateViewModel.Modified)
                    {
                        createdItem.IsRequired = equipmentItem.IsRequired;
                    }
                }
            }

            _context.NeededEquipments.UpdateRange(projectEquipments);
            _context.NeededEquipments.RemoveRange(projectEquipmentsToRemove);
            await _context.NeededEquipments.AddRangeAsync(projectEquipmentsToCreate);

            await _context.SaveChangesAsync();
        }
    }
}
