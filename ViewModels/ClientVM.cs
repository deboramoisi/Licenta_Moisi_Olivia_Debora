using Licenta.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    // specific view-ului create/edit clients
    // nu se adauga in BD
    public class ClientVM
    {
        public Client Client { get; set; }

        [Display(Name = "Furnizori")]
        public List<int> SelectedFurnizors { get; set; }

    }
}
