using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using IRS.Constants;
using IRS.Data;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using IRS.Services.Base;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace IRS.Services
{
    public interface IWorkPlan2Service : IServiceBase<WorkPlan2, WorkPlan2Dto>
    {
        Task<object> GetAllWorkPlan2();
        Task ImportExcelWorkPlan2(List<WorkPlan2Dto> dto);
        Task<object> GetBuyingList();
        Task<byte[]> ExportExcelBuyingList(string lang);

    }
    public class WorkPlan2Service : ServiceBase<WorkPlan2, WorkPlan2Dto>, IWorkPlan2Service
    {
        private readonly IRepositoryBase<WorkPlan2> _repo;
        //private readonly IRepositoryBase<WorkPlan2> _repoWorkPlan2;
        private readonly IRepositoryBase<SchedulesUpdate> _repoSchedulesUpdate;
        private readonly IRepositoryBase<Models.Schedule> _repoSchedule;
        private readonly IRepositoryBase<TreatmentWay> _repoTreatmentWay;
        private readonly IRepositoryBase<Part2> _repoPart;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<Color> _repoColor;
        private readonly IRepositoryBase<Process2> _repoProcess;
        private readonly IRepositoryBase<Process> _repoTreatment;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IRepositoryBase<WorkPlanNew> _repoWorkPlanNew;
        private readonly IRepositoryBase<Ink> _repoInk;
        private readonly IRepositoryBase<InkColor> _repoInkColor;
        private readonly IRepositoryBase<Chemical2> _repoChemical;
        private readonly IRepositoryBase<ChemicalColor> _repoChemicalColor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public WorkPlan2Service(
            IRepositoryBase<WorkPlan2> repo,
            //IRepositoryBase<WorkPlan2> repoWorkPlan2,
            IRepositoryBase<SchedulesUpdate> repoSchedulesUpdate,
            IRepositoryBase<Models.Schedule> repoSchedule,
            IRepositoryBase<TreatmentWay> repoTreatmentWay,
            IRepositoryBase<Process2> repoProcess,
            IRepositoryBase<Shoe> repoShoe,
            IRepositoryBase<Process> repoTreatment,
            IRepositoryBase<Part2> repoPart,
            IRepositoryBase<Color> repoColor,
            IRepositoryBase<XAccount> repoXAccount,
            IRepositoryBase<WorkPlanNew> repoWorkPlanNew,
            IRepositoryBase<Ink> repoInk,
            IRepositoryBase<InkColor> repoInkColor,
            IRepositoryBase<Chemical2> repoChemical,
            IRepositoryBase<ChemicalColor> repoChemicalColor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoSchedulesUpdate = repoSchedulesUpdate;
            _repoSchedule = repoSchedule;
            _repoShoe = repoShoe;
            _repoProcess = repoProcess;
            _repoTreatment = repoTreatment;
            _repoTreatmentWay = repoTreatmentWay;
            _repoPart = repoPart;
            _repoColor = repoColor;
            _repoXAccount = repoXAccount;
            _repoWorkPlanNew = repoWorkPlanNew;
            _repoInk = repoInk;
            _repoInkColor = repoInkColor;
            _repoChemical = repoChemical;
            _repoChemicalColor = repoChemicalColor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            item.Status = false;
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        

        public async Task ImportExcelWorkPlan2(List<WorkPlan2Dto> res)
        {
            try
            {
                foreach (var item in res)
                {
                    var checkDuplicate = _repo.FindAll().Where(x => x.StTeam == item.StTeam && x.SfTeam == item.SfTeam && x.Pono == item.Pono && x.Batch == item.Batch && x.ModelName == item.ModelName && x.ModelNo == item.ModelNo && x.ArticleNo == item.ArticleNo && x.Qty == item.Qty && x.CutStartDate == item.CutStartDate && x.SfStartDate == item.SfStartDate).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        var item_workPlan2 = _mapper.Map<WorkPlan2>(item);
                    
                        var query = _repoShoe.FindAll().Where(x => x.ModelName == item.ModelName && x.ModelNo == item.ModelNo && x.Article1 == item.ArticleNo).FirstOrDefault();
                        if (query != null)
                        {
                            item_workPlan2.ShoeGuid = query.Guid;
                            item_workPlan2.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                            _repo.Add(item_workPlan2);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<object> GetAllWorkPlan2()
        {
            var res = await _repo.FindAll().Select(x => new
            {
                ID = x.Id,
                ST_Team = x.StTeam,
                SF_Team = x.SfTeam,
                Pono = x.Pono,
                Batch = x.Batch,
                ModelName = x.ModelName,
                ModelNo = x.ModelNo,
                ArticleNo = x.ArticleNo,
                Qty = x.Qty,
                CutStartDate = x.CutStartDate,
                SFStartDate = x.SfStartDate,
                Status = x.Status
                
            }).ToListAsync();
            var time_upload = _repo.FindAll().ToList().Count > 0 ? _repo.FindAll().ToList().LastOrDefault().CreateDate.Value.ToString("yyyy-MM-dd") : "";
            
            return new
            {
                result = res.ToList(),
                time_upload = time_upload
            };
            //throw new NotImplementedException();
        }

        public async Task<object> GetBuyingList()
        {
            var listQtyShoe = _repo.FindAll().ToList().GroupBy(x => x.ShoeGuid)
                                    .Select(x => new {
                                        ShoeGuid = x.First().ShoeGuid,
                                        Qty = x.Sum(y => y.Qty.ToInt())
                                    }).ToList();

            var listSchedules = (from a in _repoSchedule.FindAll().ToList()
                                 let Qty = listQtyShoe.Where(x => x.ShoeGuid == a.ShoesGuid).FirstOrDefault() != null 
                                            ? listQtyShoe.Where(x => x.ShoeGuid == a.ShoesGuid).FirstOrDefault().Qty : 0
                                 select new {
                                    ColorGuid = a.ColorGuid,
                                    StandardConsumption = a.Consumption.ToDouble() * Qty
                                 }).ToList();
            
            var listColorConsumption = listSchedules.GroupBy(x => x.ColorGuid)
                                        .Select(x => new {
                                            ColorGuid = x.First().ColorGuid,
                                            StandardConsumption = x.Sum(y => y.StandardConsumption).ToDouble()
                                        }).ToList();
            
            var listInkColor = (from a in _repoInkColor.FindAll().ToList()
                            join b in _repoInk.FindAll().Where(x => x.IsShow).ToList() on a.InkGuid equals b.Guid
                            let NameColor = _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault() != null
                                            ? _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault().Name : "N/A"

                            let StandardConsumption = listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault() != null
                                            ? listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault().StandardConsumption : 0
                                            
                            let NameInk = _repoInk.FindAll().Where(x => x.Guid == a.InkGuid).FirstOrDefault() != null
                                            ? _repoInk.FindAll().Where(x => x.Guid == a.InkGuid).FirstOrDefault().Name : "N/A"
                            
                            let Process = _repoTreatment.FindByID(b.ProcessId) != null
                                            ? _repoTreatment.FindByID(b.ProcessId).Name : "N/A"
                            
                            select new InkColorListDto() {
                                Guid = a.Guid,
                                ColorGuid = a.ColorGuid,
                                NameColor = NameColor,
                                InkGuid = a.InkGuid,
                                NameInk = NameInk + " (" + Process + ")",
                                Code = b.Code,
                                Percentage = a.Percentage,
                                Consumption = Math.Round((a.Percentage.ToDouble() * StandardConsumption)/100, 2)
                            }).ToList();

            var listInk = listInkColor.GroupBy(x => x.InkGuid)
                                        .Select(x => new InkListDto(){
                                            Code = x.First().Code,
                                            NameInk = x.First().NameInk,
                                            Consumption = Math.Round((x.Sum(y => y.Consumption).ToDouble()/1000),2).ToString() + " Kg"
                                        }).ToList();
           
            try
            {   
                return listInk;
                // operationResult = new OperationResult
                // {
                //     StatusCode = HttpStatusCode.OK,
                //     Message = MessageReponse.UpdateSuccess,
                //     Success = true,
                //     Data = result
                // };
                }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return listInk;
            
            // var time_upload = _repoWorkPlan2.FindAll().ToList().Count > 0 ? _repoWorkPlan2.FindAll().ToList().LastOrDefault().CreateDate.Value.ToString("yyyy-MM-dd") : "";
            // var data = res.ToList();
            // return new
            // {
            //     result = res,
            //     time_upload = time_upload
            // };
            // return data;
            //throw new NotImplementedException();
        }

        public async Task<Byte[]> ExportExcelBuyingList(string lang)
        {
            var listQtyShoe = _repo.FindAll().ToList().GroupBy(x => x.ShoeGuid)
                                    .Select(x => new {
                                        ShoeGuid = x.First().ShoeGuid,
                                        Qty = x.Sum(y => y.Qty.ToInt())
                                    }).ToList();

            var listSchedules = (from a in _repoSchedule.FindAll().ToList()
                                 let Qty = listQtyShoe.Where(x => x.ShoeGuid == a.ShoesGuid).FirstOrDefault() != null 
                                            ? listQtyShoe.Where(x => x.ShoeGuid == a.ShoesGuid).FirstOrDefault().Qty : 0
                                 select new {
                                    ColorGuid = a.ColorGuid,
                                    StandardConsumption = a.Consumption.ToDouble() * Qty
                                 }).ToList();
            
            var listColorConsumption = listSchedules.GroupBy(x => x.ColorGuid)
                                        .Select(x => new {
                                            ColorGuid = x.First().ColorGuid,
                                            StandardConsumption = x.Sum(y => y.StandardConsumption).ToDouble()
                                        }).ToList();

            ///////////////// get list ink ///////////////
            
            var listInkColor = (from a in _repoInkColor.FindAll().ToList()
                            join b in _repoInk.FindAll().Where(x => x.IsShow).ToList() on a.InkGuid equals b.Guid
                            let NameColor = _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault() != null
                                            ? _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault().Name : "N/A"

                            let StandardConsumption = listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault() != null
                                            ? listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault().StandardConsumption : 0
                                            
                            let NameInk = _repoInk.FindAll().Where(x => x.Guid == a.InkGuid).FirstOrDefault() != null
                                            ? _repoInk.FindAll().Where(x => x.Guid == a.InkGuid).FirstOrDefault().Name : "N/A"
                            
                            let Process = _repoTreatment.FindByID(b.ProcessId) != null
                                            ? _repoTreatment.FindByID(b.ProcessId).Name : "N/A"
                            
                            select new InkColorListDto() {
                                Guid = a.Guid,
                                ColorGuid = a.ColorGuid,
                                NameColor = NameColor,
                                InkGuid = a.InkGuid,
                                NameInk = NameInk + " (" + Process + ")",
                                Code = b.Code,
                                Percentage = a.Percentage,
                                Consumption = Math.Round((a.Percentage.ToDouble() * StandardConsumption)/100, 2)
                            }).ToList();

            var listInk = listInkColor.GroupBy(x => x.InkGuid)
                                        .Select(x => new InkListDto(){
                                            Code = x.First().Code,
                                            NameInk = x.First().NameInk,
                                            Consumption = Math.Round((x.Sum(y => y.Consumption).ToDouble()/1000),2).ToString() + " Kg"
                                        }).ToList();

            ///////////////// get list chemical ///////////////
            
            var listChemicalColor = (from a in _repoChemicalColor.FindAll().ToList()
                                    join b in _repoChemical.FindAll().Where(x => x.isShow).ToList() on a.ChemicalGuid equals b.Guid
                                    let NameColor = _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault() != null
                                                    ? _repoColor.FindAll().Where(x => x.Guid == a.ColorGuid).FirstOrDefault().Name : "N/A"
                                                    
                                    let StandardConsumption = listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault() != null
                                            ? listColorConsumption.Where(x => x.ColorGuid == a.ColorGuid).FirstOrDefault().StandardConsumption : 0
                                    
                                    let NameChemical = _repoChemical.FindAll().Where(x => x.Guid == a.ChemicalGuid).FirstOrDefault() != null
                                                    ? _repoChemical.FindAll().Where(x => x.Guid == a.ChemicalGuid).FirstOrDefault().Name : "N/A"

                                    let Process = _repoTreatment.FindByID(b.ProcessID) != null
                                            ? _repoTreatment.FindByID(b.ProcessID).Name : "N/A"
                                    
                                    select new ChemicalColorListDto() {
                                        Guid = a.Guid,
                                        ColorGuid = a.ColorGuid,
                                        NameColor = NameColor,
                                        ChemicalGuid = a.ChemicalGuid,
                                        NameChemical = NameChemical + " (" + Process + ")",
                                        Code = b.Code,
                                        Percentage = a.Percentage,
                                        Consumption = Math.Round((a.Percentage.ToDouble() * StandardConsumption)/100, 2)
                                    }).ToList();

            var listChemical = listChemicalColor.GroupBy(x => x.ChemicalGuid)
                                        .Select(x => new ChemicalListDto(){
                                            Code = x.First().Code,
                                            NameChemical = x.First().NameChemical,
                                            Consumption = Math.Round((x.Sum(y => y.Consumption).ToDouble()/1000),2).ToString() + " Kg"
                                        }).ToList();
            
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Huu Quynh";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "BuyingList";

                    var listInkExcelExport = new List<InkListDto>();
                    listInkExcelExport = listInk;

                    var listChemicalExcelExport = new List<ChemicalListDto>();
                    listChemicalExcelExport = listChemical;
                    
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("BuyingListInk");
                    p.Workbook.Worksheets.Add("BuyingListChemical");
                    

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["BuyingListInk"];
                    ExcelWorksheet ws2 = p.Workbook.Worksheets["BuyingListChemical"];
                    


                    // đặt tên cho sheet
                    ws.Name = "BuyingList-Ink";
                    ws2.Name = "BuyingList-Chemical";
                    
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 12;
                    ws2.Cells.Style.Font.Size = 12;
                    
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";
                    ws2.Cells.Style.Font.Name = "Calibri";

                    //////////// Sheet 1 //////////

                    var headers = new string[]{
                        "Name Ink", "Code Ink", "Total Consumption (Kg)"
                    };

                    int headerRowIndex = 1;
                    int headerColIndex = 1;
                    foreach (var header in headers)
                    {
                        int col = headerColIndex++;
                        ws.Cells[headerRowIndex, col].Value = header;
                        ws.Cells[headerRowIndex, col].Style.Font.Bold = true;
                        ws.Cells[headerRowIndex, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex = 1;
                    int rowIndex = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body in listInkExcelExport)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        //gán giá trị cho từng cell   
                                        
                        ws.Cells[rowIndex, colIndex++].Value = body.NameInk;
                        ws.Cells[rowIndex, colIndex++].Value = body.Code;
                        ws.Cells[rowIndex, colIndex++].Value = body.Consumption;
                        
                    }

                    ws.Cells[ws.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[ws.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item in headers.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item.Index + 1;
                        ws.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Column(col).AutoFit();
                    }    

                    

                    //////////// Sheet 2 //////////

                    var headers2 = new string[]{
                        "Name Chemical", "Code Chemical", "Total Consumption (Kg)"
                    };

                    int headerRowIndex2 = 1;
                    int headerColIndex2 = 1;
                    foreach (var header2 in headers2)
                    {
                        int col = headerColIndex2++;
                        ws2.Cells[headerRowIndex2, col].Value = header2;
                        ws2.Cells[headerRowIndex2, col].Style.Font.Bold = true;
                        ws2.Cells[headerRowIndex2, col].Style.Font.Size = 12;
                    }
                    // end Style
                    int colIndex2 = 1;
                    int rowIndex2 = 1;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var body2 in listChemicalExcelExport)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0 #c0514d
                        colIndex2 = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex2++;

                        //gán giá trị cho từng cell   
                                        
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.NameChemical;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.Code;
                        ws2.Cells[rowIndex2, colIndex2++].Value = body2.Consumption;
                        
                    }

                    ws2.Cells[ws2.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws2.Cells[ws2.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    foreach (var item2 in headers2.Select((x, i) => new { Value = x, Index = i }))
                    {
                        var col = item2.Index + 1;
                        ws2.Column(col).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws2.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws2.Column(col).AutoFit();
                    }  
 
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return bin;
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                Console.Write(mes);
                return new Byte[] { };
            }
        }
        
    }
}
