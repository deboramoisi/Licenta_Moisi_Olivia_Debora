using System;
using System.ComponentModel.DataAnnotations;

namespace Licenta.Areas.Admin.Models.ViewModels
{
    public class DeleteSolduriBalantaVM
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime DataStart { get; set; }

        [Required]
        public DateTime DataEnd { get; set; }
    }
}
