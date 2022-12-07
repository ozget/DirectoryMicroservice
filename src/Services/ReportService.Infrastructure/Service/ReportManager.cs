using ReportService.Application.Service;
using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Infrastructure.Service
{
    public  class ReportManager : IReportService
    {
      
        public ReportManager()
        { 
        }

        public List<Report> GetAllReport()
        {
            throw new NotImplementedException();
        }
    }
}
