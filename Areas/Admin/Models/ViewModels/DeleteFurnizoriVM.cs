using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Areas.Admin.Models.ViewModels
{
    public class DeleteFurnizoriVM
    {
        [Required]
        public int ClientId { get; set; }
    }
}
