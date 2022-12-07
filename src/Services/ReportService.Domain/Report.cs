using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Domain
{
    public class Report
    {
        public string LocationInformation { get; set; }
        public int PersonCount { get; set; }
        public int PhoneCount { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}
