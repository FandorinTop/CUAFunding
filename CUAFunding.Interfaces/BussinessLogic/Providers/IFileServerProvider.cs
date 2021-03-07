using CUAFunding.DomainEntities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Providers
{
    public interface IFileServerProvider
    {
        Task<string> LoadFilesAsync(string FolderPath, IFormFile file, IEnumerable<EnalableFileExtensionTypes> extention);
        Task DeleteFileAsync(string path);
    }
}
