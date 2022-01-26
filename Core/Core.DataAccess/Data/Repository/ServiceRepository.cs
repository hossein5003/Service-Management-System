using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.DataAccess.Data.Repository.IRepository;

namespace Core.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

       
        public void Update(Service service)
        {
            var objFromDb = _db.Service.FirstOrDefault(s => s.Id == service.Id);

            objFromDb.Name = service.Name;
            objFromDb.LongDesc = service.LongDesc;
            objFromDb.Price = service.Price;
            objFromDb.ImageUrl = service.ImageUrl;
            objFromDb.FrequencyId = service.FrequencyId;
            objFromDb.CategoryId = service.CategoryId; 

            _db.SaveChanges();
        }
    }
}
