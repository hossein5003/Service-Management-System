using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository: IRepository<Frequency>
    {
        IEnumerable<SelectListItem> GetFrequencyListForDropDown();

        void Update(Frequency frequency);
    }
}
