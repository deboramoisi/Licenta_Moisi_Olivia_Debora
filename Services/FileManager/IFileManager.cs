using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.FileManager
{
    public interface IFileManager
    {
        FileStream StreamImage(string image);
        Task<string> SaveImage(IFormFile image);
        string GetFullImagePath(string image);
    }
}
