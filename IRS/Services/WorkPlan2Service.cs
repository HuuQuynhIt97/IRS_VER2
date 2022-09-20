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
    public interface IWorkPlan2Service : IServiceBase<WorkPlan2, WorkPlan2Dto>
    {
        Task<object> LoadData(string shoeGuid,string lang);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<object> GetAllWorkPlan();
        Task<object> GetAllWorkPlan2();
        Task ImportExcelWorkPlan2(List<WorkPlan2Dto> dto);

    }
    public class WorkPlan2Service : ServiceBase<WorkPlan2, WorkPlan2Dto>, IWorkPlan2Service
    {
        private readonly IRepositoryBase<WorkPlan> _repo;
        private readonly IRepositoryBase<WorkPlan2> _repoWorkPlan2;
        private readonly IRepositoryBase<SchedulesUpdate> _repoSchedulesUpdate;
        private readonly IRepositoryBase<TreatmentWay> _repoTreatmentWay;
        private readonly IRepositoryBase<Part2> _repoPart;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<Color> _repoColor;
        private readonly IRepositoryBase<Process2> _repoProcess;
        private readonly IRepositoryBase<Process> _repoTreatment;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IRepositoryBase<WorkPlanNew> _repoWorkPlanNew;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public WorkPlan2Service(
            IRepositoryBase<WorkPlan> repo,
            IRepositoryBase<WorkPlan2> repoWorkPlan2,
            IRepositoryBase<SchedulesUpdate> repoSchedulesUpdate,
            IRepositoryBase<TreatmentWay> repoTreatmentWay,
            IRepositoryBase<Process2> repoProcess,
            IRepositoryBase<Shoe> repoShoe,
            IRepositoryBase<Process> repoTreatment,
            IRepositoryBase<Part2> repoPart,
            IRepositoryBase<Color> repoColor,
            IRepositoryBase<XAccount> repoXAccount,
            IRepositoryBase<WorkPlanNew> repoWorkPlanNew,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repoWorkPlan2, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoWorkPlan2 = repoWorkPlan2;
            _repoSchedulesUpdate = repoSchedulesUpdate;
            _repoShoe = repoShoe;
            _repoProcess = repoProcess;
            _repoTreatment = repoTreatment;
            _repoTreatmentWay = repoTreatmentWay;
            _repoPart = repoPart;
            _repoColor = repoColor;
            _repoXAccount = repoXAccount;
            _repoWorkPlanNew = repoWorkPlanNew;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<OperationResult> IsExistKey(string key , string version)
        {
            var item = await _repo.FindAll(x => x.Status ).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "PART_NAME_ALREADY_EXISTED", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }

        // public override async Task<OperationResult> AddAsync(WorkPlanDto model)
        // {
        //     try
        //     {
        //         //var check = await IsExistKey(model.Name, model.Name);
        //         //if (!check.Success) return check;
        //         var item = _mapper.Map<WorkPlan>(model);
        //         //item.Name = model.Name.Trim();
        //         item.Status = true;
        //         //item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
        //         _repo.Add(item);
        //         await _unitOfWork.SaveChangeAsync();
        //         operationResult = new OperationResult
        //         {
        //             StatusCode = HttpStatusCode.OK,
        //             Message = MessageReponse.AddSuccess,
        //             Success = true,
        //             Data = model
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         operationResult = ex.GetMessageError();
        //     }
        //     return operationResult;
        // }

        // public override async Task<OperationResult> UpdateAsync(WorkPlanDto model)
        // {
        //     try
        //     {
                
        //         //var checkKey_pre = await _repo.FindAll(x => x.Name == model.Name && x.Status == 1 ).AsNoTracking().FirstOrDefaultAsync();
        //         //var checkKey = await _repo.FindAll(x => x.Id == model.Id && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
        //         //if (checkKey != null && checkKey_pre != null)
        //         //{
        //         //    if (checkKey.Name != model.Name )
        //         //    {
        //         //        var check = await IsExistKey(model.Name, model.Name);
        //         //        if (!check.Success) return check;
        //         //    }
        //         //}
        //         var item = _mapper.Map<WorkPlan>(model);
        //         //item.Name = model.Name.Trim();
        //         _repo.Update(item);
        //         await _unitOfWork.SaveChangeAsync();

        //         operationResult = new OperationResult
        //         {
        //             StatusCode = HttpStatusCode.OK,
        //             Message = MessageReponse.UpdateSuccess,
        //             Success = true,
        //             Data = model
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         operationResult = ex.GetMessageError();
        //     }
        //     return operationResult;
        // }

        // public override async Task<List<WorkPlanDto>> GetAllAsync()
        // {
        //     var query = _repo.FindAll(x => x.Status).ProjectTo<WorkPlanDto>(_configMapper);

        //     var data = await query.ToListAsync();
        //     return data;

        // }

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

        public async Task<object> LoadData(string shoeGuid, string lang)
        {
            var datasource = new List<WorkPlan>();
            //var schedule = await _repo.FindAll(x => x.Status  && x.ShoesGuid == shoeGuid).OrderByDescending(x => x.Id).ToListAsync();
            //var treatmentWay = await _repoTreatmentWay.FindAll().ToListAsync();
            //var part = await _repoPart.FindAll().ToListAsync();
            //var color = await _repoColor.FindAll().ToListAsync();
            //var datasource = (from x in schedule
            //                  join t in treatmentWay on x.TreatmentWayGuid equals t.Guid
            //                  join p in part on x.PartGuid equals p.Guid
            //                  join c in color on x.ColorGuid equals c.Guid
            //                  let process = _repoTreatment.FindAll().Where(o => o.ID == t.ProcessId).FirstOrDefault() != null
            //                      ? _repoTreatment.FindAll().Where(o => o.ID == t.ProcessId).FirstOrDefault().Name : null
            //                  select new
            //                  {
            //                      x.Id,
            //                      x.Guid,
            //                      x.ShoesGuid,
            //                      x.PartGuid,
            //                      x.Consumption,
            //                      x.ColorGuid,
            //                      x.TreatmentGuid,
            //                      x.ProcessGuid,
            //                      x.TreatmentWayGuid,
            //                      TreatmentWay = t.Name,
            //                      Part = lang == Languages.EN ? (p.PartNameEn == "" || p.PartNameEn == null ? p.Name : p.PartNameEn) 
            //                      : lang == Languages.VI ? (p.Name == "" || p.Name == null ? p.Name : p.Name) 
            //                      : lang == Languages.CN ? (p.PartNameCn == "" || p.PartNameCn == null ? p.Name : p.PartNameCn) : p.Name,
            //                      Color = c.Name,
            //                      Process = process
            //                  }).ToList();

            return datasource;
        }

        public async Task<object> GetAudit(object id)
        {
            //var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { x.UpdateBy, x.CreateBy, x.UpdateDate, x.CreateDate }).FirstOrDefaultAsync();
            string createBy = "N/A";
            string createDate = "N/A";
            string updateBy = "N/A";
            string updateDate = "N/A";
            //if (data == null)
            //    return new
            //    {
            //        createBy,
            //        createDate,
            //        updateBy,
            //        updateDate
            //    };
            //if (data.UpdateBy.HasValue)
            //{
            //    var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    updateBy = updateBy != null ? updateAudit.Uid : "N/A";
            //    updateDate = data.UpdateDate.HasValue ? data.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            //if (data.CreateBy.HasValue)
            //{
            //    var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    createBy = createAudit != null ? createAudit.Uid : "N/A";
            //    createDate = data.CreateDate.HasValue ? data.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
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
            var query = _repo.FindAll(x => x.Status).Select(x => new { 
                x.ModelName
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task ImportExcelWorkPlan2(List<WorkPlan2Dto> res)
        {
            try
            {
                foreach (var item in res)
                {
                    var checkDuplicate = _repoWorkPlan2.FindAll().Where(x => x.ST_Team == item.ST_Team && x.SF_Team == item.SF_Team && x.Pono == item.Pono && x.Batch == item.Batch && x.ModelName == item.ModelName && x.ModelNo == item.ModelNo && x.ArticleNo == item.ArticleNo && x.Qty == item.Qty && x.CutStartDate == item.CutStartDate && x.SFStartDate == item.SFStartDate).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        var item_workPlan2 = _mapper.Map<WorkPlan2>(item);
                    
                        var query = _repoShoe.FindAll().Where(x => x.ModelName == item.ModelName && x.ModelNo == item.ModelNo && x.Article1 == item.ArticleNo).FirstOrDefault();
                        if (query != null)
                        {
                            item_workPlan2.ShoeGuid = query.Guid;
                            item_workPlan2.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                            _repoWorkPlan2.Add(item_workPlan2);
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

        public async Task<object> GetAllWorkPlan()
        {
            var res = await _repo.FindAll().ToListAsync();
            var time_upload = _repo.FindAll().ToList().Count > 0 ? _repo.FindAll().ToList().LastOrDefault().CreatedTime.ToString("yyyy-MM-dd") : "";
            var items = res.GroupBy(x => new
            {
                x.ModelName,
                x.ModelNo,
                x.ArticleNo,
            }).Select(x => new
            {
                ID = x.FirstOrDefault().Id,
                ScheduleID = x.Select(a => a.ScheduleId),
                ModelName = x.FirstOrDefault().ModelName,
                ModelNo = x.FirstOrDefault().ModelNo,
                ArticleNo = x.FirstOrDefault().ArticleNo,
                Line = x.FirstOrDefault().Line,
                PONo = x.FirstOrDefault().Pono,
                Qty = x.Sum(a => a.Qty.ToDouble()),
                Stitching = x.FirstOrDefault().Stitching,
                Stockfitting = x.FirstOrDefault().Stockfitting,
                CreatedDate = x.FirstOrDefault().CreatedDate,
                CreatedTime = x.FirstOrDefault().CreatedTime.ToString("yyyy-MM-dd"),
                UploadDate = x.FirstOrDefault().CreatedTime.ToString("yyyy-MM"),
            }).OrderBy(x => x.ID);
            var data = items.Select(x => new
            {
                ID = x.ID,
                ModelName = x.ModelName,
                ModelNo = x.ModelNo,
                ArticleNo = x.ArticleNo,
                Line = x.Line,
                PONo = x.PONo,
                //Treatment = _repoSchedulesUpdate.FindAll(a => x.ScheduleID.Contains(a.Id)).Select(x => new TreatmentWorkPlanDto
                //{
                //    ID = x.Id,
                //    Treatment = x.Treatment,
                //    Status = _repoGlues.FindAll().Where(b => b.ScheduleID == x.ID).ToList().Count > 0 ? true : false,
                //    FinishedStatus = _repoWorkPlan.FindAll().Where(b => b.ScheduleID == x.ID).All(b => b.Status == true) ? true : false,
                //    Color = _repoProcess.FindAll().Where(y => y.Name == x.Treatment).FirstOrDefault().Color
                //}),
                // Treatment = _repoShoe.FindAll().Where(a => a.ModelName == x.ModelName && a.ModelNo == x.ModelNo && a.Article1 == x.ArticleNo).Select(x => new TreatmentWorkPlanDto
                // {
                // //    ID = x.ID,
                //    Treatment = _repoTreatment.FindAll().Where(b => b.Guid == x.TreatmentGuid).FirstOrDefault() != null ? _repoTreatment.FindAll().Where(b => b.Guid == x.TreatmentGuid).FirstOrDefault().Name : "",
                // //    Status = _repoGlues.FindAll().Where(b => b.ScheduleID == x.ID).ToList().Count > 0 ? true : false,
                // //    FinishedStatus = _repoWorkPlan.FindAll().Where(b => b.ScheduleID == x.ID).All(b => b.Status == true) ? true : false,
                // //    Color = _repoProcess.FindAll().Where(y => y.Name == x.Treatment).FirstOrDefault().Color
                // }),
                Qty = x.Qty,
                Stitching = x.Stitching,
                Stockfitting = x.Stockfitting,
                CreatedDate = x.CreatedDate,
                CreatedTime = x.CreatedTime,
                UploadDate = x.UploadDate,
            });
            var result = data.Select(x => new
            {
                ID = x.ID,
                ModelName = x.ModelName,
                ModelNo = x.ModelNo,
                ArticleNo = x.ArticleNo,
                Line = x.Line,
                PONo = x.PONo,
                //Treatment = x.Treatment,
                //Status = x.Treatment.ToList().All(b => b.Status == true) ? true : false,
                Qty = x.Qty,
                Stitching = x.Stitching,
                Stockfitting = x.Stockfitting,
                CreatedDate = x.CreatedDate,
                CreatedTime = x.CreatedTime,
                UploadDate = x.UploadDate
            });
            return new
            {
                result = result.ToList(),
                time_upload = time_upload
            };
            //throw new NotImplementedException();
        }

        public async Task<object> GetAllWorkPlan2()
        {
            var res = await _repoWorkPlan2.FindAll().Select(x => new
            {
                ID = x.Id,
                ST_Team = x.ST_Team,
                SF_Team = x.SF_Team,
                Pono = x.Pono,
                Batch = x.Batch,
                ModelName = x.ModelName,
                ModelNo = x.ModelNo,
                ArticleNo = x.ArticleNo,
                Qty = x.Qty,
                CutStartDate = x.CutStartDate,
                SFStartDate = x.SFStartDate,
                Status = x.Status
                
            }).ToListAsync();
            var time_upload = _repoWorkPlan2.FindAll().ToList().Count > 0 ? _repoWorkPlan2.FindAll().ToList().LastOrDefault().CreateDate.Value.ToString("yyyy-MM-dd") : "";
            
            return new
            {
                result = res.ToList(),
                time_upload = time_upload
            };
            //throw new NotImplementedException();
        }
    }
}
