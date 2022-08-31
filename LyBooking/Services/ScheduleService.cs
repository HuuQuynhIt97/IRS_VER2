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
    public interface IScheduleService : IServiceBase<Models.Schedule, ScheduleDto>
    {
        Task<object> LoadData(string shoeGuid,string lang);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task ImportExcel(List<ScheduleUploadDto> dto);

    }
    public class ScheduleService : ServiceBase<Models.Schedule, ScheduleDto>, IScheduleService
    {
        private readonly IRepositoryBase<Models.Schedule> _repo;
        private readonly IRepositoryBase<TreatmentWay> _repoTreatmentWay;
        private readonly IRepositoryBase<Part2> _repoPart;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<Color> _repoColor;
        private readonly IRepositoryBase<Process2> _repoProcess;
        private readonly IRepositoryBase<Process> _repoTreatment;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ScheduleService(
            IRepositoryBase<Models.Schedule> repo,
            IRepositoryBase<TreatmentWay> repoTreatmentWay,
            IRepositoryBase<Process2> repoProcess,
            IRepositoryBase<Shoe> repoShoe,
            IRepositoryBase<Process> repoTreatment,
            IRepositoryBase<Part2> repoPart,
            IRepositoryBase<Color> repoColor,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoShoe = repoShoe;
            _repoProcess = repoProcess;
            _repoTreatment = repoTreatment;
            _repoTreatmentWay = repoTreatmentWay;
            _repoPart = repoPart;
            _repoColor = repoColor;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<OperationResult> IsExistKey(string key , string version)
        {
            var item = await _repo.FindAll(x => x.Status == 1 ).AnyAsync();
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

        public override async Task<OperationResult> AddAsync(ScheduleDto model)
        {
            try
            {
                //var check = await IsExistKey(model.Name, model.Name);
                //if (!check.Success) return check;
                var item = _mapper.Map<Models.Schedule>(model);
                //item.Name = model.Name.Trim();
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

        public override async Task<OperationResult> UpdateAsync(ScheduleDto model)
        {
            try
            {
                
                //var checkKey_pre = await _repo.FindAll(x => x.Name == model.Name && x.Status == 1 ).AsNoTracking().FirstOrDefaultAsync();
                //var checkKey = await _repo.FindAll(x => x.Id == model.Id && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
                //if (checkKey != null && checkKey_pre != null)
                //{
                //    if (checkKey.Name != model.Name )
                //    {
                //        var check = await IsExistKey(model.Name, model.Name);
                //        if (!check.Success) return check;
                //    }
                //}
                var item = _mapper.Map<Models.Schedule>(model);
                //item.Name = model.Name.Trim();
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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

        public override async Task<List<ScheduleDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<ScheduleDto>(_configMapper);

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

        public async Task<object> LoadData(string shoeGuid, string lang)
        {
            var schedule = await _repo.FindAll(x => x.Status == 1 && x.ShoesGuid == shoeGuid).OrderByDescending(x => x.Id).ToListAsync();
            var treatmentWay = await _repoTreatmentWay.FindAll().ToListAsync();
            var part = await _repoPart.FindAll().ToListAsync();
            var color = await _repoColor.FindAll().ToListAsync();
            var datasource = (from x in schedule
                              join t in treatmentWay on x.TreatmentWayGuid equals t.Guid
                              join p in part on x.PartGuid equals p.Guid
                              join c in color on x.ColorGuid equals c.Guid
                              let process = _repoTreatment.FindAll().Where(o => o.ID == t.ProcessId).FirstOrDefault() != null
                                  ? _repoTreatment.FindAll().Where(o => o.ID == t.ProcessId).FirstOrDefault().Name : null
                              select new
                              {
                                  x.Id,
                                  x.Guid,
                                  x.ShoesGuid,
                                  x.PartGuid,
                                  x.Consumption,
                                  x.ColorGuid,
                                  x.TreatmentGuid,
                                  x.ProcessGuid,
                                  x.TreatmentWayGuid,
                                  TreatmentWay = t.Name,
                                  Part = lang == Languages.EN ? (p.PartNameEn == "" || p.PartNameEn == null ? p.Name : p.PartNameEn) 
                                  : lang == Languages.VI ? (p.Name == "" || p.Name == null ? p.Name : p.Name) 
                                  : lang == Languages.CN ? (p.PartNameCn == "" || p.PartNameCn == null ? p.Name : p.PartNameCn) : p.Name,
                                  Color = c.Name,
                                  Process = process
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

        public async Task ImportExcel(List<ScheduleUploadDto> res)
        {
            var result = res.GroupBy(x => new { 
                    x.ModelName, 
                    x.ModelNo, 
                    x.ArticelNo,
                    x.Treatment,
                    x.Process
            })
              .Select(x => new
              {

                  ModelName = x.First().ModelName,
                  ModelNo = x.First().ModelNo,
                  ArtNo = x.First().ArticelNo,
                  Treatment = x.First().Treatment,
                  Process = x.First().Process,
                  Name = x.Select(y => new {
                      y.Name,
                      y.Part,
                      y.Consumption,
                      y.TreatmentWayID
                  }),
                  
              });
            try
            {
                foreach (var item in result)
                {
                    //add shoes truoc
                    var treatmentGuid = _repoTreatment.FindAll(x => x.Name == item.Treatment).FirstOrDefault() != null
                        ? _repoTreatment.FindAll(x => x.Name == item.Treatment).FirstOrDefault().Guid : null;

                    var processGuid = _repoProcess.FindAll(x => x.Name == item.Process).FirstOrDefault() != null
                        ? _repoProcess.FindAll(x => x.Name == item.Process).FirstOrDefault().Guid : null;

                    var processID = _repoTreatment.FindAll(x => x.Name == item.Treatment).FirstOrDefault() != null
                        ? _repoTreatment.FindAll(x => x.Name == item.Treatment).FirstOrDefault().ID : 0;

                    var shoe_add = new Shoe();

                    shoe_add.ModelName = item.ModelName;
                    shoe_add.ModelNo = item.ModelNo;
                    shoe_add.Article1 = item.ArtNo;
                    shoe_add.TreatmentGuid = treatmentGuid;
                    shoe_add.ProcessGuid = processGuid;
                    var item_shoe = _mapper.Map<Shoe>(shoe_add);
                    item_shoe.Status = 1;
                    item_shoe.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                    _repoShoe.Add(item_shoe);
                    await _unitOfWork.SaveChangeAsync();

                    //add schedule

                    foreach (var item_schedule in item.Name)
                    {
                        if (item_schedule.Name != "1")
                        {
                            //tim colorGuid
                            var colorGuid = _repoColor.FindAll(x => x.Name == item_schedule.Name && x.Status == 1).FirstOrDefault() != null
                                ? _repoColor.FindAll(x => x.Name == item_schedule.Name && x.Status == 1).FirstOrDefault().Guid
                                : null;

                            var partGuid = _repoPart.FindAll(x => x.Name == item_schedule.Part && x.Status == 1).FirstOrDefault() != null
                                ? _repoPart.FindAll(x => x.Name == item_schedule.Part && x.Status == 1).FirstOrDefault().Guid
                                : null;

                            var treatmenWayGuid = _repoTreatmentWay.FindAll(o => o.Id ==  item_schedule.TreatmentWayID && o.ProcessId == processID).FirstOrDefault() != null
                               ? _repoTreatmentWay.FindAll(o => o.Id == item_schedule.TreatmentWayID && o.ProcessId == processID).FirstOrDefault().Guid
                               : null;

                            if (colorGuid != null && partGuid != null && treatmenWayGuid != null)
                            {
                                //add schedule
                                var schedule_add = new Models.Schedule();
                                schedule_add.ColorGuid = colorGuid;
                                schedule_add.PartGuid = partGuid;
                                schedule_add.Consumption = item_schedule.Consumption.ToDouble();
                                schedule_add.TreatmentWayGuid = treatmenWayGuid;
                                schedule_add.Status = 1;
                                schedule_add.ShoesGuid = item_shoe.Guid;
                                schedule_add.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                                var item_schedule_add = _mapper.Map<Models.Schedule>(schedule_add);
                                _repo.Add(item_schedule_add);

                                await _unitOfWork.SaveChangeAsync();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
