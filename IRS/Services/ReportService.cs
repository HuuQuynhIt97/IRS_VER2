using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using IRS.Constants;
using IRS.Data;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.Services
{
    public interface IReportService
    {
        Byte[] ExcelExport(ExcelExportChartDto request);
        Byte[] ExcelExportPieChart(ExcelExportPieChartDto request);
        Task<object> GetReportType(string menuLink);
        Task<object> GetReportChartSetting(string menuLink, string lang);
        Task<object> GetReport(ReportParams reportParams);
        Task<object> GetReportChart(DateTime d1, DateTime d2, string menuLink, string lang);
    }
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryBase<SysMenu> _repo;
        private readonly IRepositoryBase<StoredProcedure> _repoStoredProcedure;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;

        public ReportService(
            IUnitOfWork unitOfWork,
            IRepositoryBase<SysMenu> repo,
            IRepositoryBase<StoredProcedure> repoStoredProcedure,
            IMapper mapper,
            MapperConfiguration configMapper,
            IConfiguration configuration
            )
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _repoStoredProcedure = repoStoredProcedure;
            _mapper = mapper;
            _configMapper = configMapper;
            _configuration = configuration;
        }

        public async Task<object> GetReportChartSetting(string menuLink, string lang)
        {
            var info = await _repo.FindAll(x => x.Status == 1 && x.MenuLink == menuLink)
                .ProjectTo<ChartSettingDto>(_configMapper)
                .FirstOrDefaultAsync();
            if (info == null)
                return new
                {
                    xAxisName = "N/A",
                    yAxisName = "N/A",
                    chartName = "N/A",
                    chartUnit = "N/A"
                };
            string xAxisName = lang == Languages.EN ? (info.ChartXAxisNameEn == "" || info.ChartXAxisNameEn == null ? info.ChartXAxisName : info.ChartXAxisNameEn) : lang == Languages.VI ? (info.ChartXAxisNameVn == "" || info.ChartXAxisNameVn == null ? info.ChartXAxisName : info.ChartXAxisNameVn) : lang == Languages.TW ? info.ChartXAxisName : lang == Languages.CN ? (info.ChartXAxisNameCn == "" || info.ChartXAxisNameCn == null ? info.ChartXAxisName : info.ChartXAxisNameCn) : info.ChartXAxisName;
            string yAxisName = lang == Languages.EN ? (info.ChartYAxisNameEn == "" || info.ChartYAxisNameEn == null ? info.ChartYAxisName : info.ChartYAxisNameEn) : lang == Languages.VI ? (info.ChartYAxisNameVn == "" || info.ChartYAxisNameVn == null ? info.ChartYAxisName : info.ChartYAxisNameVn) : lang == Languages.TW ? info.ChartYAxisName : lang == Languages.CN ? (info.ChartYAxisNameCn == "" || info.ChartYAxisNameCn == null ? info.ChartYAxisName : info.ChartYAxisNameCn) : info.ChartYAxisName;
            string chartName = lang == Languages.EN ? (info.ChartNameEn == "" || info.ChartNameEn == null ? info.ChartName : info.ChartNameEn) : lang == Languages.VI ? (info.ChartNameVn == "" || info.ChartNameVn == null ? info.ChartName : info.ChartNameVn) : lang == Languages.TW ? info.ChartName : lang == Languages.CN ? (info.ChartNameCn == "" || info.ChartNameCn == null ? info.ChartName : info.ChartNameCn) : info.ChartName;
            string chartUnit = info.ChartUnit;

            return new
            {
                xAxisName,
                yAxisName,
                chartName,
                chartUnit
            };

        }
        public async Task<object> GetReportType(string menuLink)
        {
            var item = await _repo.FindAll(x => x.Status == 1 && x.MenuLink == menuLink)
                .Select(x => new { x.ReportType, x.Guid }).FirstOrDefaultAsync();
            return item;
        }
        public async Task<string> GetStoredProceduresName(string menuLink)
        {
            var item = await _repo.FindAll(x => x.Status == 1 && x.MenuLink == menuLink)
                .Select(x => new { StoredProceduresName = x.StoredProceduresName }).FirstOrDefaultAsync();
            if (item != null)
                return item.StoredProceduresName;
            return string.Empty;
        }
        public async Task<ChartSettingDto> GetStoredProceduresInfo(string menuLink)
        {
            var item = await _repo.FindAll(x => x.Status == 1 && x.MenuLink == menuLink)
                .ProjectTo<ChartSettingDto>(_configMapper)
                .FirstOrDefaultAsync();
            if (item != null)
                return item;
            return null;
        }

        public async Task<List<StoredProcedureDto>> GetStoredProcedures(string systemMenuGuid)
        {
            var item = await _repoStoredProcedure.FindAll(x => x.Status == 1 && x.SystemMenuGuid == systemMenuGuid)
                .ProjectTo<StoredProcedureDto>(_configMapper)
                .ToListAsync();
            return item;
        }
        private async Task<object> GetDataDropdownlist(DataManager data )
        {
              var datasource = _repo.FindAll().AsQueryable();
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return await datasource.ToListAsync();
        }
        public async Task<object> GetReport(ReportParams reportParams)
        {
            var storeProcedureName = await GetStoredProceduresName(reportParams.MenuLink);
            return await StoreProcedureBase(reportParams, storeProcedureName);
        }
        public async Task<object> GetReportChart(DateTime d1, DateTime d2, string menuLink, string lang)
        {
            var info = await GetStoredProceduresInfo(menuLink);

            if (info == null)
                return new List<dynamic>() { };

            var storedProcedures = await GetStoredProcedures(info.Guid);
            if (storedProcedures.Any() == false)
                return new List<dynamic>() { };
            var data = new List<object>() { };
            var chartData = new List<ChartDataDto>() { };
            foreach (var item in storedProcedures)
            {
                var dataSource = await StoreProcedureForChartBase(d1, d2, item.StoredName, lang);
                data.Add(dataSource);

                chartData.Add(new ChartDataDto
                {
                    type = "Column",
                    xName = "x",
                    yName = "y",
                    colorName = item.Color,
                    legend = item.Legend ?? "N/A"
                });

            }
            return new
            {
                data,
                chartData
            };
        }
        private async Task<object> StoreProcedureBase(ReportParams reportParams, string storeProcedureName)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                string sql = storeProcedureName;
                reportParams.Keyword = reportParams.Keyword == null ? "" : reportParams.Keyword;
                reportParams.Sort = reportParams.Sort == null ? "" : reportParams.Sort;
                reportParams.Sort2 = reportParams.Sort2 == null ? "" : reportParams.Sort2;
                reportParams.RoomGuid1 = reportParams.RoomGuid1 == null ? "" : reportParams.RoomGuid1;
                reportParams.RoomGuid2 = reportParams.RoomGuid2 == null ? "" : reportParams.RoomGuid2;
                reportParams.MakeOrderGuid1 = reportParams.MakeOrderGuid1 == null ? "" : reportParams.MakeOrderGuid1;
                reportParams.MakeOrderGuid2 = reportParams.MakeOrderGuid2 == null ? "" : reportParams.MakeOrderGuid2;
                object parameters = new 
                {
                    Room_GUID1 = reportParams.RoomGuid1,
                    Room_GUID2 = reportParams.RoomGuid2,
                    MakeOrder_GUID1 = reportParams.MakeOrderGuid1,
                    MakeOrder_GUID2 = reportParams.MakeOrderGuid2,
                    Farm_GUID = reportParams.FarmGuid,
                    D1 = reportParams.D1,
                    D2 = reportParams.D2,
                    KeyWord = reportParams.Keyword,
                    Sort1 = reportParams.Sort,
                    Sort2 = reportParams.Sort2
                };
                if (reportParams.D1.Year == 1970)
                {
                    parameters = new 
                    {
                        Room_GUID1 = reportParams.RoomGuid1,
                        Room_GUID2 = reportParams.RoomGuid2,
                        MakeOrder_GUID1 = reportParams.MakeOrderGuid1,
                        MakeOrder_GUID2 = reportParams.MakeOrderGuid2,
                        Farm_GUID = reportParams.FarmGuid,
                        D1 = "",
                        D2 = reportParams.D2,
                        KeyWord = reportParams.Keyword,
                        Sort1 = reportParams.Sort,
                        Sort2 = reportParams.Sort2
                    };
                }
                if (reportParams.D2.Year == 1970)
                {
                    parameters = new 
                    {
                        Room_GUID1 = reportParams.RoomGuid1,
                        Room_GUID2 = reportParams.RoomGuid2,
                        MakeOrder_GUID1 = reportParams.MakeOrderGuid1,
                        MakeOrder_GUID2 = reportParams.MakeOrderGuid2,
                        Farm_GUID = reportParams.FarmGuid,
                        D1 = reportParams.D1,
                        D2 = "",
                        KeyWord = reportParams.Keyword,
                        Sort1 = reportParams.Sort,
                        Sort2 = reportParams.Sort2
                    };
                }
                if (reportParams.D1.Year == 1970 && reportParams.D2.Year == 1970)
                {
                    parameters = new 
                    {
                        Room_GUID1 = reportParams.RoomGuid1,
                        Room_GUID2 = reportParams.RoomGuid2,
                        MakeOrder_GUID1 = reportParams.MakeOrderGuid1,
                        MakeOrder_GUID2 = reportParams.MakeOrderGuid2,
                        Farm_GUID = reportParams.FarmGuid,
                        D1 = reportParams.D1,
                        D2 = reportParams.D2,
                        KeyWord = reportParams.Keyword,
                        Sort1 = reportParams.Sort,
                        Sort2 = reportParams.Sort2
                    };
                }
                try
                {
                    var data = await conn.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch 
                {
                    return new object[] { };
                }

            }
        }
        private async Task<object> StoreProcedureForChartBase(DateTime d1, DateTime d2, string storeProcedureName, string lang)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                string sql = storeProcedureName;
                var parameters = new { D1 = d1, D2 = d2, Lang = lang };
                try
                {
                    var data = await conn.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch
                {
                    return new object[] { };
                }

            }
        }


        public Byte[] ExcelExport(ExcelExportChartDto model)
        {
            try
            {
                var currentTime = DateTime.Now;
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Henry Pham";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = model.FunctionName;
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add(model.FunctionName);

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets.FirstOrDefault();

                    // đặt tên cho sheet
                    ws.Name = model.FunctionName;
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Times New Roman";
                    ws.InsertRow(1, 20);
                    ws.InsertColumn(1, 20);
                    var picture = ws.Drawings.AddPicture(model.FunctionName, LoadBase64(model.ImageBase64));
                    picture.SetPosition(2, 0, 2, 0);
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.WriteLine(mes);
                return new Byte[] { };
            }
        }

        public Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public byte[] ExcelExportPieChart(ExcelExportPieChartDto model)
        {
            try
            {
                var currentTime = DateTime.Now;
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Henry Pham";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = model.FunctionName;
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add(model.FunctionName);

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets.FirstOrDefault();

                    // đặt tên cho sheet
                    ws.Name = model.FunctionName;
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Times New Roman";
                    ws.InsertRow(1, 50);
                    ws.InsertColumn(1, 50);
                    int col = 2;
                    foreach (var img in model.ImageBase64)
                    {
                        var picture = ws.Drawings.AddPicture(Guid.NewGuid().ToString(), LoadBase64(img));
                        picture.SetPosition(2, 0, col, 0);
                        col += 8;
                    }

                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.WriteLine(mes);
                return new Byte[] { };
            }
        }
    }
}
