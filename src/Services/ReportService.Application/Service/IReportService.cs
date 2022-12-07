using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Application.Service
{
    public interface IReportService
    {
        List<Report> GetAllReport();
    }
}
