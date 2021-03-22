using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Clienti.Models
{
    public class DashboardVM
    {
        public string AnSelectat { get; set; }

        public IEnumerable<SelectListItem> Years
        {
            get
            {
                return Enumerable.Range(2004, DateTime.Today.Year - 2003)
                       .Select(i => new SelectListItem
                       {
                           Value = i.ToString(),
                           Text = i.ToString()
                       })
                       .OrderByDescending(u => u.Value);
            }
        }
    }
}
