﻿using Licenta.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Admin.Models.ViewModels
{
    public class DashboardVM
    {
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        // Pentru afisare client selectat in view
        public string DenumireClient { get; set; }

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