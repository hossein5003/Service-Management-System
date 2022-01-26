using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Frequency
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public int FrequencyCount { get; set; }
    }
}
