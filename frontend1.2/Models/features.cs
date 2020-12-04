using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7.Models
{

    public class features
    {
        public string idEstacio { get; set; }

        public string type { get; set; }

        public geometry Geometry { get; set; }

        public properties Properties { get; set; }
    }

    public class properties
    {
        public string nomUbicacio { get; set; }
    }

    public class geometry
    {
       public string type { get; set; }

       public double[] coordinates { get; set; }
    }
}