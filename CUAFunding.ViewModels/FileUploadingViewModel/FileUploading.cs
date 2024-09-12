using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.FileUploadingViewModel
{
    public class FileUploading
    {
        public string Id { get; set;} 
        public IFormFile Picture { get; set; }
    }
}
