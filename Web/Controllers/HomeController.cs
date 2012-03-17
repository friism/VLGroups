using System.Web.Mvc;
using Core.Persistence;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly Context _context = new Context();

		[OutputCache(Duration = 60 * 60)]
		public ActionResult Index()
		{
			return View(_context.Batches);
		}
	}
}
