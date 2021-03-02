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
        Task<string> SaveDocument(IFormFile document, string Denumire, int ClientId, string UserId);
        string DeleteDocument(string fileName);
        Task<string> UpdateDocument(IFormFile document, int oldDocumentId, string Denumire, int ClientId, string UserId);

    }
}
