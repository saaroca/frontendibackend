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
        string idEstacio { get; set; }

        string type { get; set; }

        geometry geometry { get; set; }

        properties properties { get; set; }
    }

    internal class properties
    {
        string nomUbicacio { get; set; }
    }

    internal class geometry
    {
        string type { get; set; }

        double[] coordinates { get; set; }
    }
}