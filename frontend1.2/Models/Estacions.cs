using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7.Models
{
    [Table("archive")]
    public class Estacions
    {

        [Required]
        [Key]
        public int dateTime { get; set; }

        public string idEstacio { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime dataHuma { get; set; }

        public int? usUnits { get; set; }

        public int? arcInt { get; set; }

        public double? barometer { get; set; }

        public double? pressure { get; set; }

        public double? altimeter { get; set; }

        public double? inTemp { get; set; }

        public double? outTemp { get; set; }

        public double? inHumidity { get; set; }

        public double? outHumidity { get; set; }

        public double? windSpeed { get; set; }

        public double? windDir { get; set; }

        public double? windGust { get; set; }

        public double? windGustDir { get; set; }

        public double? rainRate { get; set; }

        public double? rain { get; set; }

        public double? dewpoint { get; set; }

        public double? windchill { get; set; }

        public double? heatindex { get; set; }

    }
}