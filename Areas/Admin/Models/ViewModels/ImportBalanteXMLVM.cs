using Licenta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Licenta.Areas.Admin.Models.ViewModels
{
    public class ImportBalanteXMLVM
    {
        [Key]
        public int DocumentId { get; set; }

        [Display(Name = "Balanta XML")]
        public IFormFile DocumentPathUrl { get; set; }

        [Display(Name = "Client"), Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }

        public IEnumerable<SelectListItem> Years
        {
            get
            {
                return Enumerable.Range(2015, DateTime.Today.Year - 2014)
                       .Select(i => new SelectListItem
                       {
                           Value = i.ToString(),
                           Text = i.ToString()
                       });
            }
        }

        public string Year { get; set; }
        public string Month { get; set; }
    }
}
