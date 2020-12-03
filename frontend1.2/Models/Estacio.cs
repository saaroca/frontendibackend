using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7.Models
{

    public class Estacio
    {

        public string codiEstacio { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
      
    }
}