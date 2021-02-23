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

        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:ProfileImage"];
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

    }
}
