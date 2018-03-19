using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TDMUTestsServer.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}