using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.DataAccess.Data.Repository.IRepository;

namespace Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _iunitOfWork;

        public FrequencyController(IUnitOfWork iunitOfWork)
        {
            _iunitOfWork = iunitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }

            frequency = _iunitOfWork.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }

            return View(frequency);
        }

        [HttpPost]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)   //this is create ...
                {
                    _iunitOfWork.Frequency.Add(frequency);
                }
                else                    //this is an update ...
                {
                    _iunitOfWork.Frequency.Update(frequency);
                }

                _iunitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(frequency);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _iunitOfWork.Frequency.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objectFromDb = _iunitOfWork.Frequency.Get(id);
            if (objectFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting ..." });
            }

            _iunitOfWork.Frequency.Remove(objectFromDb);
            _iunitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #endregion
    }
}
