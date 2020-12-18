using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication7.Controllers;

namespace frontend1._2.Pages
{
    public class HistoricModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        EstacionsController estacionsC = new EstacionsController();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }

        public void cercaHistoric(DateTime dataInici, DateTime dataFi)
        {
           double Inici = estacionsC.DataToEpoch(dataInici);
           double Fi = estacionsC.DataToEpoch(dataFi);
        }
    }
}
