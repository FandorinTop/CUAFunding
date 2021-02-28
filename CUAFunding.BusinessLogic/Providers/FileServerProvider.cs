using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Enums;
using CUAFunding.Interfaces.BussinessLogic.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Providers
{
    public class FileServerProvider : IFileServerProvider
    {
        public async Task<string> LoadFilesAsync(string FolderPath, IFormFile file, IEnumerable<EnalableFileExtensionTypes> extention)
        {
            var fileExtention = extention.ToString().Split(", ").Select(item => $".{item}").ToArray();

            if (file.Length > 0)
            {
                var fileExt = Path.GetExtension(file.FileName);
                if (fileExtention.Contains(fileExt))
                {
                    var filePath = Path.GetTempFileName();
                    filePath = File.Exists(filePath)
                        ? filePath.Replace(Path.GetExtension(filePath), fileExt)
                        : filePath.Replace(Path.GetExtension(filePath), "_UploadedTime:" + DateTime.Now.ToShortTimeString() + fileExt);

                    var path = Path.Combine(FolderPath, filePath);
                    await LoadFilesAsync(path, file, extention);
               
                    return path;
                }
            }
            throw new UploadingException($"File size is less then 1");
        }
        public async Task DeleteFileAsync(string path)
        {
            var filePath = System.IO.File.Exists(path);
            if (filePath == false)
            {
                throw new UploadingException($"File with url: {filePath} doesn`t exist");
            }
            await Task.Run(() => System.IO.File.Delete(path));
        }
    }
}
