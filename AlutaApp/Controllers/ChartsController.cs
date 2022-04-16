using AlutaApp.Data;
using AlutaApp.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlutaApp.Controllers
{
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]

        public ActionResult Charts()
        {
            return View();
        }
        // GET: ChartsController
        [HttpPost]

        public ActionResult Charts(ChartFilter chart)
        {

            if (chart.startDate == null)
            {
                chart.startDate = "2022-01-01";
            }
            if (chart.endDate == null)
            {
                chart.endDate = "2022-02-01";
            }
            ChartDataPoint data = new ChartDataPoint();
            List<int> values = new List<int>();
            HashSet<string> dates = new HashSet<string>();
            var start  =  Convert.ToDateTime(chart.startDate);
            var end  =  Convert.ToDateTime(chart.endDate);
            var getData = _context.Users.Where(s => s.TimeRegistered >= start && s.TimeRegistered <= end).OrderBy(s=>s.TimeRegistered).ToList();
            foreach (var item in getData)
            {
                dates.Add(item.TimeRegistered.ToString("yyyy-dd-MM"));
                values.Add(getData.Where(s => s.TimeRegistered == item.TimeRegistered).Count());
            }
            data.Date = dates;
            data.Value = values;
            ViewBag.Date = data.Date;
            ViewBag.Value = data.Value;
            return new JsonResult(data);
        }

        // GET: ChartsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChartsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChartsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChartsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChartsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChartsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChartsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
