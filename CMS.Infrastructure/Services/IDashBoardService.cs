using CMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services
{
    public interface IDashBoardService
    {
        Task<DashBoardViewModel> GetData();
        Task<List<PieChartViewModel>> GetUserTypeChart();
        Task<List<PieChartViewModel>> GetContentTypeChart();
        Task<List<PieChartViewModel>> GetContentByMonthChart();
        
        }
}
