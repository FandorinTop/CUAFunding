using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.FileUploadingViewModel
{
    public class FileUploading
    {
        public Guid Id { get; set;} 
        public IFormFile File { get; set; }
    }
}
