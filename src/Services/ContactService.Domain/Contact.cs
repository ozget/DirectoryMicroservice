using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Domain
{
    public class Contact:BaseEntity
    {
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
