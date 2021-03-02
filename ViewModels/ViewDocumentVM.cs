using Licenta.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    public class ViewDocumentVM
    {
        public IEnumerable<SelectListItem> TipDocumente { get; set; }
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

        // Va trebui modificat in functie de data infiintarii intreprinderii!!
        public IEnumerable<SelectListItem> Years
        {
            get
            {
                return Enumerable.Range(1975, DateTime.Today.Year-1974)
                       .Select(i => new SelectListItem
                       {
                           Value = i.ToString(),
                           Text = i.ToString()
                       });
            }
        }
        public string Year { get; set; }
        public int TipulDoc { get; set; }
        public string Month { get; set; }
    }
}
