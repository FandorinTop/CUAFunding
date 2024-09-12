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
        public readonly string _baseFolderPath;

        public FileServerProvider(string baseFolderPath = "ProjectFiles")
        {
            _baseFolderPath = baseFolderPath;
        }

        [Flags]
        public enum EnalableExtension
        {
            jpg = 1,
            png = 2,
            jpeg = 4,
            gif = 8,
            any = int.MaxValue
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists($"{filePath}"))
            {
                File.Delete($"{filePath}");
            }
        }

        public void ReplaceFile(string from, string to)
        {
            var dir = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(Path.Combine(dir, Path.GetDirectoryName(to)));
            File.Move(Path.Combine(dir, from), Path.Combine(dir, to));
            File.Delete(Path.Combine(dir, from));
        }

        public string GetUnicNameByPath(string folderPath, string fileName)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var fileExt = Path.GetExtension(fileName);
            var withoutExtFileName = Path.GetFileNameWithoutExtension(fileName);
            var newFilePath = Path.Combine(currentDir, folderPath, fileName);

            int i = 1;

            while (File.Exists(newFilePath))
            {

                if (!File.Exists(newFilePath.Replace(Path.GetExtension(newFilePath), $"_{i}" + fileExt)))
                {
                    newFilePath = newFilePath.Replace(Path.GetExtension(newFilePath), $"_{i}" + fileExt);
                }

                if (i > int.MaxValue / 1000)
                {
                    throw new UploadingException("Измените имя файл");
                }

                i++;
            }

            return Path.Combine(folderPath.Replace(currentDir, ""), Path.GetFileName(newFilePath));
        }

        public async Task<string> LoadFilesAsync(string FolderPath, IFormFile file, EnalableExtension extentions)
        {
            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), FolderPath);
            var ext = EnalableExtension.GetNames(typeof(EnalableExtension)).Select(item => $".{item}").ToArray();

            if (file.Length > 0)
            {
                var fileExt = Path.GetExtension(file.FileName).ToLower();
                EnalableExtension fileExtFlag;
                Enum.TryParse<EnalableExtension>(fileExt.Replace(".", ""), out fileExtFlag);

                if (extentions.HasFlag(fileExtFlag))
                {
                    Directory.CreateDirectory(FolderPath);
                    var filePath = file.FileName;
                    var fullPath = Path.Combine(fullFolderPath, filePath);
                    var newFilePath = Path.Combine(fullFolderPath, filePath);

                    int i = 1;

                    while (File.Exists(newFilePath))
                    {
                        if (!File.Exists(newFilePath.Replace(Path.GetExtension(newFilePath), $"_{i}" + fileExt)))
                        {
                            newFilePath = newFilePath.Replace(Path.GetExtension(newFilePath), $"_{i}" + fileExt);
                        }

                        i++;

                        if (i > 100000)
                        {
                            throw new UploadingException("Измените имя файл");
                        }
                    }

                    fullPath = newFilePath;

                    using (var stream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Path.Combine(FolderPath, Path.GetFileName(fullPath));
                }

                throw new UploadingException($"Неподдержиемый тип файла: {fileExt}. Подходящие расширения {ext}");
            }

            throw new UploadingException($"Слишком низкий размер файла");
        }


        public async Task<string> LoadFilesAsync(string FolderPath, IFormFile file, IEnumerable<EnalableFileExtensionTypes> extentions)
        {
            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), FolderPath);
            var fileExtention = new List<string>();

            foreach (var item in extentions)
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
