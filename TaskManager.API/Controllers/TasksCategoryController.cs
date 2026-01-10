using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers
{
    public class TasksCategoryController : Controller
    {
        // GET: TasksCategoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TasksCategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TasksCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksCategoryController/Create
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

        // GET: TasksCategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TasksCategoryController/Edit/5
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

        // GET: TasksCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TasksCategoryController/Delete/5
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
