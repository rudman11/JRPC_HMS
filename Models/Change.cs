using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Models
{
    public class Change
    {
        [Key]
        public int Id { get; set; }
        public string ChangeName { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public int PercentageCompleted { get; set; }
    }
}
