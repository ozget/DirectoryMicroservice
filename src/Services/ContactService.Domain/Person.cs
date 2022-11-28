using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Domain
{
    public class Person:BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Firm { get; set; }
        public int MyProperty { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
