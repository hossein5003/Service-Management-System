using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Core.Models.ViewModels
{
    public class ServiceVM
    {
        public Service Service { get; set;}
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? FrequencyList { get; set; }

    }
}
