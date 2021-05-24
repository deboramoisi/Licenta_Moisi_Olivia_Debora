using Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    public class CereriDocumenteVM
    {
        public IEnumerable<Salariat> Salariato { get; set; }
        [ForeignKey("SalariatId")]
        public int SalariatId { get; set; }

    }
}
