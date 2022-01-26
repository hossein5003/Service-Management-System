using Microsoft.AspNetCore.Mvc;
using Core.DataAccess.Data.Repository.IRepository;
using Core.Models.ViewModels;
using Core.Models;

namespace Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        [BindProperty]
        public ServiceVM ServVM { get; set; }

        public ServiceController(IUnitOfWork iUnitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _iUnitOfWork = iUnitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServVM = new ServiceVM()
            {
                Service = new Service(),
                CategoryList = _iUnitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _iUnitOfWork.Frequency.GetFrequencyListForDropDown(),
            };

            if (id != null)
            {
                ServVM.Service = _iUnitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(ServVM);
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (ServVM.Service.Id == 0)
                {
                    //Create New Service

                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extention = Path.GetExtension(files[0].FileName);

                    using(var fileStrams= new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStrams);
                    }

                    ServVM.Service.ImageUrl = @"\images\services\"+fileName+extention;
                    _iUnitOfWork.Service.Add(ServVM.Service);
                }
                else
                {
                    //Edit the Service

                    var ServiceFromDb = _iUnitOfWork.Service.Get(ServVM.Service.Id);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extention_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, ServiceFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        using (var fileStrams = new FileStream(Path.Combine(uploads, fileName + extention_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStrams);
                        }

                        ServVM.Service.ImageUrl = @"\images\services\" + fileName + extention_new;
                    }
                    else
                    {
                        ServVM.Service.ImageUrl = ServiceFromDb.ImageUrl;
                    }

                    _iUnitOfWork.Service.Update(ServVM.Service);
                }

                _iUnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            ServVM.CategoryList = _iUnitOfWork.Category.GetCategoryListForDropDown();
            ServVM.FrequencyList = _iUnitOfWork.Frequency.GetFrequencyListForDropDown();
            return View(ServVM);
        }


        #region API Calls

        public IActionResult GetAll()
        {
            return Json(new { data = _iUnitOfWork.Service.GetAll(inculdeProperties: "Category,Frequency") });
        }

        public IActionResult Delete(int id)
        {
            var ServiceFromDb = _iUnitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;

            var imagePath = Path.Combine(webRootPath, ServiceFromDb.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (ServiceFromDb == null)
            {
                return Json(new { sucsses = false, message = "Error while deleting ..." });
            }

            _iUnitOfWork.Service.Remove(ServiceFromDb);
            _iUnitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });
        }

        #endregion
    }
}

