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
            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), FolderPath);
            var fileExtention = new List<string>();

            foreach (var item in extention)
            {
                fileExtention.Add($".{item}");
            }

            if (file.Length > 0)
            {
                var fileExt = Path.GetExtension(file.FileName);
                if (fileExtention.Contains(fileExt))
                {
                    Directory.CreateDirectory(FolderPath);
                    var filePath = Path.GetRandomFileName();
                    var fullPath = Path.Combine(fullFolderPath, filePath);

                    fullPath = File.Exists(fullPath)
                        ? fullPath.Replace(Path.GetExtension(fullPath), fileExt)
                        : fullPath.Replace(Path.GetExtension(fullPath), "_UploadedTime_" + DateTime.Now.ToFileTimeUtc() + fileExt);

                    var fileName = Path.GetFileName(fullPath);

                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }


                    return Path.Combine(FolderPath, fileName);
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
