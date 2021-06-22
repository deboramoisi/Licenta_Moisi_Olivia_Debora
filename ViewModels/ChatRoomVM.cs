using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    public class ChatRoomVM
    {
        [Required, MinLength(length: 5, ErrorMessage = "Numele camerei trebuie sa fie de minim 5 caractere")]
        public string Name { get; set; }
        [Required]
        public List<string> Users { get; set; }
    }
}
