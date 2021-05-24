using Grpc.Core;
using Licenta.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.FileManager
{
    public class FileManager : IFileManager
    {
        // preluare cale imagine din appsettings
        private readonly string _imagePath;
        private readonly string _documentPath;
        private readonly string _xmlPath;
        private readonly ApplicationDbContext _context;

        public FileManager(IConfiguration config, ApplicationDbContext context)
        {
            _imagePath = config["Path:ProfileImage"];
            _documentPath = config["Path:Documente"];
            _xmlPath = config["Path:Xml"];
            _context = context;
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var save_path = Path.Combine(_imagePath);
                // cautam directorul
                if (!Directory.Exists(save_path))
                {
                    // daca nu exista, il cream
                    Directory.CreateDirectory(save_path);
                }

                // extragere extensie imagine
                var extension = Path.GetExtension(image.FileName);
                // numele imaginii va fi de tip img_zi-luna-an-ora-minut-secunda.extensie
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{extension}";

                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    // folosim FileStream pentru a copia imaginea in director, pe server
                    await image.CopyToAsync(fileStream);
                }

                // returnam numele imaginii pentru a fi salvata in baza de date (fara calea absoluta - pentru securitate) 
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error";
            }
        }

        public FileStream StreamImage(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public string GetFullImagePath(string image)
        {
            var fullFileName = Path.Combine(_imagePath, image);
            return fullFileName;
        }

        [HttpGet("Image/{image}")]
        public FileStreamResult Image(string image)
        {
            var extension = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(this.StreamImage(image), $"image/{extension}");
        }


        // DOCUMENT
        #region
        // Creare Document Nou, folosim metoda SaveDocumentOnServer
        public async Task<string> SaveDocument(IFormFile document, string Denumire, int ClientId, string UserId)
        {
            try
            {
                var save_path = Path.Combine(_documentPath);
                // cautam directorul
                if (!Directory.Exists(save_path))
                {
                    // daca nu exista, il cream
                    Directory.CreateDirectory(save_path);
                }

                string fileName = await this.SaveDocumentOnServer(document, Denumire, ClientId, UserId, save_path);
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error";
            }
        }


        // Metoda pentru stergerea unui document
        public string DeleteDocument(string fileName)
        {
            string fullPath = _documentPath + "/" + fileName;
            try
            {
                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "Success";
        }

        public void DeleteDocumentXML(string fileName)
        {
            string fullPath = "wwwroot" + fileName;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        // Metoda de Update Document - se cauta documentul vechi, se sterge de pe server si se adauga documentul nou folosind metoda SaveDocumentOnServer
        public async Task<string> UpdateDocument(IFormFile document, int oldDocumentId, string Denumire, int ClientId, string UserId)
        {
            try
            {
                var oldDocument = await _context.Document.FindAsync(oldDocumentId);
                var save_path = Path.Combine(_documentPath);
                // cautam directorul
                if (!Directory.Exists(save_path))
                {
                    return "Error";
                }

                if (this.DeleteDocument(oldDocument.DocumentPath) == "Success")
                {
                    string fileName = await this.SaveDocumentOnServer(document, Denumire, ClientId, UserId, save_path);
                    return fileName;
                }

                return "Error";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error";
            }
            
        }

        // SALVARE PE SERVER A DOCUMENTULUI
        private async Task<string> SaveDocumentOnServer(IFormFile document, string Denumire, int ClientId, string UserId, string save_path)
        {
            // extragere extensie document
            var extension = Path.GetExtension(document.FileName);
            if (extension.Contains("xml")) {
                save_path = _xmlPath;
            }
            // numele im va fi de tip img_zi-luna-an-ora-minut-secunda.extensie
            var fileName = $"{Denumire}_{ClientId}_{UserId}_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{extension}";

            using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
            {
                // folosim FileStream pentru a copia documentul in director, pe server
                await document.CopyToAsync(fileStream);
            }
            // returnam numele documentului pentru a fi salvata in baza de date (fara calea absoluta - pentru securitate)
            // fileName = $"{Denumire}_{ClientId}_{UserId}_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{extension}";
            return fileName;
        }
        #endregion
    }
}
