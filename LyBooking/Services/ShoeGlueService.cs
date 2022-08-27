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

namespace IRS.Services
{
    public interface IShoeGlueService : IServiceBase<ShoeGlue, ShoeGlueDto>
    {
        Task<object> LoadData(string glueGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<object> GetMenuPageSetting();
        Task<object> GetRecipePageSetting();
        Task ImportExcel(List<ScheduleUploadDto> scheduleUpload);

    }
    public class ShoeGlueService : ServiceBase<ShoeGlue, ShoeGlueDto>, IShoeGlueService
    {
        private readonly IRepositoryBase<ShoeGlue> _repo;
        private readonly IRepositoryBase<Chemical> _repoChemical;
        private readonly IRepositoryBase<CodeType> _repoCodeType;
        private readonly IRepositoryBase<Glue> _repoGlue;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<IRS.Models.Schedule> _repoSchedule;
        private readonly IRepositoryBase<Process2> _repoProcess2;
        private readonly IRepositoryBase<Part2> _repoPart2;
        private readonly IRepositoryBase<Color> _repoColor;
        private readonly IRepositoryBase<TreatmentWay> _repoTreatmentWay;
        private readonly IRepositoryBase<Process> _repoProcess;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ShoeGlueService(
            IRepositoryBase<ShoeGlue> repo,
            IRepositoryBase<CodeType> repoCodeType,
            IRepositoryBase<Chemical> repoChemical,
            IRepositoryBase<Glue> repoGlue,
            IRepositoryBase<XAccount> repoXAccount,
            IRepositoryBase<Shoe> repoShoe,
            IRepositoryBase<IRS.Models.Schedule> repoSchedule,
            IRepositoryBase<Process2> repoProcess2,
            IRepositoryBase<Part2> repoPart2,
            IRepositoryBase<Color> repoColor,
            IRepositoryBase<TreatmentWay> repoTreatmentWay,
            IRepositoryBase<Process> repoProcess,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoGlue = repoGlue;
            _repoCodeType = repoCodeType;
            _repoChemical = repoChemical;
            _repoXAccount = repoXAccount;
            _repoShoe = repoShoe;
            _repoSchedule = repoSchedule;
            _repoProcess2 = repoProcess2;
            _repoPart2 = repoPart2;
            _repoColor = repoColor;
            _repoTreatmentWay = repoTreatmentWay;
            _repoProcess = repoProcess;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task ImportExcel(List<ScheduleUploadDto> scheduleUpload)
        {
           var data = new List<ScheduleUploadDto>();
           data = scheduleUpload.Select(x => {
                    var partGuid = _repoPart2.FindAll().Where(k => k.Name == x.PartID).FirstOrDefault() != null ? _repoPart2.FindAll().Where(k => k.Name == x.PartID).FirstOrDefault().Guid : null;
                    var colorGuid = _repoColor.FindAll().Where(k => k.Name == x.Name).FirstOrDefault() != null ? _repoColor.FindAll().Where(k => k.Name == x.Name).FirstOrDefault().Guid : null;
                    var treatmentID = _repoProcess.FindAll(k => k.Name == x.Treatment).FirstOrDefault() != null ? _repoProcess.FindAll(k => k.Name == x.Treatment).FirstOrDefault().ID : 0;
                    var treatmentWayGuid = _repoTreatmentWay.FindAll(k => k.Id == x.TreatmentWayID && k.ProcessId == treatmentID).FirstOrDefault() != null ? _repoTreatmentWay.FindAll(k =>  k.Id == x.TreatmentWayID && k.ProcessId == treatmentID).FirstOrDefault().Guid : null;
                    return new ScheduleUploadDto  {
                        ModelName = x.ModelName,
                        ModelNo = x.ModelNo,
                        ArticleNo = x.ArticleNo,
                        Treatment = x.Treatment,
                        Process = x.Process,
                        Status = x.Status,
                        PartID = x.PartID,
                        // PartID = partGuid,
                        Name = x.Name,
                        // Name = colorGuid,
                        TreatmentWayGuid = treatmentWayGuid,
                        Consumption = x.Consumption
                    };}).ToList(); 

           var result = data.GroupBy(x => new { x.ModelName, 
                                                x.ModelNo,
                                                x.ArticleNo,
                                                x.Treatment,
                                                x.Process})
               .Select(x => 
               {
                var guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                var treatmentGuid = _repoProcess.FindAll(k => k.Name == x.Key.Treatment).FirstOrDefault() != null ? _repoProcess.FindAll(k => k.Name == x.Key.Treatment).FirstOrDefault().Guid : null;
                var processGuid = _repoProcess2.FindAll(k => k.Name == x.Key.Process).FirstOrDefault() != null ? _repoProcess2.FindAll(k => k.Name == x.Key.Process).FirstOrDefault().Guid : null;
                var createDate = DateTime.Now;
            
                return new ScheduleUploadDto
                {
                    Guid = guid,   
                    ModelName = x.Key.ModelName,
                    ModelNo = x.Key.ModelNo,
                    ArticleNo = x.Key.ArticleNo,
                    TreatmentGuid = treatmentGuid,
                    ProcessGuid = processGuid,
                    CreateDate = createDate,
                    Status = x.First().Status,
                    Schedule = x.Select( y => {
                                var guidSchedule = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                                return new ScheduleDto
                                {
                                    Guid = guidSchedule,
                                    ShoesGuid = guid,
                                    PartGuid = y.PartID,
                                    ColorGuid = y.Name,
                                    Status = y.Status,
                                    CreateDate = createDate,
                                    TreatmentWayGuid = y.TreatmentWayGuid,
                                    Consumption = y.Consumption.ToDouble()
                                };
                            }).ToList(),
                };
                }).ToList();
            
            try
            {
                foreach (var item in result)
                {
                    //add shoe truoc
                    var shoe_new = new Shoe();
                    shoe_new.Guid = item.Guid;
                    shoe_new.ModelName = item.ModelName;
                    shoe_new.ModelNo = item.ModelNo;
                    shoe_new.Article1 = item.ArticleNo;
                    shoe_new.CreateDate = item.CreateDate;
                    shoe_new.Status = item.Status;
                    shoe_new.TreatmentGuid = item.TreatmentGuid;
                    shoe_new.ProcessGuid = item.ProcessGuid;
                    
                    var shoe_add = _mapper.Map<Shoe>(shoe_new);
                    _repoShoe.Add(shoe_add);
                    await _unitOfWork.SaveChangeAsync();

                    //add tiep schedule

                    foreach (var item_schedule in item.Schedule)
                    {
                        var schedule_new = new Models.Schedule();
                        schedule_new.ShoesGuid = item_schedule.ShoesGuid;
                        schedule_new.PartGuid = item_schedule.PartGuid;
                        schedule_new.ColorGuid = item_schedule.ColorGuid;
                        schedule_new.TreatmentWayGuid = item_schedule.TreatmentWayGuid;
                        schedule_new.Guid = item_schedule.Guid;
                        schedule_new.CreateDate = item_schedule.CreateDate;
                        schedule_new.Status = item_schedule.Status;
                        schedule_new.Consumption = item_schedule.Consumption;
                        if (schedule_new.PartGuid != null && schedule_new.ColorGuid != null && schedule_new.TreatmentWayGuid != null)
                        {
                            var schedule_add = _mapper.Map<Models.Schedule>(schedule_new);

                            _repoSchedule.Add(schedule_add);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            
            //throw new NotImplementedException();
        }

        public override async Task<OperationResult> AddAsync(ShoeGlueDto model)
        {
            try
            {
                var item = _mapper.Map<ShoeGlue>(model);
                item.Status = 1;
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();

                _repo.Add(item);

                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public override async Task<List<ShoeGlueDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<ShoeGlueDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            item.Status = 0;
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

        public async Task<object> LoadData(string shoeGuid)
        {
            var shoeGlue = await _repo.FindAll(x => x.Status == 1 && x.ShoesGuid == shoeGuid)
            .OrderByDescending(x => x.Id).ToListAsync();
            var glue = await _repoGlue.FindAll().ToListAsync();
            var datasource = (from x in shoeGlue
                              join y in glue on x.GlueGuid equals y.Guid
                              select new
                              {
                                  x.Id,
                                  x.Guid,
                                  x.ShoesGuid,
                                  x.GlueGuid,
                                  x.Unit,
                                  y.Name
                              }).ToList();

            return datasource;
        }

        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { x.UpdateBy, x.CreateBy, x.UpdateDate, x.CreateDate }).FirstOrDefaultAsync();
            string createBy = "N/A";
            string createDate = "N/A";
            string updateBy = "N/A";
            string updateDate = "N/A";
            if (data == null)
                return new
                {
                    createBy,
                    createDate,
                    updateBy,
                    updateDate
                };
            if (data.UpdateBy.HasValue)
            {
                var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                updateBy = updateBy != null ? updateAudit.Uid : "N/A";
                updateDate = data.UpdateDate.HasValue ? data.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            if (data.CreateBy.HasValue)
            {
                var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                createBy = createAudit != null ? createAudit.Uid : "N/A";
                createDate = data.CreateDate.HasValue ? data.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            return new
            {
                createBy,
                createDate,
                updateBy,
                updateDate
            };
        }

        public async Task<object> LoadDataBySite(string siteID)
        {
            var query = _repo.FindAll(x => x.Status == 1).Select(x => new { 
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task<object> GetMenuPageSetting()
        {
            var pageCount = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Count).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Count).FirstOrDefault().CodeNo : "5";

            var pageSize = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Size).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Size).FirstOrDefault().CodeNo : "5";

            var pageSizes = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Sizes).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Sizes).Select(x => x.CodeNo).ToList() : null;

            return new
            {
                pageCount,
                pageSize,
                pageSizes
            };
        }

        public async Task<object> GetRecipePageSetting()
        {
            var pageCount = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Count).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Count).FirstOrDefault().CodeNo : "5";

            var pageSize = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Size).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Size).FirstOrDefault().CodeNo : "5";

            var pageSizes = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Sizes).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Sizes).Select(x => x.CodeNo).ToList() : null;

            return new
            {
                pageCount,
                pageSize,
                pageSizes
            };
        }
    }
}
