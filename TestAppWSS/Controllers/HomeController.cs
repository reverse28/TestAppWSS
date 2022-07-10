using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestAppWSS.Models;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index([FromServices] INodeData NodeData)
        {
            var deprtments = NodeData.GetNodesList();

            return View(deprtments);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}