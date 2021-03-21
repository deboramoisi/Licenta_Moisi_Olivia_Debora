using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Models
{
    public class Salariat
    {
        public int SalariatId { get; set; }
        
        [Required]
        public string Nume { get; set; }
        
        [Required]
        public string Prenume { get; set; }

        [Display(Name = "Locatie")]
        public string locatie { get; set; }

        [Required]
        public string functie { get; set; }

        [DataType(DataType.Date), Display(Name = "Data incepere"), DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? datai { get; set; }

        [Display(Name = "Tip pozitie")]
        public string tip { get; set; }
        
        [Display(Name = "Ore zi")]
        public int ore_zi { get; set; }

        [Display(Name = "Grupa")]
        public string grupa { get; set; }

        [Display(Name = "Numar zile concediu")]
        public int nr_zile_co { get; set; }

        [Display(Name = "Tip remuneratie")]
        public string tip_rem { get; set; }

        [Display(Name = "Salar brut")]
        public int salar_brut { get; set; }

        [Display(Name = "CNP")]
        public string cn { get; set; }

        [Display(Name = "Judet")]
        [Required(AllowEmptyStrings = true)]
        public string judet { get; set; }

        [Display(Name = "Localitate")]
        [Required(AllowEmptyStrings = true)]
        public string localitate { get; set; }

        [Display(Name = "Strada")]
        [Required(AllowEmptyStrings = true)]
        public string str { get; set; }

        [Display(Name = "Numar")]
        [Required(AllowEmptyStrings = true)]
        public string nr { get; set; }

        [Display(Name = "Cod postal"), MinLength(length: 6, ErrorMessage = "Codul postal trebuie sa aiba 6 caractere"), MaxLength(length: 6, ErrorMessage = "Codul postal nu poate avea mai mult de 6 caractere")]
        [Required(AllowEmptyStrings = true)]
        public string cod_post { get; set; }

        [Display(Name = "Numar contract")]
        public int nr_contr { get; set; }
        
        [DataType(DataType.Date), Display(Name = "Data contract"), DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? d_contract { get; set; }

        [ForeignKey("Client"), Display(Name = "Client")]
        public int ClientId { get; set; }

        // Navigation Property Client
        public Client Client { get; set; }

        // Nume complet pentru select list sau diferite afisari
        [NotMapped]
        [Display(Name = "Nume Prenume")]
        public string NumePrenume
        {
            get
            {
                return Nume + " " + Prenume;
            }
        }
    }
}
