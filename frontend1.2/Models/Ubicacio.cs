using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7.Models
{
    [Table("Ubicacio")]
    public class Ubicacio
    {
        [Required]
        [Key]
        public string idEstacio { get; set; }

        public string nomUbicacio { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

    }
}
