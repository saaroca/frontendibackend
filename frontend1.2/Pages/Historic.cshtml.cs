using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication7.Controllers;
using WebApplication7.Models;

namespace frontend1._2.Pages
{
    public class HistoricModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        EstacionsController estacionsC = new EstacionsController();
        Estacions _estacio = new Estacions();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewData["idEstacio"] = _estacio;
        }

        public void cercaHistoric(DateTime dataInici, DateTime dataFi)
        {
           double Inici = estacionsC.DataToEpoch(dataInici);
           double Fi = estacionsC.DataToEpoch(dataFi);
        }
    }
}
