using AppMeteo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using Pomelo.EntityFrameworkCore.MySql;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class EstacionsController : Controller
    {
        private readonly EstacionsContext _context;

        //public EstacionsControllerContext(DbContextOptions options) : base(options)
        //{

        //}
        List<Estacions> llistaEstacions = new List<Estacions>();
        List<Ubicacio> llistaUbicacions = new List<Ubicacio>();

        public EstacionsController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EstacionsContext>();

            //BUSCAR LA MANERA DE POSARO AL STARTUP ENCOMPTES D'AQUI
            optionsBuilder.UseMySql("server=hermes;port=3306;database=estacions;uid=estacio;password=estacio");

            _context = new EstacionsContext(optionsBuilder.Options);
        }

        public IActionResult Index()
        {
            Calendar();
            FindDate("", "07/11/2020", "08/11/2020");
            PintarUbicacio();
            return View(llistaEstacions);
        }

        public void Calendar()
        {

        }

        public List<features> PintarUbicacio()
        {
            llistaUbicacions = _context.Ubicacio.ToList();
            List<features> res = new List<features>();

            foreach (Ubicacio ubicacio in llistaUbicacions)
            {
                features feature = new features();
                feature.type = "Feature";

                feature.Geometry = new geometry();
                feature.Geometry.coordinates = new double?[2];
                feature.Geometry.coordinates[0] = ubicacio.Longitud;
                feature.Geometry.coordinates[1] = ubicacio.Latitud;
                feature.Geometry.type = "Point";

                feature.Properties = new properties();
                feature.Properties.nomUbicacio = ubicacio.nomUbicacio;

                feature.idEstacio = ubicacio.idEstacio;

                res.Add(feature);
            }

            return res;
        }

        public void FindDate(string idEstacio, string dataInici, string dataFi)
        {
            DateTime inici = DateTime.Parse(dataInici);
            DateTime fi = DateTime.Parse(dataFi);
            long epocinici = DataToEpoch(inici);
            long epocfi = DataToEpoch(fi);

            //FILTRAR PER CODI D'ESTACIÓ
            // && x.idEstacio == idEstacio aixo va dins del where quan tinguem Id's 
            llistaEstacions = _context.Estacio.Where(x => x.dateTime >= epocinici && x.dateTime <= epocfi).ToList();

            foreach (Estacions estacio in llistaEstacions)
            {

                //Arrodonir a  dos decimals
                estacio.pressure = Convert.ToDouble(Math.Round(Convert.ToDecimal(estacio.pressure), 2));
                estacio.barometer = Convert.ToDouble(Math.Round(Convert.ToDecimal(estacio.barometer), 2));
                estacio.dewpoint = Convert.ToDouble(Math.Round(Convert.ToDecimal(estacio.dewpoint), 2));
                estacio.altimeter = Convert.ToDouble(Math.Round(Convert.ToDecimal(estacio.altimeter), 2));

                if (estacio.usUnits == 1)
                {
                    DataConvertHuman(estacio);
                    FarenheitToCelsius(estacio);
                    knotsToKm(estacio);
                }
            }

        }

        public void FindId(string idEstacio)
        {
            // long maxData = Convert.ToInt64(_context.Estacio.Where(x => x.idEstacio == idEstacio).Select(x => x.dateTime).Max());

            // Models.Estacions estacio = _context.Estacio.Where( x => x.dateTime == maxData && x.idEstacio == idEstacio).SingleOrDefault();

            // if (estacio.usUnits == 1)
            // {
            //     DataConvertHuman(llistaEstacions);
            //     FarenheitToCelsius(llistaEstacions);
            //     knotsToKm(llistaEstacions);
            // }
        }

        //Metode que passa de farenheit a celsius
        public void FarenheitToCelsius(Estacions estacio)
        {
            //Arrodoneix a dos decimals
            estacio.inTemp = Convert.ToDouble(Math.Round(Convert.ToDecimal(5.0 / 9.0 * (estacio.inTemp - 32)), 2));
            estacio.outTemp = Convert.ToDouble(Math.Round(Convert.ToDecimal(5.0 / 9.0 * (estacio.outTemp - 32)), 2));
            estacio.heatindex = Convert.ToDouble(Math.Round(Convert.ToDecimal(5.0 / 9.0 * (estacio.heatindex - 32)), 2));
            estacio.windchill = Convert.ToDouble(Math.Round(Convert.ToDecimal(5.0 / 9.0 * (estacio.windchill - 32)), 2));
        }

        //Passa de Epoc/UNIX a lectura humana de dates
        public void DataConvertHuman(Estacions estacio)
        {
            DateTime dataHuma = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            estacio.dataHuma = dataHuma.AddSeconds(Convert.ToDouble((estacio.dateTime)));
        }

        //De nusos a km/h
        public void knotsToKm(Estacions estacio)
        {
            estacio.windSpeed = estacio.windSpeed * 1.852;
        }

        //Passa de data humana a epoc/unix
        public static long DataToEpoch(DateTime dateTime) => (int)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;


    }
}
