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
    public interface ITreatmentWayService : IServiceBase<TreatmentWay, TreatmentWayDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<List<TreatmentWayDto>> GetAllTreatmentWay(string treatmentGuid);

    }
    public class TreatmentWayService : ServiceBase<TreatmentWay, TreatmentWayDto>, ITreatmentWayService
    {
        private readonly IRepositoryBase<TreatmentWay> _repo;
        private readonly IRepositoryBase<Process> _repoProcess;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public TreatmentWayService(
            IRepositoryBase<TreatmentWay> repo,
            IRepositoryBase<Process> repoProcess,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoProcess = repoProcess;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<OperationResult> IsExistKey(string key)
        {
            var item = await _repo.FindAll(x => x.Name == key).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "GLUE_NAME_ALREADY_EXISTED", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }

        public override async Task<OperationResult> AddAsync(TreatmentWayDto model)
        {
            try
            {
                var check = await IsExistKey(model.Name);
                if (!check.Success) return check;
                var item = _mapper.Map<TreatmentWay>(model);
                var processID = _repoProcess.FindAll(x => x.Name == model.process).FirstOrDefault().ID;
                item.ProcessId = processID;
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

        public override async Task<OperationResult> UpdateAsync(TreatmentWayDto model)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.Id == model.ID).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null )
                {
                    if (checkKey.Name != model.Name)
                    {
                        var check = await IsExistKey(model.Name);
                        if (!check.Success) return check;
                    }
                    
                }
                var item = _mapper.Map<TreatmentWay>(model);
                var processID = _repoProcess.FindAll(x => x.Name == model.process).FirstOrDefault().ID;
                item.ProcessId = processID;
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

        public override async Task<List<TreatmentWayDto>> GetAllAsync()
        {
            var data = new List<TreatmentWayDto>();
            var model = await _repo.FindAll().ToListAsync();
            data = model.Select(x => new TreatmentWayDto
            {
                ID = x.Id,
                Name = x.Name,
                NameEn = x.NameEn,
                Guid = x.Guid,
                process = _repoProcess.FindByID(x.ProcessId) != null ? _repoProcess.FindByID(x.ProcessId).Name : null
            }).Where(x => x.process != null).OrderByDescending(x => x.ID).ToList();
            return data;

        }

        public async Task<List<TreatmentWayDto>> GetAllTreatmentWay(string treatmentGuid)
        {
            var treatmentID = _repoProcess.FindAll().Where(x => x.Guid == treatmentGuid).FirstOrDefault() != null ? _repoProcess.FindAll().Where(x => x.Guid == treatmentGuid).FirstOrDefault().ID : 0;
            var data = new List<TreatmentWayDto>();
            var model = await _repo.FindAll().ToListAsync();
            data = model.Select(x => new TreatmentWayDto
            {
                ID = x.Id,
                Name = x.Name,
                NameEn = x.NameEn,
                Guid = x.Guid,
                processID = x.ProcessId,
                process = _repoProcess.FindByID(x.ProcessId) != null ? _repoProcess.FindByID(x.ProcessId).Name : null
            }).Where(x => x.process != null && x.processID == treatmentID).OrderByDescending(x => x.ID).ToList();
            return data;

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            _repo.Remove(item);
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

        public async Task<object> LoadData(DataManager data, string farmGuid)
        {
            var datasource = _repo.FindAll()
            .OrderByDescending(x=> x.Id)
            .Select(x => new TreatmentWayDto {
                ID = x.Id,
                Name = x.Name,
                Guid = x.Guid
            });
            var count = await datasource.CountAsync();
            if (data.Where != null)// for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)// for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)// for sorting
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);

            count = await datasource.CountAsync();

            if (data.Skip >= 0)// for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)// for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }

        public async Task<object> GetAudit(object id)
        {
            //var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { 
            //    x.ModifiedBy, 
            //    x.CreatedBy, 
            //    x.CreatedDate, 
            //    x.ModifiedDate 
            //}).FirstOrDefaultAsync();
            //string createBy = "N/A";
            //string createDate = "N/A";
            //string updateBy = "N/A";
            //string updateDate = "N/A";
            //if (data == null)
            //    return new
            //    {
            //        createBy,
            //        createDate,
            //        updateBy,
            //        updateDate
            //    };
            //if (data.ModifiedBy > 0)
            //{
            //    var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.ModifiedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    updateBy = updateBy != null ? updateAudit.Uid : "N/A";
            //    updateDate = data.ModifiedDate != null ? data.ModifiedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            //if (data.CreatedBy > 0)
            //{
            //    var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreatedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    createBy = createAudit != null ? createAudit.Uid : "N/A";
            //    createDate = data.CreatedDate != null ? data.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            //return new
            //{
            //    createBy,
            //    createDate,
            //    updateBy,
            //    updateDate
            //};
            throw new NotImplementedException();
        }

        public async Task<object> LoadDataBySite(string siteID)
        {
            var query = _repo.FindAll().Select(x => new { 
                x.Name
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }
    }
}
