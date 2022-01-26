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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _iunitOfWork;

        public CategoryController(IUnitOfWork iunitOfWork)
        {
            _iunitOfWork = iunitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }

            category = _iunitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)   //this is create ...
                {
                    _iunitOfWork.Category.Add(category);
                }
                else                    //this is an update ...
                {
                    _iunitOfWork.Category.Update(category);
                }

                _iunitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _iunitOfWork.Category.GetAll() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objectFromDb = _iunitOfWork.Category.Get(id);
            if (objectFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting ..." });
            }

            _iunitOfWork.Category.Remove(objectFromDb);
            _iunitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        } 

        #endregion

    }
}
