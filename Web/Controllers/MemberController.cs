using System.Linq;
using System.Web.Mvc;
using Core.Persistence;

namespace Web.Controllers
{
	public class MemberController : Controller
	{
		private readonly Context _context = new Context();

		[OutputCache(Duration = 60 * 60, VaryByParam="*")]
		public ActionResult Index(int? batchId)
		{
			if (!batchId.HasValue)
			{
				batchId = _context.Batches.OrderByDescending(_ => _.FetchedAt).First().BatchId;
			}

			var result = _context.Members
				.Where(_ => _.BatchId == batchId)
				.OrderBy(_ => _.Group).ThenBy(_ => _.Firstname)
				.Select(_ => new {
					firstname = _.Firstname,
					lastname = _.Lastname,
					title = _.Title,
					company = _.Company,
					vl_group = _.Group,
					propable_gender = _.ProbablyGender,
				});

			return Json(result , JsonRequestBehavior.AllowGet);
		}
	}
}
